using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class BA_TripleFireSpawn : BA_FireBallSpawn
{
    private BossController SavedBoss;
    public override void executeAttack(BossController boss)
    {
        base.executeAttack(boss);
        SavedBoss = boss;
    }

    private IEnumerator FireDelay()
    {
        if (SavedBoss == null)
        {
            SavedBoss = FindObjectOfType<BossController>();
        }
        if (SavedBoss != null)
        {
            for (int i = 0; i < 3; i++)
            {
                SavedBoss.setCanFlipX(false);
                base.SpawnBossAttack();
                yield return new WaitForSeconds(0.25f);
            }
            SavedBoss.setCanFlipX(true);
        }
        
    }

    public override void SpawnBossAttack()
    {
        StartCoroutine(FireDelay());
    } 

}
