using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public abstract class BaseAttackSpawn : MonoBehaviour, IBossAttack
{
    //Variables
    [SerializeField] protected float weight;            //Likely to be chosen -> Bigger number = More likely
    [SerializeField] protected float duration;          //Duration of ability including gap between next cast

    //References
    [SerializeField] protected GameObject prefab;       //Prefab to use
    [SerializeField] protected ObjectPool objPool;      //Pool to get/return objects from
    protected PoolManager poolManager;                  //Pool manager
    protected Transform playerLocation;                 //Player transform

    //Animation
    [SerializeField] protected AnimationClip spellCastAnim;
    private Animator AnimControl;
    [SerializeField] private int AttackFrameTarget;
    private float animationFrameRate;
    private bool attackTriggered = false;
    private void Start()
    {
        poolManager = FindObjectOfType<PoolManager>();
        AnimControl = FindObjectOfType<BossController>().GetComponentInChildren<Animator>();
        //Anim
        if (spellCastAnim != null )
        {
            animationFrameRate = spellCastAnim.frameRate;
        }
    }

    public float getCooldown()
    {
        return duration;
    }

    public float getWeight()
    {
        return weight;
    }
   

    protected void findPlayer()
    {
        playerLocation = GameManager.Instance.player.transform;
    }
    
    protected GameObject getPlayer()
    {
        return GameManager.Instance.player;
    }

    protected Vector2 getPlayerDirection()
    {
        Vector2 dir = playerLocation.position - transform.position;
        return dir;
    }

    //Pool get
    public ObjectPool GetObjectPool()
    {
        return objPool;
    }

    protected GameObject GetPoolManager()
    {
        return poolManager.getObjectFromPool(GetObjectPool());
    }
    public virtual void executeAttack(BossController boss)
    {
        if (boss != null)
        {
            CastAnimation(boss);
        }
    }


    //Animation
    protected void CastAnimation(BossController boss)
    {
        if (AnimControl == null)
        {
            AnimControl = boss.GetComponentInChildren<Animator>();
        }
        if (spellCastAnim != null)
        {
            AnimControl.Play(spellCastAnim.name);
        }
    }

    private void Update()
    {
        if (AnimControl.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            var clipInfo = AnimControl.GetCurrentAnimatorClipInfo(0)[0];
            if (clipInfo.clip.name == spellCastAnim.name)
            {
                float currentFrame = AnimControl.GetCurrentAnimatorStateInfo(0).normalizedTime * clipInfo.clip.length * animationFrameRate;

                if (currentFrame >= AttackFrameTarget && !attackTriggered)
                {
                    attackTriggered = true;
                    SpawnBossAttack();
                }
            }
            else
            {
                attackTriggered = false;
            }
        }
    }

    public virtual void SpawnBossAttack() //Override in each script to create attack
    {

    }
}
