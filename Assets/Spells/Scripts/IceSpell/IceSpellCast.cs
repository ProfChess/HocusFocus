
using UnityEngine;
using System.Collections;

public class IceSpellCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine()
    {
        yield return base.CastSpellRoutine();
    }

    public override void spawnSpell()
    {
        base.spawnSpell();
        GameObject IceSpell = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        IceSpell.GetComponent<IceSpellDamage>().setDirection(direction);
    }
}
