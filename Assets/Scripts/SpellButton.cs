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
            DamageSpell damageSpell = (DamageSpell)spellData;
            BattleSystem.system.PlayerSpell(damageSpell);
        } else if (spellData.type == SpellType.RECOVERY)
        {
            RecoverySpell recoverySpell = (RecoverySpell)spellData;
            BattleSystem.system.PlayerSpell(recoverySpell);
        } else if (spellData.type == SpellType.STATUSEFFECT)
        {
            StatusEffectSpell statusSpell = (StatusEffectSpell)spellData;
            BattleSystem.system.PlayerSpell(statusSpell);
        }
    }
}
