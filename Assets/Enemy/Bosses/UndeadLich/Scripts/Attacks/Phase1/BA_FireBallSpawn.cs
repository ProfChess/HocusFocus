using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class BA_FireBallSpawn : BaseAttackSpawn
{
    [SerializeField] GameObject SpellLeftSpawn;
    [SerializeField] GameObject SpellRightSpawn;
    protected Vector2 SpellLocation;

    //Spawn in fireball attack then get direction to start movement
    public override void executeAttack(BossController boss)
    {
        if (SpellLeftSpawn != null && SpellRightSpawn != null)
        {
            SpellLocation = boss.GetComponentInParent<BossController>().getSpriteRendererFlip() ?
                SpellLeftSpawn.transform.position : SpellRightSpawn.transform.position;
        }
        base.executeAttack(boss);
    }

    public override void SpawnBossAttack()
    {
        GameObject fireball = GetPoolManager();
        if (fireball != null)
        {
            fireball.transform.position = SpellLocation;
            findPlayer();
            fireball.GetComponent<BA_FireBall>().Initialize(getPlayerDirection(), poolManager, GetObjectPool());
        }
    }



}
