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
    public BaseSpellCast FireIceCast;
    public BaseSpellCast FireArcaneCast;
    public BaseSpellCast ArcaneIceCast;
    private string spellBeingCast;

    //Combo variables
    private bool comboStarted = false;
    private string firstComboSpell;
    private string secondComboSpell;
    private float comboCount = 0f;

    private bool isCastingSpell = false;

    //Sound
    private bool comboFailedSound = true;
    private void Awake()
    {
        spellControls = new PlayerInput();
        playerController = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (FireBallCast.casting || IceSpellCast.casting || ArcaneSpellCast.casting ||
            FireIceCast.casting || FireArcaneCast.casting || ArcaneIceCast.casting)
        {
            isCastingSpell = true;
        }
        else
        {
            isCastingSpell = false;
        }

        if (comboCount == 2)
        {
            //Perform Combo
            if (firstComboSpell == "Fire" && secondComboSpell == "Ice" ||
                firstComboSpell == "Ice" && secondComboSpell == "Fire")
            {
                spellBeingCast = "FireIce";
                FireIceCast.Cast();
                Debug.Log("Fire and Ice");
            }
            if (firstComboSpell == "Fire" && secondComboSpell == "Arcane" ||
                firstComboSpell == "Arcane" && secondComboSpell == "Fire")
            {
                spellBeingCast = "FireArcane";
                FireArcaneCast.Cast();
                Debug.Log("Fire and Arcane");
            }
            if (firstComboSpell == "Ice" && secondComboSpell == "Arcane" || 
                firstComboSpell == "Arcane" && secondComboSpell == "Ice")
            {
                spellBeingCast = "ArcaneIce";
                ArcaneIceCast.Cast();
                Debug.Log("Arcane and Ice");
            }
            comboCount = 0;
            comboStarted = false;
        }

        if (playerController.onGround == false || playerController.moveDirection != Vector2.zero)
        {
            comboCount = 0;
            comboStarted = false;

            if (comboFailedSound == false)
            {
                AudioManager.Instance.playSpellSound("ComboStop");
                comboFailedSound = true;
            }
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
        if (isCastingSpell)
        {
            return;
        }
        if (comboStarted)
        {
            if(comboCount == 0)
            {
                firstComboSpell = "Fire";
                AudioManager.Instance.playSpellSound("SpellSelect");
                comboCount++;
            }
            else if (comboCount == 1 && firstComboSpell != "Fire")
            {
                secondComboSpell = "Fire";
                AudioManager.Instance.playSpellSound("SpellSelect");
                comboFailedSound = true;
                comboCount++;
            }
        }
        else
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                Debug.Log("Fire Spell");
                spellBeingCast = "Fire";
                FireBallCast.Cast();
            }
        }
    }

    //IceSpell
    private void OnIceSpellCast(InputAction.CallbackContext obj)
    {
        if (isCastingSpell)
        {
            return;
        }
        if (comboStarted)
        {
            if (comboCount == 0)
            {
                firstComboSpell = "Ice";
                AudioManager.Instance.playSpellSound("SpellSelect");
                comboCount++;
            }
            else if (comboCount == 1 && firstComboSpell != "Ice")
            {
                secondComboSpell = "Ice";
                AudioManager.Instance.playSpellSound("SpellSelect");
                comboFailedSound = true;
                comboCount++;
            }
        }
        else
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                Debug.Log("Ice Spell");
                spellBeingCast = "Ice";
                IceSpellCast.Cast();
            }
        }

    }

    //ArcaneSpell
    private void OnArcaneSpellCast(InputAction.CallbackContext obj)
    {
        if (isCastingSpell)
        {
            return;
        }
        if (comboStarted)
        {
            if (comboCount == 0)
            {
                firstComboSpell = "Arcane";
                AudioManager.Instance.playSpellSound("SpellSelect");
                comboCount++;
            }
            else if (comboCount == 1 && firstComboSpell != "Arcane")
            {
                secondComboSpell = "Arcane";
                AudioManager.Instance.playSpellSound("SpellSelect");
                comboFailedSound = true;
                comboCount++;
            }
        }
        else
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                Debug.Log("Arcane Spell");
                spellBeingCast = "Arcane";
                ArcaneSpellCast.Cast();
            }
        }

    }
    private void OnComboStarted(InputAction.CallbackContext obj)
    {
        if (comboStarted)
        {
            comboStarted = false;
            comboFailedSound = true;
            AudioManager.Instance.playSpellSound("ComboStop");
            Debug.Log("Combo Canceled");
        }
        else if (!comboStarted)
        {
            if (playerController.onGround && playerController.moveDirection == Vector2.zero)
            {
                comboStarted = true;
                comboFailedSound = false;
                AudioManager.Instance.playSpellSound("ComboStart");
                Debug.Log("Combo Started");
            }
        }

    }

    public string getSpellBeingCast()
    {
        return spellBeingCast;
    }

}
