using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private PlayerInput inputActions;
    private Rigidbody2D rb;
    private LayerMask groundLayer;
    private Transform groundTransform;
    [HideInInspector]
    public bool onGround;
    private GameObject playerVisuals;
    private int playerLayer;
    private int playerImmuneLayer;

    [Header("Player Movement/Stats")]
    public float playerSpeed = 5f;
    public float playerJumpforce = 8f;
    public float playerDashSpeed = 10f;
    private float playerDashDuration = 0.3f;
    private float playerDashCooldown = 1f;
    public float playerTeleportSpeed = 30f;
    private float playerTeleportDuration = 0.15f;
    private float playerTeleportCooldown = 3f;

    private float dashCooldownTimer;
    private float groundDetectRadius = 0.2f;
    private Vector2 moveDirection;
    private Vector2 lastMoveDirection;
    private float teleportCooldownTimer;
    private bool lookUp;
    private bool lookDown;
    public bool lookingRight = true;

    [Header("Player bool Abilites")]
    public bool playerDash = false;
    public bool playerDoubleJump = false;
    public bool playerTeleport = false;
    private bool teleported = false;
    private bool doubleJumped = false;
    private bool dashed = false;
    private bool singleDash = true;

    //player freeze variable
    private bool isCasting = false;

    private void Awake()
    {
        inputActions = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        groundTransform = transform.Find("GroundCheck");
        playerVisuals = GameObject.Find("PlayerVisual");
        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.NameToLayer("Player");
        playerImmuneLayer = LayerMask.NameToLayer("PlayerImmuneLayer");
    }

    private void OnEnable()
    {
        inputActions.Enable();
        //Move left/right
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        //Jump
        inputActions.Player.Jump.performed += OnJumpPerformed;
        //Dash
        inputActions.Player.Dash.performed += OnDashPerformed;
        //Teleport/look
        inputActions.Player.Teleport.performed += OnTeleport;
        inputActions.Player.LookUp.performed += OnLookUpPerformed;
        inputActions.Player.LookUp.canceled += OnLookUpCanceled;
        inputActions.Player.LookDown.performed += OnLookDownPerformed;
        inputActions.Player.LookDown.canceled += OnLookDownCanceled;
    }

    private void OnDisable()
    {   //Move
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        //Jump
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        //Dash
        inputActions.Player.Dash.performed -= OnDashPerformed;
        //Teleport/look
        inputActions.Player.Teleport.performed -= OnTeleport;
        inputActions.Player.LookUp.performed -= OnLookUpPerformed;
        inputActions.Player.LookUp.canceled -= OnLookUpCanceled;
        inputActions.Player.LookDown.performed -= OnLookDownPerformed;
        inputActions.Player.LookDown.canceled -= OnLookDownCanceled;
        inputActions.Disable();

    }

    //Move
    private void OnMovePerformed(InputAction.CallbackContext context)
    {   
        if (!isCasting)
        {
            float newDirection = context.ReadValue<float>();
            moveDirection = new Vector2(newDirection, 0);
            if (moveDirection != Vector2.zero)
            {
                lastMoveDirection = moveDirection;
                if (moveDirection.x > 0 && !lookingRight)
                {
                    flipPlayer();
                }
                else if (moveDirection.x < 0 && lookingRight)
                {
                    flipPlayer();
                }
            }
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {   
        if (!isCasting)
        {
            moveDirection = Vector2.zero;
        }
    }

    //Look

    private void OnLookUpPerformed(InputAction.CallbackContext context)
    {
        lookUp = true;
    }
    private void OnLookUpCanceled(InputAction.CallbackContext context)
    {
        lookUp = false;
    }
    private void OnLookDownPerformed(InputAction.CallbackContext context)
    {
        lookDown = true;
    }
    private void OnLookDownCanceled(InputAction.CallbackContext context)
    {
        lookDown = false;
    }

    //Jump
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {   
        if (!isCasting)
        {
            if (playerDoubleJump)
            {
                if (onGround)
                {
                    rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
                }
                if (!onGround && !doubleJumped)
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
                    doubleJumped = true;
                }
            }

            if (!playerDoubleJump && onGround)
            {
                rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
            }
        }
        
    }

    //Dash
    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (!dashed && dashCooldownTimer <= 0f && playerDash && singleDash && !isCasting)
        {   
            singleDash = false;
            StartCoroutine(Dash());
        }
    }

    //Teleport
    private void OnTeleport(InputAction.CallbackContext context)
    {
        if (!isCasting)
        {
            Vector2 teleportDirection = Vector2.zero;
            if (lookUp)
            {
                teleportDirection = Vector2.up;
            }
            else if (lookDown)
            {
                teleportDirection = Vector2.down;
            }

            if (teleportDirection == Vector2.zero)
            {
                teleportDirection = lastMoveDirection;
            }

            if (!teleported && teleportCooldownTimer <= 0f && playerTeleport)
            {
                StartCoroutine(Teleport(teleportDirection));
            }
        }
    }

    private void Update()
    {
        onGround = Physics2D.OverlapCircle(groundTransform.position, groundDetectRadius, groundLayer);

        if (onGround)
        {
            doubleJumped = false;
            singleDash = true;
        }

        if (!dashed && !isCasting)
        {   
            transform.Translate(moveDirection * playerSpeed * Time.deltaTime);
        }

        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }

        if (teleportCooldownTimer > 0f)
        {
            teleportCooldownTimer -= Time.deltaTime;
        }

    }

    private IEnumerator Dash()
    {
        dashed = true;
        playerImmune();
        Vector2 dashDir;
        if (moveDirection == Vector2.zero)
        {
            dashDir = lastMoveDirection.normalized;
        }
        else
        {
            dashDir = moveDirection.normalized;
        }

        float startTime = Time.time;

        while (Time.time < startTime + playerDashDuration)
        {
            transform.Translate(dashDir * playerDashSpeed * Time.deltaTime);
            yield return null;
        }

        dashed = false;
        dashCooldownTimer = playerDashCooldown;
        playerVuln();
    }

    private IEnumerator Teleport(Vector2 teleportDirection)
    {
        teleported = true;
        playerImmune();
        float startTime = Time.time;
        playerVisuals.SetActive(false);
        while (Time.time < startTime + playerTeleportDuration)
        {
            transform.Translate(teleportDirection * playerTeleportSpeed *  Time.deltaTime);
            yield return null;
        }

        teleported = false; 
        teleportCooldownTimer = playerTeleportCooldown;
        playerVisuals.SetActive(true);
        playerVuln();
    }

    //Player stop/start casting
    public void playerStartCast()
    {
        isCasting = true;
        moveDirection = Vector2.zero;
    }

    public void playerStopCast()
    {
        isCasting = false;
    }

    //Flips player direction
    private void flipPlayer()
    {
        lookingRight = !lookingRight;
    }

    //Player Immunity / Not immunity
    private void playerImmune()
    {
        gameObject.layer = playerImmuneLayer;
    }

    private void playerVuln()
    {
        gameObject.layer = playerLayer;
    }
}
