using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SpellList
{
    public int lvl;
    public Spell spell;
}

public class Character : MonoBehaviour
{
    public string characterName;
    public int level;

    public DieRoll dpt = new DieRoll(1, 4);

    public float maxHP;
    public float currentHP;

    public float maxMana;
    public float currentMana;

    public SpellList[] knownSpells;
    public List<StatusEffectSpell> currentEffects;
}
