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
        }else
        {
            Destroy(system);
        }
    }

    public void Start()
    {
        state = BattleState.START;

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
        enermy.currentHP -= player.dpt;
        
        dialogBox.setFullText(player.characterName + " Attacks and does " + player.dpt + " dmg");

        yield return new WaitForSeconds(2f);

        enermyHUD.SetHP(enermy.currentHP);
        state = BattleState.ENERMYTURN;

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

    IEnumerator PlayerSpellSetText(float damage)
    {
        enermy.currentHP -= damage;

        dialogBox.setFullText(player.characterName + " cast and does " + damage + " dmg");

        yield return new WaitForSeconds(2f);

        player.currentMana -= 1;

        enermyHUD.SetHP(enermy.currentHP);
        playerHUD.SetMana(player.currentMana);
        state = BattleState.ENERMYTURN;

        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public void PlayerSpell(float damage)
    {
        if (state != BattleState.PLAYERTURN)
        {
            return;
        }

        StartCoroutine(PlayerSpellSetText(damage));
    }

    IEnumerator EnermyAttackSetText()
    {
        player.currentHP -= enermy.dpt;

        dialogBox.setFullText(enermy.characterName + " Attacks and does " + enermy.dpt + " dmg");

        yield return new WaitForSeconds(2f);

        playerHUD.SetHP(player.currentHP);
        state = BattleState.PLAYERTURN;

        AIProcessing = false;
        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public void EnermyAttack()
    {

        StartCoroutine(EnermyAttackSetText());
    }

    IEnumerator EnermyAttackSpellSetText()
    {
        player.currentHP -= enermy.dpt;
        enermy.currentMana -= 1;

        dialogBox.setFullText(enermy.characterName + " Attacks and does " + enermy.dpt + " dmg");

        yield return new WaitForSeconds(2f);

        playerHUD.SetHP(player.currentHP);
        enermyHUD.SetMana(enermy.currentMana);
        state = BattleState.PLAYERTURN;

        AIProcessing = false;
        dialogBox.FullText.SetActive(false);
        SpellMenu.SetActive(false);
        dialogBox.gridText.SetActive(true);
    }

    public void EnermyAttackSpell()
    {
        StartCoroutine(EnermyAttackSpellSetText());
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
