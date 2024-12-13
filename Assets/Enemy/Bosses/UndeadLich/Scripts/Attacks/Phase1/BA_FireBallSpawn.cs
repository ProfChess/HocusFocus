using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BA_FireBallSpawn : BaseAttackSpawn
{
    private BossController SavedBoss;
    //Spawn in fireball attack then get direction to start movement
    public override void executeAttack(BossController boss)
    {
        base.executeAttack(boss);
        SavedBoss = boss;
    }

    public override void SpawnBossAttack()
    {
        GameObject fireball = GetPoolManager();
        if (fireball != null)
        {
            fireball.transform.position = SavedBoss.transform.position;
            findPlayer();
            fireball.GetComponent<BA_FireBall>().Initialize(getPlayerDirection(), poolManager, GetObjectPool());
        }
    }
        
}
