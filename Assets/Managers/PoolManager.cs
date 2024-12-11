using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public List<ObjectPool> pools;

    public GameObject getObjectFromPool(ObjectPool obj) 
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i] == obj)
            {
                return obj.getObject();
            }
        }
        Debug.Log("Pool is not found");
        return null;

    }

    //Returns object to correct pool based on number given
    public void ReturnObjectToPool(ObjectPool obj, GameObject self)
    {
        for (int i = 0; i < pools.Count; i++)
        {
            if (pools[i] == obj)
            {
                obj.returnObject(self);
                return;
            }
        }
        Debug.Log("Pool is not found");
        return;
    }
}
