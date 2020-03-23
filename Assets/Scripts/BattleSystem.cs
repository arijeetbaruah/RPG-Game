using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum BattleState {
    START,
    PLAYERTURN,
    ENERMYTURN,
    WON,
    LOST
}

[System.Serializable]
public class DieRoll
{
    public int number;
    public int die;

    public DieRoll()
    {
        number = 0;
        die = 0;
    }

    public DieRoll(int n, int d)
    {
        number = n;
        die = d;
    }

    public int roll()
    {
        int sum = 0;

        for (int i = 0; i < number; i++)
        {
            sum += Random.Range(1, die);
        }

        return sum;
    }
}

public class BattleSystem : MonoBehaviour
{
    public static BattleSystem system;
    public BattleState state;

    public GameObject playerPrefab;
    public GameObject enermyPrefab;
    public GameObject SpellMenuButtonPrefab;

    public Transform playerPlatform;
    public Transform enermyPlatform;

    Character player;
    Character enermy;

    public HUD playerHUD;
    public HUD enermyHUD;

    public DialogBox dialogBox;
    public GameObject SpellMenu;

    private bool AIProcessing = false;

    BattleSystem()
    {
        if (system == null)
        {
            system = this;
        }
        else
        {
            Destroy(system);
        }
    }

    public void Start()
    {
        state = BattleState.START;
        Random.InitState(System.DateTime.Now.Millisecond);

        StartCoroutine(SetupBattle());
    }

    IEnumerator SetupBattle()
    {
        GameObject p = Instantiate(playerPrefab, playerPlatform);
        player = p.GetComponent<Character>();
        p = Instantiate(enermyPrefab, enermyPlatform);
        enermy = p.GetComponent<Character>();

        playerHUD.SetGUI(player);
        enermyHUD.SetGUI(enermy);

        foreach (SpellList sl in player.knownSpells)
        {
            GameObject button = Instantiate(SpellMenuButtonPrefab, SpellMenu.transform);
            TextMeshProUGUI meshPro = button.GetComponentInChildren<TextMeshProUGUI>();
            meshPro.SetText(sl.spell.spellName);
            button.GetComponent<SpellButton>().spellData = sl.spell;
        }

        dialogBox.setFullText("You are challanged by " + enermy.characterName);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;

        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    IEnumerator PlayerAttackSetText()
    {
        float hit = (new DieRoll(1, 20)).roll();

        if (hit == 1)
        {
            dialogBox.setFullText(player.characterName + " missed");

            yield return new WaitForSeconds(2f);

            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        }
        else
        {
            float damage = player.dpt.roll();
            if (hit == 20)
            {
                damage += player.dpt.roll();
            }
            enermy.currentHP -= damage;

            dialogBox.setFullText(player.characterName + " Attacks and does " + damage + " dmg");

            yield return new WaitForSeconds(2f);

            enermyHUD.SetHP(enermy.currentHP);
            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(true);
        }

        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public void PlayerAttack()
    {
        if (state != BattleState.PLAYERTURN) {
            return;
        }

        StartCoroutine(PlayerAttackSetText());
    }

    public void PlayerSelectSpell()
    {
        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(true);
        dialogBox.gridText.SetActive(false);
    }

    public void SpellBack()
    {
        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public IEnumerator UpdateStatus(Character character)
    {
        foreach(StatusEffectSpell spell in character.currentEffects) {
            if (spell.isDamaging)
            {
                float damage = spell.damage.roll();
                dialogBox.setFullText(character.characterName + " take " + damage + " damage by poison");
                yield return new WaitForSeconds(2f);

                character.currentHP -= damage;
                playerHUD.SetHP(player.currentHP);
                enermyHUD.SetHP(enermy.currentHP);

                dialogBox.FullText.SetActive(false);
                SpellMenu.SetActive(false);
                dialogBox.gridText.SetActive(true);
            }
        }
    }

    IEnumerator PlayerSpellHealSetText(string name, float hitpoint, float cost)
    {
        float hit = (new DieRoll(1, 20)).roll();

        if (hit == 1)
        {
            dialogBox.setFullText(player.characterName + " missed");

            yield return new WaitForSeconds(2f);

            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        }
        else
        {
            player.currentHP += hitpoint;

            dialogBox.setFullText(player.characterName + " cast " + name + " and recovers " + hitpoint + " hitpoints");

            yield return new WaitForSeconds(2f);

            player.currentMana -= cost;

            playerHUD.SetMana(player.currentMana);
            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(true);
        }

        StartCoroutine(UpdateStatus(player));

        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    IEnumerator PlayerSpellStatusSetText(StatusEffectSpell spell)
    {
        float hit = (new DieRoll(1, 20)).roll();

        if (hit == 1)
        {
            dialogBox.setFullText(player.characterName + " missed");

            yield return new WaitForSeconds(2f);

            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        }
        else
        {
            dialogBox.setFullText(player.characterName + " cast " + spell.spellName + " on " + enermy.characterName);

            yield return new WaitForSeconds(2f);

            player.currentMana -= spell.manaCost;

            playerHUD.SetMana(player.currentMana);
            enermy.currentEffects.Add(spell);
            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(true);
        }

        StartCoroutine(UpdateStatus(player));

        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    IEnumerator PlayerSpellSetText(string name, float damage, float cost)
    {
        float hit = (new DieRoll(1, 20)).roll();

        if (hit == 1)
        {
            dialogBox.setFullText(player.characterName + " missed");

            yield return new WaitForSeconds(2f);

            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        }
        else
        {
            enermy.currentHP -= damage;

            dialogBox.setFullText(player.characterName + " cast " + name + " and does " + damage + " dmg");

            yield return new WaitForSeconds(2f);

            player.currentMana -= cost;

            enermyHUD.SetHP(enermy.currentHP);
            playerHUD.SetMana(player.currentMana);
            state = BattleState.ENERMYTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(true);
        }

        StartCoroutine(UpdateStatus(player));

        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public void PlayerSpell(DamageSpell spell)
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerSpellSetText(spell.spellName, spell.damage.roll(), spell.manaCost));
    }

    public void PlayerSpell(RecoverySpell spell)
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerSpellHealSetText(spell.spellName, spell.hitpoints.roll(), spell.manaCost));
    }

    public void PlayerSpell(StatusEffectSpell spell)
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerSpellStatusSetText(spell));
    }

    IEnumerator EnermyAttackSetText()
    {
        float hit = (new DieRoll(1, 20)).roll();

        if (hit == 1) {
            dialogBox.setFullText(enermy.characterName + " missed");

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        }
        else
        {
            float damage = enermy.dpt.roll();
            if (hit == 20)
            {
                damage += enermy.dpt.roll();
            }
            player.currentHP -= damage;

            dialogBox.setFullText(enermy.characterName + " Attacks and does " + damage + " dmg");

            yield return new WaitForSeconds(2f);

            playerHUD.SetHP(player.currentHP);
            state = BattleState.PLAYERTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        }

        StartCoroutine(UpdateStatus(enermy));

        AIProcessing = false;
        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public void EnermyAttack()
    {

        StartCoroutine(EnermyAttackSetText());
        StartCoroutine(UpdateStatus(enermy));
    }

    IEnumerator EnermyAttackSpellSetText()
    {
        float hit = (new DieRoll(1, 20)).roll();
        if (hit == 1)
        {
            dialogBox.setFullText(enermy.characterName + " missed");

            yield return new WaitForSeconds(2f);

            state = BattleState.PLAYERTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        } else
        {
            float damage = enermy.dpt.roll();
            if (hit == 20)
            {
                damage += enermy.dpt.roll();
            }

            player.currentHP -= damage;
            enermy.currentMana -= 1;

            dialogBox.setFullText(enermy.characterName + " Attacks and does " + damage + " dmg");

            yield return new WaitForSeconds(2f);

            playerHUD.SetHP(player.currentHP);
            enermyHUD.SetMana(enermy.currentMana);
            state = BattleState.PLAYERTURN;

            dialogBox.FullText.SetActive(false);
            SpellMenu.SetActive(false);
            dialogBox.gridText.SetActive(false);
        }

        StartCoroutine(UpdateStatus(enermy));

        AIProcessing = false;
        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public void EnermyAttackSpell()
    {
        StartCoroutine(EnermyAttackSpellSetText());
        StartCoroutine(UpdateStatus(enermy));
    }

    public void Update()
    {
        if (state == BattleState.ENERMYTURN)
        {
            if (AIProcessing)
            {
                return;
            }
            AIProcessing = true;
    
            if (enermy.currentMana > 0)
            {
                EnermyAttackSpell();
            } else
            {
                EnermyAttack();
            }
        }
    }
}
