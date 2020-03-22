using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpellType
{
    OFFENSIVE,
    STATUSEFFECT,
    RECOVERY
}

public class Spell : ScriptableObject
{
    public string spellName;
    public SpellType type;
    public float manaCost;
}
