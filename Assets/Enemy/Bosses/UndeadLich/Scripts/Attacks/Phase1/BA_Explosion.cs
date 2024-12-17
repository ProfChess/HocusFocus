using System.Collections;
using System.Linq;
using Unity.Properties;
using UnityEditor.Animations;
using UnityEngine;

public class BA_Explosion : BaseBossAttack
{
    private GameObject player;        //Ref to player       -> get from Game manager
    private float timer;              //Damage Timer        -> Tracks when attack stops changing location/damages player
    private bool endAttack;           //Signal to return
    private bool startAttack;
    //References
    private CircleCollider2D col;
    private SpriteRenderer sr;
    [SerializeField] private GameObject AttackVisual;

    public void Initialize(float time, PoolManager pm, GameObject pl, ObjectPool objectPool) //initialize/reset all variables and start coroutine
    {
        timer = time;
        poolManager = pm;
        pool = objectPool;
        player = pl;
        endAttack = false;
        explosionWarning(0);
        col.enabled = false;
        startAttack = false;
        StartCoroutine(explodeDelay());
    }

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerHealth>().takeDamage(attackDamage);
                col.enabled = false;
            }
        }
    }
    private void Update()
    {
        if (endAttack)
        {
            returnGameObject();
        }
        if (startAttack)
        {
            AttackVisual.GetComponent<SpriteRenderer>().color = Color.white;
            AttackVisual.GetComponent<Animator>().SetTrigger("Boom");
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
        startAttack = true;
    }

    public void turnCollider()
    {
        col.enabled = true;
    }
    public void endAttacking()
    {
        endAttack = true;
    }

    private void explosionWarning(int num)
    {
        boomComing(num, Color.white, Color.red, sr);
    }
    
    
}
