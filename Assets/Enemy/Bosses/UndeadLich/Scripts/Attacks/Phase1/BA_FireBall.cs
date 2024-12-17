using System.Reflection;
using UnityEngine;

public class BA_FireBall : BaseBossAttack
{
    private Vector2 moveDirection;
    [SerializeField] protected GameObject attackVisual;

    public void Initialize(Vector2 direction, PoolManager pm, ObjectPool objpool)
    {
        moveDirection = direction;
        poolManager = pm;
        pool = objpool;
        float angle = Mathf.Atan2(moveDirection.normalized.y, moveDirection.normalized.x) * Mathf.Rad2Deg;
        angle = angle + 180;
        attackVisual.transform.rotation = Quaternion.Euler(0, 0, angle);
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
