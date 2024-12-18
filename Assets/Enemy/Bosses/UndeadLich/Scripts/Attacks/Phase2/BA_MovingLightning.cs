using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_MovingLightning : BaseBossAttack
{
    //Variables
    private Vector2 spawnLocation;   //Spawn of object
    private bool stopAttack = false; //Trigger to return object
    private bool startMoving = false;//Trigger to start moving 
    public void Initialize(PoolManager pm, Vector2 spawn, ObjectPool objpool)
    {
        stopAttack = false;
        startMoving = false;
        poolManager = pm;   
        pool = objpool;
        spawnLocation = spawn;

        gameObject.transform.position = spawnLocation;
        AudioManager.Instance.playBossSound("Lightning");
    }

    private void Update()
    {
        if (gameObject.transform.position.x <= -10)
        {
            stopAttack = true;
        }
        if(stopAttack)
        {
            startMoving = false;
            returnGameObject();
        }
        else if (startMoving)
        {
            transform.Translate(Vector2.left * attackSpeed * Time.deltaTime);
        }
       
    }


    //Get and Set for Movement trigger
    public bool getMoveBool()
    {
        return startMoving;
    }

    public void setMoveBool()
    {
        startMoving = true;
    }


}
