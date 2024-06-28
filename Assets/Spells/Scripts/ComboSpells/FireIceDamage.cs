using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireIceDamage : BaseSpellDamage
{
    public LayerMask enemyLayer;
    private float spellCooldown;
    protected void Awake()
    {
        spellLength = 3f;
        col = GetComponent<BoxCollider2D>();
    }

    public void AOEBlind()
    {
        Vector2 boxPointA = col.bounds.min;
        Vector2 boxPointB = col.bounds.max;
        Collider2D[] enemyList = Physics2D.OverlapAreaAll(boxPointA, boxPointB, enemyLayer);

        int enemyNum = enemyList.Length;

        foreach (Collider2D enemy in enemyList)
        {
            BaseEnemyMovement enemyControl = enemy.GetComponent<BaseEnemyMovement>();
            if (enemyControl != null)
            {
                enemyControl.blindness(spellLength);
            }
        }
        beginCooldown();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<BaseEnemyMovement>().blindness(spellLength);
            }
        }

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
