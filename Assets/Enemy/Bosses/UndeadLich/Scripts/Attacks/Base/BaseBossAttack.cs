using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class BaseBossAttack : MonoBehaviour
{
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected ObjectPool pool;
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
        poolManager.ReturnObjectToPool(pool, gameObject);
    }


    //Warning Logic
    protected void boomComing(int num, Color normColor, Color warnColor, SpriteRenderer sr)
    {
        //0 = reset warning, 1 = change to warning, 2 = damage signal
        if (num == 0)
        {
            Color color = normColor;
            color.a = 0.2f;
            sr.color = color;
        }
        else if (num == 1)
        {
            Color color = warnColor;
            color.a = 0.2f;
            sr.color = color;
        }
        else if (num == 2)
        {
            Color color = sr.color;
            color.a = 1f;
            sr.color = color;
        }
    }

    //Get and Set
    public float getAttackSpeed()
    {
        return attackSpeed;
    }


}
