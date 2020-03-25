using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpellButton : MonoBehaviour
{
    public Spell spell;

    public void Awake()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        text.SetText(spell.spellName);
    }

    public void Cast()
    {
        if (spell.spellType == SpellType.Destruction)
        {
            DestructionSpell destructionSpell = (DestructionSpell)spell;
            BattleSystem.battleSystem.PlayerCastSpell(destructionSpell);
        } else if (spell.spellType == SpellType.Restoration)
        {
            RestorationSpell restorationSpell = (RestorationSpell)spell;
            BattleSystem.battleSystem.PlayerCastSpell(restorationSpell);
        } else if (spell.spellType == SpellType.StatusEffect)
        {
            StatusEffectSpell statusEffectSpell = (StatusEffectSpell)spell;
            BattleSystem.battleSystem.PlayerCastSpell(statusEffectSpell);
        }
    }
}
