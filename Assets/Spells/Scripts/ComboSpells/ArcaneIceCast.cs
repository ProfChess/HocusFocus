using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneIceCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine()
    {
        yield return base.CastSpellRoutine();
    }

    public override void spawnSpell()
    {
        base.spawnSpell();
        GameObject comboSpell = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
    }
}
