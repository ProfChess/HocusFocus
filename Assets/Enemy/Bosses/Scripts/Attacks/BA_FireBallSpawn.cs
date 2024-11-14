using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BA_FireBallSpawn : BaseAttackSpawn
{
    //Spawn in fireball attack then get direction to start movement
    public override void executeAttack(BossController boss)
    {
        Debug.Log("Spawn FireBall");
        GameObject fireball = poolManager.getObjectFromPool(0);
        if (fireball != null)
        {
            fireball.transform.position = boss.transform.position;
            findPlayer();
            fireball.GetComponent<BA_FireBall>().Initialize(getPlayerDirection(), poolManager); 
        }
    }
        
}
