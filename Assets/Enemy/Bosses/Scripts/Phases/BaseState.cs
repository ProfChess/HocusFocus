using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState : MonoBehaviour
{
    //List of attacks boss can use in phase 1
    [SerializeField] protected List<BaseAttackSpawn> attackList;
    [SerializeField] protected float attackCooldown; //Time between each attack

    protected virtual void getRandomAttack(BossController boss)
    {
        //Add Weight from each attack of phase to total weight
        float totalWeight = 0;
        foreach (var choice in attackList)
        {
            totalWeight += choice.getWeight();
        }

        //Choose random number for range 
        float ran = Random.Range(0, totalWeight);

        //Pick attack based on weight
        float weightcheck = 0f;
        bool oneChoice = false;
        foreach (BaseAttackSpawn choice in attackList)
        {
            weightcheck += choice.getWeight();
            if (ran <= weightcheck && oneChoice == false)
            {
                //If attack is chosen, execute attack and start small cooldown (for animation later)
                choice.executeAttack(boss);
                attackCooldown = choice.getCooldown();
                oneChoice = true;
            }
        }
    }
}
