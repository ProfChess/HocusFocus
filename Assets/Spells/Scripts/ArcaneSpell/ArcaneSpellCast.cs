using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneSpellCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine()
    {
        yield return base.CastSpellRoutine();
    }

    public override void spawnSpell()
    {
        base.spawnSpell();
        GameObject arcaneStorm = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        arcaneStorm.GetComponent<ArcaneSpellDamage>().arcaneAttack();
    }
}
