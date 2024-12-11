using System.Collections;
using UnityEngine;

public class BA_StalkLightningSpawn : BaseAttackSpawn
{
    //Variables
    [SerializeField] private float strikeInterval = 1f; //How often lightning strikes
    [SerializeField] private float stalkDuration = 5f;  //Amount of time of lightning
    private bool strikeAgain = true;                    //Bool to repeat a strike

    public override void executeAttack(BossController boss)
    {
        strikeAgain = true;
        if (strikeAgain)
        {
            StartCoroutine(repeatStrikRoutine());
            strikeAgain = false;
        }
    }

    //Coroutine for striking players location repeatably
    private IEnumerator repeatStrikRoutine()
    {
        for (int i = 0; i < stalkDuration; i++)
        {
            spawnStalkLightning();
            yield return new WaitForSeconds(strikeInterval);
        }
    }

    //Spawn lightning strike at players location
    private void spawnStalkLightning()
    {
        GameObject strike = GetPoolManager();
        if (strike != null)
        {
            float time = strike.GetComponent<BaseBossAttack>().getAttackSpeed();
            findPlayer();
            strike.GetComponent<BA_LightningStrike>().Initialize(poolManager, playerLocation.position, time, true, GetObjectPool());
        }
    }


    
}
