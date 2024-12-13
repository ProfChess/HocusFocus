using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase3State : BaseState, IBossPhase
{
    public void EnterPhase(BossController boss)
    {
        Debug.Log("Phase 3");
        attackCooldown = 4;

    }
    public void UpdatePhase(BossController boss)
    {
        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            getRandomAttack(boss);
        }
    }
    public void ExitPhase(BossController boss)
    {
        Debug.Log("Leaving Phase 3");
    }
}
