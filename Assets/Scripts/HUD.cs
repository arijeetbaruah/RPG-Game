using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
    public TextMeshProUGUI GUIName;
    public TextMeshProUGUI GUILvl;

    public Slider hitPoint;
    public Slider manaPoint;

    public void SetGUI(Character unit) {
        GUIName.SetText(unit.characterName);
        GUILvl.SetText("lvl " + unit.level);

        hitPoint.maxValue = unit.maxHP;
        hitPoint.minValue = 0f;
        hitPoint.value = unit.currentHP;

        manaPoint.maxValue = unit.maxMana;
        manaPoint.minValue = 0f;
        manaPoint.value = unit.currentMana;
    }

    public void SetHP(float hp)
    {
        hitPoint.value = hp;
    }

    public void SetMana(float hp)
    {
        manaPoint.value = hp;
    }
}
