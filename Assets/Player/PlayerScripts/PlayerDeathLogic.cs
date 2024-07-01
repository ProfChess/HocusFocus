using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathLogic : MonoBehaviour
{
    public void endGame()
    {
        GameManager.Instance.playerDeath();
    }
}
