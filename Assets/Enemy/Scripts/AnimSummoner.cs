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
        enemyHP.onHealthChanged += enemyHPCheck;
    }
    public void enemyDeath()
    {
        enemyHP.dyingSucks();
    }

    public void enemyHPCheck(float health)
    {
        if (health <= 0)
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

    private void OnDestroy()
    {
        enemyHP.onHealthChanged -= enemyHPCheck;
    }
}
