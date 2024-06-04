using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor.UI;
using UnityEngine;

public class ArcaneSpellDamage : BaseSpellDamage
{
    public LayerMask enemyLayer;
    private float spellCooldown;
    protected void Awake()
    {
        spellDamage = 2f;
        spellSpeed = 0f;
        spellLength = 2f;
        effectArea = new Vector2(spellLength, 2);
        col = GetComponent<BoxCollider2D>();
    }
    public void arcaneAttack()
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

        beginCooldown();

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {

    }

    public override void Update() 
    {
        if (spellCooldown >= 0)
        {
            spellCooldown -= Time.deltaTime;
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


}
