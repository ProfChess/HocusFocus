using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phase2State : MonoBehaviour, IBossPhase
{
    public void EnterPhase(BossController boss)
    {
        Debug.Log("Phase 2");

    }
    public void UpdatePhase(BossController boss)
    {

    }
    public void ExitPhase(BossController boss)
    {
        Debug.Log("Leaving Phase 2");

    }
}
