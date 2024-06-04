
using UnityEngine;
using System.Collections;

public class IceSpellCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine(bool lookingRight)
    {
        yield return base.CastSpellRoutine(lookingRight);
        GameObject IceSpell = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        IceSpell.GetComponent<IceSpellDamage>().setDirection(direction);
    }
}
