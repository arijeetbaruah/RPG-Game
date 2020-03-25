using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionMenu : MonoBehaviour
{
    public GameObject option1;
    public GameObject optionMagic;
    public GameObject fullText;

    public GameObject magicListContent;
    public GameObject spellCastButtonPrefab;

    public void SetFullText(string txt)
    {
        TextMeshProUGUI gui = fullText.GetComponentInChildren<TextMeshProUGUI>();
        gui.SetText(txt);

        option1.SetActive(false);
        fullText.SetActive(true);
        optionMagic.SetActive(false);
    }

    public void Charge(float regain)
    {
        StartCoroutine(BattleSystem.battleSystem.PlayerCharge(regain));
    }

    public void ActiveOption1()
    {
        option1.SetActive(true);
        fullText.SetActive(false);
        optionMagic.SetActive(false);
    }

    public void ActiveMagic()
    {
        option1.SetActive(false);
        fullText.SetActive(false);
        optionMagic.SetActive(true);
    }

    public void SetSpells(Spell[] spells)
    {
        foreach(Spell s in spells)
        {
            GameObject newSpellBtn = Instantiate(spellCastButtonPrefab, magicListContent.transform);
            SpellButton sb = newSpellBtn.GetComponent<SpellButton>();

            sb.spell = s;
        }
    }
}
