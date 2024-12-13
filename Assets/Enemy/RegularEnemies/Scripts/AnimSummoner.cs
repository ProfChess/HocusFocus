using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimSummoner : MonoBehaviour
{
    private EnemyHealthScript enemyHP;
    public Animator enemyAnim;
    public SummonerEnemy enemy;

    void Start()
    {
        enemyHP = GetComponentInParent<EnemyHealthScript>();
    }
    public void enemyDeath()
    {
        enemyHP.dyingSucks();
    }

    public void enemyHPCheck()
    {
        if (enemyHP.getEnemyCurrentHealth() <= 0)
        {
            enemyAnim.SetTrigger("Death");
        }
    }

    public void summonCall()
    {
        string summonType = enemy.summonType;
        if (summonType == "Single")
        {
            enemy.basicSummonAbility();
        }
        if (summonType == "Double")
        {
            enemy.runSummonAbility();
        }
    }

}
