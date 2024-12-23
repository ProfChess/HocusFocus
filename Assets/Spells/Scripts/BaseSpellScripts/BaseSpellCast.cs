using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseSpellCast : MonoBehaviour
{
    // Spell Casting Stats
    public float castTime;
    public bool casting;
    protected bool lookingRight = true;

    public GameObject spellPrefab;
    public Transform rightSpellSpawn;
    public Transform leftSpellSpawn;
    protected Transform spawnPoint;

    public Vector2 direction;

    public float manaCost;

    //Animation
    public Animator playerAnim;
    public PlayerMana player;

    //Sound
    [SerializeField] protected int soundType; //0 = Fire, 1 = Ice, 2 = Thunder 

    public virtual void Cast()
    {
        if (casting)
        {
            return;
        }
        else if (player.returnCurrentMana() < manaCost)
        {
            Debug.Log("Not Enough Mana");
            return;
        }
        player.decreaseMana(manaCost);
        playerAnim.SetTrigger(castAnim());
        StartCoroutine(CastSpellRoutine());
    }

    protected virtual IEnumerator CastSpellRoutine()
    {
        casting = true;
        AudioManager.Instance.playPlayerSound("Cast");
        yield return new WaitForSeconds(castTime);

        casting = false;
    }


    protected virtual void Start()
    {
        player = GetComponentInParent<PlayerMana>();
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
        spellSound(soundType);
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
        spellPlacement();
    }

    protected virtual void spellPlacement()
    {
        if (!lookingRight)
        {
            spellPrefab.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (lookingRight)
        {
            spellPrefab.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //Sound Play
    protected virtual void spellSound(int soundNum)
    {
        switch (soundNum)
        {
            case 0:
                AudioManager.Instance.playSpellSound("Fire");
                break;
            case 1:
                AudioManager.Instance.playSpellSound("Ice");
                break;
            case 2:
                AudioManager.Instance.playPlayerSound("Thunder");
                break;
            default: 
                Debug.LogWarning("Incorrect Sound Number: " +  soundNum);
                break;
        }
    }
}
