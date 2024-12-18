using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimEvents : MonoBehaviour
{
    public void endingCall()
    {
        GameManager.Instance.FinishGame();
    }

}
