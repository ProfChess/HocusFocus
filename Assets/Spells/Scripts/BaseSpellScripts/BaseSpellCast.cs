using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseSpellCast : MonoBehaviour
{
    // Spell Casting Stats
    public float cooldownTime;
    public float castTime;
    public bool casting;
    protected float cooldownTimer;
    protected bool lookingRight;

    public GameObject spellPrefab;
    public Transform rightSpellSpawn;
    public Transform leftSpellSpawn;
    protected Transform spawnPoint;

    public Vector2 direction;

    public float manaCost;

    //Animation
    private Animator playerAnim;

    public virtual void Cast()
    {
        if (casting)
        {
            return;
        }
        if (cooldownTimer > 0)
        {
            Debug.Log("Spell on Cooldown");
            return;
        }
        else if (GameManager.Instance.player.GetComponent<PlayerMana>().returnCurrentMana() < manaCost)
        {
            Debug.Log("Not Enough Mana");
            return;
        }
        GameManager.Instance.player.GetComponent<PlayerMana>().decreaseMana(manaCost);
        playerAnim.SetTrigger(castAnim());
        StartCoroutine(CastSpellRoutine());
    }

    protected virtual IEnumerator CastSpellRoutine()
    {
        casting = true;
        yield return new WaitForSeconds(castTime);

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

    protected virtual void Start()
    {
        playerAnim = GameManager.Instance.player.GetComponentInChildren<Animator>();
    }

    //Animation
    public string castAnim()
    {
        //Animation Control
        if (castTime >= 0.2)
        {
            if (castTime >= 0.4)
            {
                if (castTime >= 0.8)
                {
                    return "PlayerLongCast";
                }
                else
                {
                    return "PlayerMediumCast";
                }
            }
            else
            {
                return "PlayerFastCast";
            }
        }
        else
        {
            return "SpellNoNo"; 
        }
    }

    public virtual void spawnSpell()
    {
        lookingRight = GameManager.Instance.player.GetComponent<PlayerController>().getLookingRight();
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
    }
}
