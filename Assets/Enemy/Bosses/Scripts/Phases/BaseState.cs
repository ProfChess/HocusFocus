using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    //List of attacks boss can use in phase 1
    [SerializeField] protected List<BaseAttackSpawn> attackList;
    [SerializeField] protected float attackCooldown; //Time between each attack

    protected virtual void getRandomAttack(BossController boss)
    {
        BaseAttackSpawn attack = attackList[Random.Range(0, attackList.Count)];
        attack.executeAttack(boss);
        attackCooldown = attack.getCooldown();
    }
}
