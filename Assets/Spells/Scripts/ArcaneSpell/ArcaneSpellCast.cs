using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneSpellCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine(bool lookingRight)
    {
        yield return base.CastSpellRoutine(lookingRight);
        if (!lookingRight)
        {
            spellPrefab.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (lookingRight)
        {
            spellPrefab.transform.localScale = new Vector3(1, 1, 1);
        }

    }

    public override void spawnSpell()
    {
        GameObject arcaneStorm = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        arcaneStorm.GetComponent<ArcaneSpellDamage>().arcaneAttack();
    }
}
