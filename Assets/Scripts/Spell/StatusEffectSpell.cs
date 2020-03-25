using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Effect", menuName = "RPG/Spell/Status Effect", order = 3)]
public class StatusEffectSpell : Spell
{
    public DieRoll damageOverTime;
}
