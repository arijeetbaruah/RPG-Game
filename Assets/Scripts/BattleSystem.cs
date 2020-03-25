using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateMachine
{
    START,
    PLAYERTURN,
    ENERMYTURN,
    WIN,
    LOST
}

public class BattleSystem : MonoBehaviour
{
    public Character player1;
    public Character AI;

    private int dmg;
    private OptionMenu optionMenu;
    public static BattleSystem battleSystem;

    private bool enermyTurn = false;

    public StateMachine state;

    public void Start()
    {
        state = StateMachine.START;

        optionMenu = FindObjectOfType<OptionMenu>();

        if (battleSystem == null)
        {
            battleSystem = this;
        } else
        {
            Destroy(this);
        }

        StartCoroutine(StartGame());

        state = StateMachine.PLAYERTURN;

        optionMenu.SetSpells(player1.knownSpell);
    }

    private IEnumerator StartGame()
    {
        optionMenu.SetFullText(AI.characterName + " appeared!!");

        yield return new WaitForSeconds(2f);

        optionMenu.ActiveOption1();
    }

    public void Update()
    {
        if (state == StateMachine.LOST)
        {
            optionMenu.SetFullText("You Lost");
        } else if (state == StateMachine.WIN)
        {
            optionMenu.SetFullText("You Win");
        }
    }

    public IEnumerator nextTurn()
    {
        if (state == StateMachine.PLAYERTURN)
        {
            if (player1.activeStatus.Count > 0)
            {
                foreach(StatusEffectSpell s in player1.activeStatus)
                {
                    dmg = s.damageOverTime.roll();
                    optionMenu.SetFullText(player1.characterName + " takes "+ dmg + " due to " + s.spellName);

                    yield return new WaitForSeconds(2f);

                    player1.TakeDamage(dmg);
                }
            }
            state = StateMachine.ENERMYTURN;
            AIAttack();
        } else if (state == StateMachine.ENERMYTURN)
        {
            if (AI.activeStatus.Count > 0)
            {
                foreach (StatusEffectSpell s in AI.activeStatus)
                {
                    dmg = s.damageOverTime.roll();
                    optionMenu.SetFullText(AI.characterName + " takes " + dmg + " due to " + s.spellName);

                    yield return new WaitForSeconds(2f);

                    AI.TakeDamage(dmg);
                }
            }
            state = StateMachine.PLAYERTURN;
            optionMenu.ActiveOption1();
        }
    }

    public IEnumerator PlayerCharge(float regain)
    {
        optionMenu.SetFullText(player1.characterName + " is charging");

        yield return new WaitForSeconds(2f);

        player1.SetMana(-regain);
        UpdateTurn();
    }

    public void UpdateTurn()
    {
        if (AI.hitpoint <= 0) { 
            state = StateMachine.WIN;
            return;
        } else if (player1.hitpoint <= 0)
        {
            state = StateMachine.LOST;
            return;
        }
        StartCoroutine(nextTurn());
    }

    IEnumerator PlayerCastDestructionSpellTxt(DestructionSpell destructionSpell)
    {
        if (player1.mana - destructionSpell.manaCost < 0)
        {
            optionMenu.SetFullText(player1.characterName + " is out of mana");
            yield return new WaitForSeconds(2f);
        } else {
            dmg = destructionSpell.damage.roll();
            optionMenu.SetFullText(player1.characterName + " cast " + destructionSpell.spellName + " and did " + dmg + " damage");
            yield return new WaitForSeconds(2f);

            player1.TriggerSpellAnimation();
            yield return new WaitForSeconds(2f);

            player1.SetMana(destructionSpell.manaCost);
        }

        StartCoroutine(PlayerAnimationEnd());
    }

    public void PlayerCastSpell(DestructionSpell destructionSpell)
    {
        StartCoroutine(PlayerCastDestructionSpellTxt(destructionSpell));
    }

    IEnumerator PlayerCastRestorationSpellTxt(RestorationSpell spell)
    {
        if (player1.mana - spell.manaCost < 0)
        {
            optionMenu.SetFullText(player1.characterName + " is out of mana");
            yield return new WaitForSeconds(2f);
        } else
        {
            float hp = spell.hitpoint.roll();
            optionMenu.SetFullText(player1.characterName + " cast " + spell.spellName + " and restore " + hp + " hitpoint");
            yield return new WaitForSeconds(2f);

            player1.TriggerSpellAnimation();
            yield return new WaitForSeconds(2f);

            player1.SetMana(spell.manaCost);
            player1.TakeDamage(-hp);
        }

        StartCoroutine(PlayerAnimationEnd());
    }

    public void PlayerCastSpell(RestorationSpell restorationSpell)
    {
        StartCoroutine(PlayerCastRestorationSpellTxt(restorationSpell));
    }

    IEnumerator PlayerCastStatusEffectSpellTxt(StatusEffectSpell spell)
    {
        if (player1.mana - spell.manaCost < 0)
        {
            optionMenu.SetFullText(player1.characterName + " is out of mana");
            yield return new WaitForSeconds(2f);
        } else
        {
            optionMenu.SetFullText(player1.characterName + " cast " + spell.spellName + " and poisoned " + AI.characterName);
            yield return new WaitForSeconds(2f);

            player1.TriggerSpellAnimation();
            yield return new WaitForSeconds(2f);

            player1.SetMana(spell.manaCost);
            AI.activeStatus.Add(spell);
        }        
        player1.PlayAttackAudio();

        StartCoroutine(PlayerAnimationEnd());
    }

    public void PlayerCastSpell(StatusEffectSpell spell)
    {
        StartCoroutine(PlayerCastStatusEffectSpellTxt(spell));
    }

    IEnumerator AIAttackTxt(int dmg)
    {
        optionMenu.SetFullText(AI.characterName + " attacked and did " + dmg + " damage");
        yield return new WaitForSeconds(2f);
        AI.TriggerAttackAnimation();

        StartCoroutine(AIAnimationEnd());
    }

    public void AIAttack()
    {
        dmg = AI.Attack();
        StartCoroutine(AIAttackTxt(dmg));
    }

    IEnumerator PlayerAttackTxt(int dmg)
    {
        optionMenu.SetFullText(player1.characterName + " attacked and did " + dmg + " damage");
        yield return new WaitForSeconds(2f);
        player1.TriggerAttackAnimation();
        player1.PlayAttackAudio();

        StartCoroutine(PlayerAnimationEnd());
    }

    public void PlayerAttack()
    {
        dmg = player1.Attack();
        StartCoroutine(PlayerAttackTxt(dmg));
    }

    public IEnumerator PlayerAnimationEnd()
    {
        yield return new WaitForSeconds(2f);

        AI.TakeDamage(dmg);
        dmg = 0;

        UpdateTurn();
    }

    public IEnumerator AIAnimationEnd()
    {
        yield return new WaitForSeconds(2f);

        player1.TakeDamage(dmg);
        dmg = 0;

        UpdateTurn();
    }

    public void AttackAnimationEnd()
    {
        if (state == StateMachine.PLAYERTURN)
        {
            AI.TakeDamage(dmg);
            dmg = 0;
        }
        else if (state == StateMachine.ENERMYTURN)
        {
            player1.TakeDamage(dmg);
            dmg = 0;
        }

        UpdateTurn();
    }
}
