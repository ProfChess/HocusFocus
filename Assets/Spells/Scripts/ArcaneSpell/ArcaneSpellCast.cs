using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneSpellCast : BaseSpellCast
{
    float arcaneSpawnMod = 4f;
    private Vector2 arcaneSpellPosition;
    protected override IEnumerator CastSpellRoutine(bool lookingRight)
    {
        yield return base.CastSpellRoutine(lookingRight);
        if (spawnPoint == rightSpellSpawn)
        {   
            arcaneSpellPosition = spawnPoint.position;
            arcaneSpellPosition.x += arcaneSpawnMod;
        }
        else
        {
            arcaneSpellPosition = spawnPoint.position;
            arcaneSpellPosition.x -= arcaneSpawnMod;
        }
        GameObject arcaneStorm = Instantiate(spellPrefab, arcaneSpellPosition, Quaternion.identity);
        arcaneStorm.GetComponent<ArcaneSpellDamage>().arcaneAttack();
    }
}
