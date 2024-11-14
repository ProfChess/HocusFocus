using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public ObjectPool fireballPool;
    //public ObjectPool explosionPool;
    //public ObjectPool lightningPool;

    //List of different pools and associated numbers 
    //0 = fireball, 1 = explosion, 2 = lightning
    public GameObject getObjectFromPool(int num) 
    {
        switch (num) { 
            case 0:
                return fireballPool.getObject();
            default:
                Debug.Log("No pool found for id:" + num);
                return null;
        }

    }

    //Returns object to correct pool based on number given
    public void ReturnObjectToPool(int num,  GameObject obj)
    {
        switch (num)
        {
            case 0:
                fireballPool.returnObject(obj);
                break;
            default:
                Debug.Log("No pool found for id:" + num);
                break;
        }
    }
}
