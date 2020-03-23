using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recovery Spell", menuName = "RPG/Spell/Recovery", order = 2)]
public class RecoverySpell : Spell
{
    public DieRoll hitpoints = new DieRoll(2, 4);
}
