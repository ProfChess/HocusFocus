using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneIceCast : BaseSpellCast
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
        GameObject comboSpell = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        comboSpell.GetComponent<ArcaneIceDamage>().AOEFreeze();
    }
}
