using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseBossAttack : MonoBehaviour
{
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackFrequency;
    [SerializeField] protected float attackSpeed;
    protected PoolManager poolManager;

    //Damage Function
    protected virtual void OnTriggerEnter2D(Collider2D col)
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

    //Empty function for returning object to pool -> override this function to place object into correct pool
    protected virtual void returnGameObject()
    {

    }


}
