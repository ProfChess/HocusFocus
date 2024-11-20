
using UnityEngine;

public class Phase2State : BaseState, IBossPhase
{
    public void EnterPhase(BossController boss)
    {
        Debug.Log("Phase 2");
        attackCooldown = 0;
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
        Debug.Log("Leaving Phase 2");

    }
}
