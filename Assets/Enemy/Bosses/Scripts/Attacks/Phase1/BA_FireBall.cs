using UnityEngine;

public class BA_FireBall : BaseBossAttack
{
    private Vector2 moveDirection;
    public void Initialize(Vector2 direction, PoolManager pm)
    {
        moveDirection = direction;
        poolManager = pm;
    }

    private void Update()
    {
        if   (transform.position.x > 10.5f 
           || transform.position.x < -10.5f
           || transform.position.y > 4.25f 
           || transform.position.y < -6.25f)
        {
            returnFireBall();
        }
        else
        {
            transform.Translate(moveDirection.normalized * attackSpeed * Time.deltaTime);
        }
    }

    //Returns object to pool to be reused later
    private void returnFireBall()
    {
        poolManager.ReturnObjectToPool(0, gameObject);
    }

    protected override void returnGameObject()
    {
        returnFireBall();
    }


}
