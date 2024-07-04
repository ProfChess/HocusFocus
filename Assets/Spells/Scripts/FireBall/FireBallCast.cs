using System.Collections;
using System.Data;
using UnityEngine;

public class FireBallCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine()
    {
        yield return base.CastSpellRoutine();
    }

    public override void spawnSpell()
    {
        base.spawnSpell();
        GameObject fireball = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        fireball.GetComponent<FireBallDamage>().setDirection(direction);
    }



}
