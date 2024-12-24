
using UnityEngine;

public class FireArcaneDamage : BaseSpellDamage
{

    protected void Awake()
    {
        spellDamage = 12f;
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyHealthScript>().takeDamage(spellDamage);
            }
        }
    }

    public override void Update()
    {

    }


    public void activateFire()
    {
        col.enabled = true;
    }
}
