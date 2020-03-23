using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Damage Spell", menuName = "RPG/Spell/Damage", order = 1)]
public class DamageSpell : Spell
{
    public DieRoll damage;
}
