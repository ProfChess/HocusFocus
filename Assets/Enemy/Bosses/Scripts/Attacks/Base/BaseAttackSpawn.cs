using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackSpawn : MonoBehaviour, IBossAttack
{
    //Variables
    [SerializeField] protected float weight;            //Likely to be chosen -> Bigger number = More likely
    [SerializeField] protected float duration;          //Duration of ability including gap between next cast

    //References
    [SerializeField] protected GameObject prefab;       //Prefab to use
    [SerializeField] protected ObjectPool objPool;      //Pool to get/return objects from
    protected PoolManager poolManager;                  //Pool manager
    protected Transform playerLocation;                 //Player transform
    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
    }

    public float getCooldown()
    {
        return duration;
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

    //Pool get
    public ObjectPool GetObjectPool()
    {
        return objPool;
    }

    protected GameObject GetPoolManager()
    {
        return poolManager.getObjectFromPool(GetObjectPool());
    }
    public abstract void executeAttack(BossController boss);

}
