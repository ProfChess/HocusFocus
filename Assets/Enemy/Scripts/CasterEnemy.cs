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
    private float enemyAttackCooldown = 5f;
    private bool isCasting = false;
    private bool lookRight = true;
    private float yRange = 1f;
    private bool isWithinYRange = false;

    //References
    public GameObject spellPrefab;

    //Assign
    protected void Awake()
    {
        Initialize(0, 0, 0, false, 50, 50);
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void Update()
    {
        if (canSeePlayer && isWithinYRange)
        {
            casterAttackState();
        }
        else
        {
            casterIdleState();
        }

        //Face left or right
        lookRight = transform.position.x < player.transform.position.x;
        isWithinYRange = player.transform.position.y <= transform.position.y + yRange &&
            player.transform.position.y >= transform.position.y - yRange;

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
        yield return new WaitForSeconds(0.5f); //Enemy charge attack time

        Vector3 castLocation = transform.position;
        spellPrefab.GetComponent<CasterEnemySpell>().setSpellDirection(getCastDirection());
        //Blasts box left/right that gradually gets bigger
        if (lookRight)
        {
            castLocation.x++;
            Instantiate(spellPrefab, castLocation, Quaternion.identity);
        }

        else if(!lookRight)
        {
            castLocation.x --;
            Instantiate(spellPrefab, castLocation, Quaternion.identity);
        }

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
}
