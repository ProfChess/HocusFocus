using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_LStrikeFinish : BaseAttackSpawn
{
    [SerializeField] private List<Transform> platforms;
    public override void executeAttack(BossController boss)
    {
        foreach (Transform t in platforms)
        {
            GameObject Strike = poolManager.getObjectFromPool(2);
            if (Strike != null)
            {
                float duration = Strike.GetComponent<BaseBossAttack>().getAttackSpeed();
                Strike.GetComponent<BA_LightningStrike>().Initialize(poolManager, t.position, duration);
            }
        }
    }
}
