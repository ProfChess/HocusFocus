using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseSpellCast : MonoBehaviour
{
    // Spell Casting Stats
    public float cooldownTime;
    public float castTime;
    protected bool casting;
    protected float cooldownTimer;

    public GameObject spellPrefab;
    public Transform rightSpellSpawn;
    public Transform leftSpellSpawn;
    protected Transform spawnPoint;

    public Vector2 direction;

    public virtual void Cast(bool lookingRight)
    {
        if (casting || cooldownTimer > 0f)
        {
            return;
        }
        StartCoroutine(CastSpellRoutine(lookingRight));
    }

    protected virtual IEnumerator CastSpellRoutine(bool lookingRight)
    {
        casting = true;
        yield return new WaitForSeconds(castTime);

        if (lookingRight)
        {
            spawnPoint = rightSpellSpawn;
            direction = Vector2.right;
        }
        else
        {
            spawnPoint = leftSpellSpawn;
            direction = Vector2.left;
        }
        casting = false;
        cooldownTimer = cooldownTime;
    }

    protected virtual void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }


}
