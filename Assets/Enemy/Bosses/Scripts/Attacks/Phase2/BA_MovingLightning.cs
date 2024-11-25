using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_MovingLightning : BaseBossAttack
{
    //Variables
    private Vector2 spawnLocation;   //Spawn of object
    private bool stopAttack = false; //Trigger to return object
    private bool startMoving = false;//Trigger to start moving 
    public void Initialize(PoolManager pm, Vector2 spawn)
    {
        stopAttack = false;
        startMoving = false;
        poolManager = pm;   
        spawnLocation = spawn;

        gameObject.transform.position = spawnLocation;
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
            returnMovingLightning();
        }
        else if (startMoving)
        {
            transform.Translate(Vector2.left * attackSpeed * Time.deltaTime);
        }
       
    }


    //Return object to pool to reuse later
    private void returnMovingLightning()
    {
        poolManager.ReturnObjectToPool(3, gameObject);
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
