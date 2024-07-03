using System.Collections;
using System.Data;
using UnityEngine;

public class FireBallCast : BaseSpellCast
{
    protected override IEnumerator CastSpellRoutine(bool lookingRight)
    {
        yield return base.CastSpellRoutine(lookingRight);
    }

    public override void spawnSpell()
    {
        GameObject fireball = Instantiate(spellPrefab, spawnPoint.position, Quaternion.identity);
        fireball.GetComponent<FireBallDamage>().setDirection(direction);
    }



}
