using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CasterEnemy : BaseEnemyMovement
{
    //Caster Enemy
    //Will charge a spell when player is in line of sight (los)
    //Will not move
    //Spell can be blocked by platforms/by breaking los

    //State variables
    private float enemyAttackCooldown = 1.5f;
    private bool isCasting = false;
    private bool lookRight = true;
    private float yRange = 0.7f;
    private bool isWithinYRange = false;
    private bool isWithinXRange = false;
    private Vector3 playerPos;
    private Vector3 enemyPos;

    //References
    public GameObject spellPrefab;

    //Amimation
    public Animator enemyAnim;
    private SpriteRenderer enemyVisual;

    //Assign
    protected void Awake()
    {
        Initialize(0, 0, 0, false, 8f, 50);
    }

    protected override void Start()
    {
        base.Start();
        enemyVisual = GetComponentInChildren<SpriteRenderer>();
    }

    protected void Update()
    {
        //Range Calc
        playerPos = player.transform.position;
        enemyPos = transform.position;
        isWithinYRange = playerPos.y <= enemyPos.y + yRange && playerPos.y >= enemyPos.y - yRange;
        isWithinXRange = playerPos.x >= enemyPos.x - 12f && playerPos.x <= enemyPos.x + 12f;
        if (canSeePlayer && isWithinYRange && isWithinXRange && !enemyBlind)
        {
            casterAttackState();
        }
        else
        {
            casterIdleState();
        }

        //Face left or right
        lookRight = transform.position.x < player.transform.position.x;

        if (canSeePlayer)
        {
            enemyVisual.flipX = lookRight;
        }

    }

    protected void casterIdleState()
    {

    }

    protected void casterAttackState()
    {
        if (!isCasting)
        {
            StartCoroutine(destructionSpell());
        }
    }

    private IEnumerator destructionSpell()
    {   
        isCasting = true;
        //Animation Trigger
        enemyAnim.SetTrigger("Attack");

        yield return new WaitForSeconds(enemyAttackCooldown); //attack cooldown
        isCasting = false;
    }

    public Vector2 getCastDirection()
    {
        if (lookRight)
        {
            return Vector2.right;
        }
        else
        {
            return Vector2.left;
        }
    }

    public void cultistSpellLogic()
    {
        AudioManager.Instance.playEnemySound("Cast");
        Vector3 castLocation = transform.position;
        spellPrefab.GetComponent<CasterEnemySpell>().setSpellDirection(getCastDirection());
        //Blasts box left/right that gradually gets bigger
        if (lookRight)
        {
            castLocation.x++;
            Instantiate(spellPrefab, castLocation, Quaternion.identity);
        }

        else if (!lookRight)
        {
            castLocation.x--;
            Instantiate(spellPrefab, castLocation, Quaternion.identity);
        }
    }
}
