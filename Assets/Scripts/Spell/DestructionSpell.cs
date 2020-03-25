using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Destruction", menuName = "RPG/Spell/Destruction", order = 1)]
public class DestructionSpell : Spell
{
    public DieRoll damage;
}
