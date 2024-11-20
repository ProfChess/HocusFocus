using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSpellDamage : BaseSpellDamage
{
    protected void Awake()
    {
        xSpawn = gameObject.transform.position.x;
        spellDamage = 2f;
        spellSpeed = 20f;
        spellLength = 10f;
        col = GetComponent<BoxCollider2D>();
        spellHit = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.CompareTag("Enemy"))
        {
            BaseEnemyMovement Enemy = collision.GetComponent<BaseEnemyMovement>();
            if (Enemy != null)
            {
                collision.GetComponent<BaseEnemyMovement>().slowDown(1f, 2f);
            }
        }
    }


}
