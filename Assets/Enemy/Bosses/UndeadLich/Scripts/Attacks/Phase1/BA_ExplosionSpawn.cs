using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_ExplosionSpawn : BaseAttackSpawn
{
    public override void executeAttack(BossController boss)
    {
        base.executeAttack(boss);

    }

    public override void SpawnBossAttack()
    {
        GameObject explosion = GetPoolManager();
        if (explosion != null)
        {
            explosion.GetComponent<BA_Explosion>().Initialize(Random.Range(2, 4), poolManager, getPlayer(), GetObjectPool());
        }
    }
}
