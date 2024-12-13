using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BA_LStrikeFinish : BaseAttackSpawn
{
    [SerializeField] private List<Transform> platforms;
    public override void executeAttack(BossController boss)
    {
        base.executeAttack(boss);
    }

    public override void SpawnBossAttack()
    {
        foreach (Transform t in platforms)
        {
            GameObject Strike = GetPoolManager();
            if (Strike != null)
            {
                float duration = Strike.GetComponent<BaseBossAttack>().getAttackSpeed();
                Strike.GetComponent<BA_LightningStrike>().Initialize(poolManager, t.position, duration, true, GetObjectPool());
            }
        }
    }
}
