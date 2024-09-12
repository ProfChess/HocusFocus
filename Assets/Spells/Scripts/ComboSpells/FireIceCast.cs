using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIceCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine()
    {
        yield return base.CastSpellRoutine();
    }

    public override void spawnSpell()
    {
        base.spawnSpell();
        GameObject comboSpell = Instantiate(spellPrefab, gameObject.transform.position, Quaternion.identity);
        comboSpell.GetComponent<FireIceDamage>().AOEBlind();
    }

}
