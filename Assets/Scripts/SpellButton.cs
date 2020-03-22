using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellButton : MonoBehaviour
{
    public Spell spellData;

    public void Cast()
    {
        if (spellData.type == SpellType.OFFENSIVE)
        {
            DamageSpell damageSpell = (DamageSpell) spellData;
            BattleSystem.system.PlayerSpell(damageSpell.damage);

            BattleSystem.system.dialogBox.FullText.SetActive(false);
            BattleSystem.system.SpellMenu.SetActive(true);
            BattleSystem.system.dialogBox.gridText.SetActive(false);
        }
    }
}
