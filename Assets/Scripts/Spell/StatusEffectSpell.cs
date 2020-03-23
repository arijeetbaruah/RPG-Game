using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatusEffect
{
    POISON,
    FREEZE,
    BURNING
}

[CreateAssetMenu(fileName = "Status Effect Spell", menuName = "RPG/Spell/Status Effect", order = 3)]
public class StatusEffectSpell : Spell
{
    public StatusEffect statusEffect;
    public bool isDamaging;
    public DieRoll damage;
}
