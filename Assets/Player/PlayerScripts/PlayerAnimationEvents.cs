using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    public PlayerAttackScript playerAttacks;

    public void endGame()
    {
        GameManager.Instance.playerDeath();
    }

    public void teleportEvent()
    {
        GameManager.Instance.player.GetComponent<PlayerController>().teleportPlayer();
    }

    public void spellCreator()
    {
        spellSpawner(playerAttacks.getSpellBeingCast());
    }
    private void spellSpawner(string spellName)
    {
        if (spellName == "Fire")
        {
            playerAttacks.FireBallCast.spawnSpell();
        }
        else if (spellName == "Ice")
        {
            playerAttacks.IceSpellCast.spawnSpell();
        }
        else if (spellName == "Arcane")
        {
            playerAttacks.ArcaneSpellCast.spawnSpell();
        }
        else if (spellName == "FireIce")
        {
            playerAttacks.FireIceCast.spawnSpell();
        }
        else if (spellName == "FireArcane")
        {
            playerAttacks.FireArcaneCast.spawnSpell();
        }
        else if (spellName == "ArcaneIce")
        {
            playerAttacks.ArcaneIceCast.spawnSpell();
        }
    }
 


}
