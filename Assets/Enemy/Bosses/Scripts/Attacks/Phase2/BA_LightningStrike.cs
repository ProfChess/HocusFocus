using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BA_LightningStrike : BaseBossAttack
{
    //Variables 
    protected Vector2 location;       //Place to strike
    protected float timer;            //How long to last after strike
    protected bool stopAttack = false;//Controls when to return to pool
    protected bool stopOnDamage;
    //References
    protected BoxCollider2D hitbox;
    protected SpriteRenderer sr;

    protected virtual void Awake() //assign references
    {
        hitbox = GetComponent<BoxCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {

    }

    protected void OnTriggerStay2D(Collider2D col)
    {
        if (col != null)
        {
            if (col.CompareTag("Player"))
            {
                col.GetComponent<PlayerHealth>().takeDamage(attackDamage);
                returnGameObject();
            }
        }
    }



    private void Update()
    {
        if (stopAttack)
        {
            returnGameObject(); //returns to pool

        }
        else
        {

        }
    }
    public virtual void Initialize(PoolManager pm, Vector2 place, float duration, bool stopondamage, ObjectPool objectPool)
    {
        //Reset Variables
        hitbox.enabled = false;
        stopAttack = false;

        //Assign
        poolManager = pm;
        pool = objectPool;
        location = place;
        timer = duration;
        stopOnDamage = stopondamage;


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
        if (stopOnDamage)
        {
            hitbox.enabled = false;
            base.returnGameObject();
        }
    }


    private void lightningWarning(int num)
    {
        boomComing(num, Color.green, Color.red, sr);
    }
}
