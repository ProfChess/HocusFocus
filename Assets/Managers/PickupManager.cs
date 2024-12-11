using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    //Pickups
    [SerializeField] GameObject health;
    [SerializeField] ObjectPool healthPool;
    public GameObject spawnHealth()
    {
        PoolManager pm = GetComponent<PoolManager>();
        GameObject pickup = pm.getObjectFromPool(healthPool);
        return pickup;
    }

    public void returnHealth(GameObject obj)
    {
        PoolManager pm = GetComponent<PoolManager>();
        pm.ReturnObjectToPool(healthPool, obj);
    }

    public void returnAllHealth()
    {
        foreach (var obj in healthPool.getList())
        {
            obj.transform.position = gameObject.transform.position;
            healthPool.returnObject(obj);
        }
    }
}
