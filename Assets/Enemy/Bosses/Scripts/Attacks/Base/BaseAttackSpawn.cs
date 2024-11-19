using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackSpawn : MonoBehaviour, IBossAttack
{
    [SerializeField] protected GameObject prefab;
    protected float cooldown;
    protected PoolManager poolManager;
    private Transform playerLocation;
    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
        cooldown = prefab.GetComponent<BaseBossAttack>().getAttackFrequency();
    }

    public float getCooldown()
    {
        return cooldown;
    }

    protected void findPlayer()
    {
        playerLocation = GameManager.Instance.player.transform;
    }
    
    protected GameObject getPlayer()
    {
        return GameManager.Instance.player;
    }

    protected Vector2 getPlayerDirection()
    {
        Vector2 dir = playerLocation.position - transform.position;
        return dir;
    }
    public abstract void executeAttack(BossController boss);

}
