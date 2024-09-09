using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ArcaneIceDamage : BaseSpellDamage
{
    public LayerMask enemyLayer;
    private float spellCooldown;
    private float dotDamage = 2f;
    private float dotDuration = 4f;
    private float dotInterval = 1f;
    protected void Awake()
    {
        spellLength = 2f;
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
    }

    private void AOEFreeze()
    {
        Vector2 boxPointA = col.bounds.min;
        Vector2 boxPointB = col.bounds.max;
        Collider2D[] enemyList = Physics2D.OverlapAreaAll(boxPointA, boxPointB, enemyLayer);

        foreach (Collider2D enemy in enemyList)
        {
            BaseEnemyMovement enemyControl = enemy.GetComponent<BaseEnemyMovement>();
            EnemyHealthScript enemyHealth = enemy.GetComponent<EnemyHealthScript>();
            if (enemyControl != null)
            {
                enemyControl.slowDown(0f, spellLength);
                enemyHealth.applyDot(dotDamage, dotDuration, dotInterval);
            }
        }
        beginCooldown();
    }

    public override void Update()
    {
        if (spellCooldown >= 0)
        {
            spellCooldown -= Time.deltaTime;
        }
    }

    private void beginCooldown()
    {
        spellCooldown = spellLength;
    }

    public void activateCollider()
    {
        col.enabled = true;
        AOEFreeze();
    }
}
