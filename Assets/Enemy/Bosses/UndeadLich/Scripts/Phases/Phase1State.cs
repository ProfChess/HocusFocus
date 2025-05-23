using UnityEngine;

public class Phase1State : BaseState, IBossPhase
{
    [SerializeField] private BaseAttackSpawn endPhaseAttack;
    public void EnterPhase(BossController boss)     //Enters phase 
    {
        Debug.Log("Phase 1");
        attackCooldown = 5f;

        //Trigger Starting/Spawning Animation
        GetComponentInParent<BoxCollider2D>().enabled = true; //Activate after animation completes
    }
    public void UpdatePhase(BossController boss)    //For whatever needs to be updated each frame in each phase
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            getRandomAttack(boss);
        }

    }
    public void ExitPhase(BossController boss)      //Final part before leaving phase
    {
        endPhaseAttack.executeAttack(boss);
        Debug.Log("Leaving Phase 1");
    }
}
