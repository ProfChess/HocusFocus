using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAttackSpawn : MonoBehaviour, IBossAttack
{
    [SerializeField] protected GameObject prefab;
    protected PoolManager poolManager;
    private Transform playerLocation;
    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
    }

    protected void findPlayer()
    {
        playerLocation = GameManager.Instance.player.transform;
        Debug.Log(playerLocation);
    }

    protected Vector2 getPlayerDirection()
    {
        Vector2 dir = playerLocation.position - transform.position;
        return dir;
    }
    public abstract void executeAttack(BossController boss);

}
