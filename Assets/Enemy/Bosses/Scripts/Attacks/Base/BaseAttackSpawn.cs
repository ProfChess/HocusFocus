using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackSpawn : MonoBehaviour, IBossAttack
{
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected float weight;
    protected float cooldown;
    protected PoolManager poolManager;
    protected Transform playerLocation;
    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
        cooldown = prefab.GetComponent<BaseBossAttack>().getAttackFrequency();
    }

    public float getCooldown()
    {
        return cooldown;
    }

    public float getWeight()
    {
        return weight;
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
