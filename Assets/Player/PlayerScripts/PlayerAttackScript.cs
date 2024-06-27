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

    //Combo variables
    private bool comboStarted = false;
    private string firstComboSpell;
    private string secondComboSpell;
    private float comboCount = 0f;


    private void Awake()
    {
        spellControls = new PlayerInput();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (comboCount == 2)
        {
            //Perform Combo
            if (firstComboSpell == "Fire" && secondComboSpell == "Ice" ||
                firstComboSpell == "Ice" && secondComboSpell == "Fire")
            {
                Debug.Log("Fire and Ice");
            }
            if (firstComboSpell == "Fire" && secondComboSpell == "Arcane" ||
                firstComboSpell == "Arcane" && secondComboSpell == "Fire")
            {
                Debug.Log("Fire and Arcane");
            }
            if (firstComboSpell == "Ice" && secondComboSpell == "Arcane" || 
                firstComboSpell == "Arcane" && secondComboSpell == "Ice")
            {
                Debug.Log("Arcane and Ice");
            }
            comboCount = 0;
            comboStarted = false;
        }

        if (playerController.onGround == false || playerController.moveDirection != Vector2.zero)
        {
            comboCount = 0;
            comboStarted = false;
        }
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

        //Combo Start
        spellControls.Player.ToggleCombineCast.performed += OnComboStarted;
    }


    private void OnDisable()
    {
        //Fire Spell
        spellControls.Player.FireSpell.performed -= OnFireSpellCast;

        //Ice Spell
        spellControls.Player.IceSpell.performed -= OnIceSpellCast;

        //Arcane Spell
        spellControls.Player.ArcaneSpell.performed -= OnArcaneSpellCast;

        //Combo Canceled
        spellControls.Player.ToggleCombineCast.performed -= OnComboStarted;
        spellControls.Disable();
    }

    //SPELLS

    //FireSpell
    private void OnFireSpellCast(InputAction.CallbackContext obj)
    {   
        if (comboStarted)
        {
            if(comboCount == 0)
            {
                firstComboSpell = "Fire";
                comboCount++;
            }
            else if (comboCount == 1 && firstComboSpell != "Fire")
            {
                secondComboSpell = "Fire";
                comboCount++;
            }
        }
        else
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                playerController.playerStartCast();
                Debug.Log("Fire Spell");
                FireBallCast.Cast(playerController.lookingRight);
                playerController.playerStopCast();
            }
        }
    }

    //IceSpell
    private void OnIceSpellCast(InputAction.CallbackContext obj)
    {
        if (comboStarted)
        {
            if (comboCount == 0)
            {
                firstComboSpell = "Ice";
                comboCount++;
            }
            else if (comboCount == 1 && firstComboSpell != "Ice")
            {
                secondComboSpell = "Ice";
                comboCount++;
            }
        }
        else
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                playerController.playerStartCast();
                Debug.Log("Ice Spell");
                IceSpellCast.Cast(playerController.lookingRight);
                playerController.playerStopCast();
            }
        }

    }

    //ArcaneSpell
    private void OnArcaneSpellCast(InputAction.CallbackContext obj)
    {
        if (comboStarted)
        {
            if (comboCount == 0)
            {
                firstComboSpell = "Arcane";
                comboCount++;
            }
            else if (comboCount == 1 && firstComboSpell != "Arcane")
            {
                secondComboSpell = "Arcane";
                comboCount++;
            }
        }
        else
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                playerController.playerStartCast();
                Debug.Log("Arcane Spell");
                ArcaneSpellCast.Cast(playerController.lookingRight);
                playerController.playerStopCast();
            }
        }

    }
    private void OnComboStarted(InputAction.CallbackContext obj)
    {
        if (comboStarted)
        {
            comboStarted = false;
            Debug.Log("Combo Canceled");
        }
        else if (!comboStarted)
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                comboStarted = true;
                Debug.Log("Combo Started");
            }
        }

    }



}
