using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    Destruction,
    Restoration,
    StatusEffect
}

public class Spell : ScriptableObject
{
    public string spellName;
    public SpellType spellType;
    public float manaCost = 1;
}
