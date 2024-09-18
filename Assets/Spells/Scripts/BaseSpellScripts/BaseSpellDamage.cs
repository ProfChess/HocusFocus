using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpellDamage : MonoBehaviour
{
    //Damage Stats/types
    [HideInInspector]
    public float spellSpeed;       //How fast projectile travels 
    [HideInInspector]
    public float spellDamage;      //Damage of spell
    [HideInInspector]
    public BoxCollider2D col;      //Collider used for area collision
    [HideInInspector]
    public float spellLength;      //Range of spell
    [HideInInspector]
    public Vector2 spellDirection; //Direction of spell 
    [HideInInspector]
    public bool spellHit;          //Check for spell hitting enemy
    [HideInInspector]
    public float xSpawn;           //Spell spawn x coordindate

    //Animation
    public Animator spellAnim;
    

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.tag == "Enemy")
        {
            onSpellHit();
            spellDeathAnimation();
            collision.gameObject.GetComponent<EnemyHealthScript>().takeDamage(spellDamage);
        }
    }

    public virtual void Update()
    {
        if (spellHit)
        {
            spellDeathAnimation();
            Invoke("killObject", 2f);
        }
        else if (gameObject.transform.position.x >= (xSpawn + spellLength) || gameObject.transform.position.x
            <= (xSpawn - spellLength))
        {
            turnOff();
            Invoke("killObject", 2f);
        }
        else
        {
            transform.Translate(spellDirection * spellSpeed * Time.deltaTime);
        }

    }

    public virtual void turnOff()
    {
        gameObject.SetActive(false);
        Invoke("killObject", 1);
    }

    protected virtual void killObject()
    {
        Destroy(gameObject);
    }

    protected virtual void onSpellHit()
    {
        spellHit = true;
    }

    public virtual void setDirection(Vector2 direction)
    {
        spellDirection = direction;
    }

    public virtual void spellDeathAnimation()
    {
        if (spellAnim != null)
        {
            spellAnim.SetTrigger("Death");
        }
    }
}
