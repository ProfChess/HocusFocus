using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BA_TripleFireSpawn : BA_FireBallSpawn
{
    public override void executeAttack(BossController boss)
    {
        base.executeAttack(boss);
    }

    private IEnumerator FireDelay()
    {
        for (int i = 0; i < 3; i++)
        {
            base.SpawnBossAttack();
            yield return new WaitForSeconds(0.25f);
        }
    }

    public override void SpawnBossAttack()
    {
        StartCoroutine(FireDelay());
    } 

}
