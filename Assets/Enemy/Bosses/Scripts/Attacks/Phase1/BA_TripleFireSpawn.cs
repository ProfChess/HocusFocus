using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_TripleFireSpawn : BA_FireBallSpawn
{
    public override void executeAttack(BossController boss)
    {
        StartCoroutine(FireDelay(boss));
    }

    private IEnumerator FireDelay(BossController boss)
    {
        for (int i = 0; i < 3; i++)
        {
            base.executeAttack(boss);
            yield return new WaitForSeconds(0.25f);
        }
        
    }

}
