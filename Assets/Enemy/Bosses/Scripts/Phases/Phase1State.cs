using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Phase1State : MonoBehaviour, IBossPhase
{
    //List of attacks boss can use in phase 1
    public List<BaseAttackSpawn> attackList;

    private float attackCooldown = 1f; //Time inbetween each attack
    public void EnterPhase(BossController boss)     //Enters phase 
    {
        Debug.Log("Phase 1");
        attackCooldown = 0f;
    }
    public void UpdatePhase(BossController boss)    //For whatever needs to be updated each frame in each phase
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            BaseAttackSpawn attack = attackList[Random.Range(0, attackList.Count)];
            attack.executeAttack(boss);
            attackCooldown = 1f;
        }
    }
    public void ExitPhase(BossController boss)      //Final part before leaving phase
    {
        Debug.Log("Leaving Phase 1");
    }
}
