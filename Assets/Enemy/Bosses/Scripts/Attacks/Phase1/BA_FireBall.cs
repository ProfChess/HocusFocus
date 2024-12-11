using UnityEngine;

public class BA_FireBall : BaseBossAttack
{
    private Vector2 moveDirection;
    public void Initialize(Vector2 direction, PoolManager pm, ObjectPool objpool)
    {
        moveDirection = direction;
        poolManager = pm;
        pool = objpool;
    }

    private void Update()
    {
        if   (transform.position.x > 10.5f 
           || transform.position.x < -10.5f
           || transform.position.y > 4.25f 
           || transform.position.y < -6.25f)
        {
            returnGameObject();
        }
        else
        {
            transform.Translate(moveDirection.normalized * attackSpeed * Time.deltaTime);
        }
    }



}
