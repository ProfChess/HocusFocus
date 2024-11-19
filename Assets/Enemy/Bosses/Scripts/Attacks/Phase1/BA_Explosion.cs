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
    public void Initialize(float time, PoolManager pm, GameObject pl) //initialize/reset all variables and start coroutine
    {
        timer = time;
        poolManager = pm;
        player = pl;
        endAttack = false;
        boomComing(0);
        col.enabled = false;
        StartCoroutine(explodeDelay());
        Debug.Log("Explosion Spawned");
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
            returnExplosion();
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
        boomComing(1);
        yield return new WaitForSeconds(0.4f);

        //Damage Activate
        boomComing(2);
        col.enabled = true;

        yield return new WaitForSeconds(1);
        endAttack = true;
    }

    private void returnExplosion()
    {
        poolManager.ReturnObjectToPool(1, gameObject);
    }

    protected override void returnGameObject()
    {
        returnExplosion();
    }

    //Explosion and Signal Logic
    private void boomComing(int num)
    {
        
        if (num == 0)
        {
            Color color = Color.white;
            color.a = 0.2f;
            sr.color = color;
        }
        else if (num == 1)
        {
            Color color = Color.red;
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

    
    
}
