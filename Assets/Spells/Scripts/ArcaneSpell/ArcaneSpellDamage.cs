using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.UI;
using UnityEngine;

public class ArcaneSpellDamage : BaseSpellDamage
{
    public LayerMask enemyLayer;
    private float spellCooldown;
    private float damageTick = 0.8f;
    private float damageClock = 0f;

    //Animation Managment
    public Animator Bolt1;
    public Animator Bolt2;
    public Animator Bolt3;
    private bool bolt1played = false;
    private bool bolt2played = false;
    protected void Awake()
    {
        spellDamage = 3f;
        spellSpeed = 0f;
        spellLength = 3f;
        col = GetComponent<BoxCollider2D>();
    }
    public void arcaneAttack()
    {
        arcaneAOE();
        beginCooldown();
    }

    private void arcaneAOE()
    {
        //Bounds of AOE
        Vector2 spellTopLeftCorner = col.bounds.min;
        Vector2 spellBottomRightCorner = col.bounds.max;

        Collider2D[] enemyList = Physics2D.OverlapAreaAll(spellTopLeftCorner, spellBottomRightCorner, enemyLayer);

        int enemyNum = enemyList.Length;

        float totalDamage = spellDamage * enemyNum;

        foreach (Collider2D enemy in enemyList)
        {
            EnemyHealthScript enemyHealth = enemy.GetComponent<EnemyHealthScript>();
            if (enemyHealth != null)
            {
                enemyHealth.takeDamage(totalDamage);
            }
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public override void Update() 
    {
        if (spellCooldown >= 0)
        {
            spellCooldown -= Time.deltaTime;
            damageClock += Time.deltaTime;
            if (damageClock > damageTick) 
            {
                damageClock = 0;
                arcaneAOE();
            }
        }
        else
        {
            turnOff();
            Invoke("killObject", 1);
        }
    }

    private void beginCooldown()
    {
        spellCooldown = spellLength;
    }

    //Animation Manager
    public void playNextBolt()
    {
        if (!bolt1played)
        {
            bolt1played = true;
            Bolt1.SetTrigger("BoltStart");
        }
        else if (bolt1played && !bolt2played)
        {
            bolt2played = true;
            Bolt2.SetTrigger("BoltStart");
        }
        else
        {
            Bolt3.SetTrigger("BoltStart");
        }
    }

    public void playFirstBolt()
    {
        if(!bolt1played)
        {
            Bolt1.SetTrigger("BoltStart");
            bolt1played = true;
        }
    }
}
