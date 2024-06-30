using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArcaneCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine(bool lookingRight)
    {
        yield return base.CastSpellRoutine(lookingRight);
        GameObject comboSpell = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        comboSpell.GetComponent<FireArcaneDamage>().setDirection(direction);
        
    }
}
