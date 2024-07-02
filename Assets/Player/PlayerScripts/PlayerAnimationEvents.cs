using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public void endGame()
    {
        GameManager.Instance.playerDeath();
    }

    public void teleportEvent()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().teleportPlayer();
    }
}
