using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackScript : MonoBehaviour
{
    private PlayerInput spellControls;
    private PlayerController playerController;

    //Spell References
    public BaseSpellCast FireBallCast;
    public BaseSpellCast IceSpellCast;
    public BaseSpellCast ArcaneSpellCast;

    //References

    private void Awake()
    {
        spellControls = new PlayerInput();
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {   
        spellControls.Enable();
        //Fire Spell
        spellControls.Player.FireSpell.performed += OnFireSpellCast;

        //Ice Spell
        spellControls.Player.IceSpell.performed += OnIceSpellCast;

        //Arcane Spell
        spellControls.Player.ArcaneSpell.performed += OnArcaneSpellCast;
    }

    private void OnDisable()
    {
        //Fire Spell
        spellControls.Player.FireSpell.performed -= OnFireSpellCast;

        //Ice Spell
        spellControls.Player.IceSpell.performed -= OnIceSpellCast;

        //Arcane Spell
        spellControls.Player.ArcaneSpell.performed -= OnArcaneSpellCast;
        spellControls.Disable();
    }

    //SPELLS

    //FireSpell
    private void OnFireSpellCast(InputAction.CallbackContext obj)
    {   
        if (playerController.onGround)
        {
            playerController.playerStartCast();
            Debug.Log("Fire Spell");
            FireBallCast.Cast(playerController.lookingRight);
            playerController.playerStopCast();
        }
    }

    //IceSpell
    private void OnIceSpellCast(InputAction.CallbackContext obj)
    {   
        if (playerController.onGround)
        {
            playerController.playerStartCast();
            Debug.Log("Ice Spell");
            IceSpellCast.Cast(playerController.lookingRight);
            playerController.playerStopCast();
        }
    }

    //ArcaneSpell
    private void OnArcaneSpellCast(InputAction.CallbackContext obj)
    {
        if (playerController.onGround)
        {
            playerController.playerStartCast();
            Debug.Log("Arcane Spell"); 
            ArcaneSpellCast.Cast(playerController.lookingRight);
            playerController.playerStopCast();
        }
    }


}
