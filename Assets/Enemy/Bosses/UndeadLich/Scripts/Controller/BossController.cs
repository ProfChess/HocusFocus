using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Burst.CompilerServices;
using UnityEngine;

public enum TrackBossPhase
{
    Phase1,
    Phase2,
    Phase3,
}

[RequireComponent(typeof(EnemyHealthScript))]
public class BossController : MonoBehaviour
{
    //Boss 1
    //Undead Lich

    //Phases 
    private IBossPhase curPhase;            //Current Phase
    private TrackBossPhase bossPhaseLabel;  //Enum to track boss phase
    public Phase1State phase1;
    public Phase2State phase2;
    public Phase3State phase3;

    //Misc 
    private EnemyHealthScript healthScript; //Health script of boss
    private float maxHP;

    //Animation
    [SerializeField] private Animator BossAnimator;
    private SpriteRenderer SR;

    private void Start()
    {
        healthScript = GetComponent<EnemyHealthScript>();
        healthScript.onHealthChanged += onHealthChanged;

        maxHP = healthScript.enemyMaxHealth;

        SR = GetComponentInChildren<SpriteRenderer>();
        bossPhaseLabel = TrackBossPhase.Phase1; //Starting Phase
        curPhase = phase1;
        curPhase.EnterPhase(this);
    }

    private void Update()
    {
        if (curPhase != null)
        {
            curPhase.UpdatePhase(this); //Updates the phase each frame
        }

        if (GameManager.Instance.player.transform.position.x > gameObject.transform.position.x)
        {
            SR.flipX = false;
        }
        else if (GameManager.Instance.player.transform.position.x < gameObject.transform.position.x)
        {
            SR.flipX = true;
        }
    }

    void OnDestroy()
    {
        healthScript.onHealthChanged -= onHealthChanged;
    }

    //Changes to next phase
    public void ChangePhase(IBossPhase newPhase)
    {
        if (curPhase != null)
        {
            curPhase.ExitPhase(this);
            curPhase = newPhase;
            curPhase.EnterPhase(this);
        }
    }

    //Call CheckPhase function each time health changes
    private void onHealthChanged(float health)
    {
        if (healthScript != null)
        {
            if (healthScript.getEnemyCurrentHealth() <= 0)
            {
                //Death Anim
                BossAnimator.SetTrigger("DeathTrigger");
            }
            else
            {
                //Damage Anim
                BossAnimator.SetTrigger("DamageTrigger");
            }
        }
        CheckPhase(health);
    }

    //If health < threshold -> go to next phase
    private void CheckPhase(float health)
    {
        if (health < 0.75 * maxHP && health > 0.35 * maxHP && bossPhaseLabel == TrackBossPhase.Phase1)
        {
            bossPhaseLabel = TrackBossPhase.Phase2;
            ChangePhase(phase2);
        }
        else if (health <= 0.35 * maxHP && bossPhaseLabel == TrackBossPhase.Phase2)
        {
            bossPhaseLabel = TrackBossPhase.Phase3;
            ChangePhase(phase3);
        }
    }

}
