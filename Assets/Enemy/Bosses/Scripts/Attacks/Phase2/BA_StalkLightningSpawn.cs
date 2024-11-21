using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_StalkLightningSpawn : BaseAttackSpawn
{
    [SerializeField] private float strikeInterval = 1f;
    [SerializeField] private float stalkDuration = 5f;
    private bool strikeAgain = true;

    public override void executeAttack(BossController boss)
    {
        strikeAgain = true;
        if (strikeAgain)
        {
            StartCoroutine(repeatStrikRoutine());
            strikeAgain = false;
        }
    }

    private IEnumerator repeatStrikRoutine()
    {
        for (int i = 0; i < stalkDuration; i++)
        {
            spawnStalkLightning();
            yield return new WaitForSeconds(strikeInterval);
        }
    }

    private void spawnStalkLightning()
    {
        GameObject strike = poolManager.getObjectFromPool(2);
        if (strike != null)
        {
            float time = strike.GetComponent<BaseBossAttack>().getAttackSpeed();
            findPlayer();
            strike.GetComponent<BA_LightningStrike>().Initialize(poolManager, playerLocation.position, time);
        }
    }


    
}
