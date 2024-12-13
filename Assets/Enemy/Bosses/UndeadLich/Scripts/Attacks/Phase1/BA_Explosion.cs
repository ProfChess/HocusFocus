using System.Collections;
using System.Linq;
using Unity.Properties;
using UnityEngine;

public class BA_Explosion : BaseBossAttack
{
    private GameObject player;        //Ref to player       -> get from Game manager
    private float timer;              //Damage Timer        -> Tracks when attack stops changing location/damages player
    private bool endAttack;           //Signal to return

    //References
    private CircleCollider2D col;
    private SpriteRenderer sr;


    public void Initialize(float time, PoolManager pm, GameObject pl, ObjectPool objectPool) //initialize/reset all variables and start coroutine
    {
        timer = time;
        poolManager = pm;
        pool = objectPool;
        player = pl;
        endAttack = false;
        explosionWarning(0);
        col.enabled = false;
        StartCoroutine(explodeDelay());
    }

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Update()
    {
        if (endAttack)
        {
            returnGameObject();
        }
    }
    private IEnumerator explodeDelay()
    {
        float timePassed = 0;
        float followTime = timer - 1;

        while (timePassed < followTime)
        {
            if (player != null)
            {
                gameObject.transform.position = player.transform.position;
            }

            timePassed += Time.deltaTime;
            yield return null;
        }
        //Signal Explosion Logic
        explosionWarning(1);
        yield return new WaitForSeconds(0.4f);

        //Damage Activate
        explosionWarning(2);
        col.enabled = true;

        yield return new WaitForSeconds(1);
        endAttack = true;
    }



    private void explosionWarning(int num)
    {
        boomComing(num, Color.white, Color.red, sr);
    }
    
    
}
