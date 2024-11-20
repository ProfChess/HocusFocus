using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_LightningStrike : BaseBossAttack
{
    //Variables 
    Vector2 location;             //Place to strike
    private float timer;            //How long to last after strike
    private bool stopAttack = false;//Controls when to return to pool

    //References
    BoxCollider2D hitbox;
    SpriteRenderer sr;

    private void Awake() //assign references
    {
        hitbox = GetComponent<BoxCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (stopAttack)
        {
            hitbox.enabled = false;
            returnLightning(); //returns to pool
        }
        else
        {

        }
    }
    public void Initialize(PoolManager pm, Vector2 place, float duration)
    {
        //Reset Variables
        hitbox.enabled = false;
        stopAttack = false;

        //Assign
        poolManager = pm;
        location = place;
        timer = duration;



        lightningWarning(0);
        StartCoroutine(strikeDelay());
    }

    //Strike Delay Coroutine
    private IEnumerator strikeDelay()
    {
        float delayTime = 1f;
        Vector3 c = gameObject.transform.position;
        c.x = location.x;
        gameObject.transform.position = c;
        yield return new WaitForSeconds(delayTime);

        //Turn to damage color -> collision on -> stay for desired time -> flag to return to pool
        lightningWarning(2);
        hitbox.enabled = true;
        yield return new WaitForSeconds(timer);
        stopAttack = true;
    }

    //Specifics for returning to correct pool
    protected override void returnGameObject()
    {
        returnLightning();
    }

    private void returnLightning()
    {
        poolManager.ReturnObjectToPool(2, gameObject);
    }

    private void lightningWarning(int num)
    {
        boomComing(num, Color.green, Color.red, sr);
    }
}
