using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;


public class TestPlayerController : MonoBehaviour
{
    [Header("References")]
    private PlayerInput inputActions;
    private Rigidbody2D rb;
    private LayerMask groundLayer;
    private Transform groundTransform;
    [HideInInspector]
    public bool onGround;
    private int playerLayer;
    private int pDashLayer;
    private int pTeleportLayer;

    [Header("Player Movement/Stats")]
    //Left/Right
    public float playerSpeed = 5f;                  //Movement speed of player
    public Vector2 moveDirection;                   //Direction player moves in
    private Vector2 lastMoveDirection;
    //Jumping
    private float playerJumpforce = 9f;             //Force for initial jump of player
    private float maxJumpTime = 0.3f;               //Length of time the jump height can be increased
    private float jumpTimer;                        //Countdown for extending jump height
    private float gravityMulti = 1.5f;              //Gravity multiplier for falling faster
    private bool isJumping = false;                 //Tracks if player performes jump from ground
    private bool jumpHeld = false;                  //Tracks if player is holding jump button
    private bool canDoubleJump = false;             //Tracks if player has double jumped -- resets upon touching ground
    //Dashing
    public float playerDashSpeed = 10f;             //Movement speed during dash
    private float playerDashDuration = 0.3f;        //Duration of dash
    private float playerDashCooldown = 1f;          //Cooldown of dash
    private float dashCooldownTimer;                //Tracks cooldown for dash 
    private bool canDash = true;
    private Vector2 dashDir;
    //Teleporting
    public float playerTeleportSpeed = 30f;
    private float playerTeleportDuration = 0.15f;
    private float playerTeleportCooldown = 3f;

    private float groundDetectRange = 1f;
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

    private void OnEnable()
    {
        inputActions.Enable();
        //Move left/right
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        //Jump
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Jump.canceled += OnJump;
        //Dash
        inputActions.Player.Dash.performed += OnDash;
        //Teleport/look
        //inputActions.Player.Teleport.performed += OnTeleport;
        //inputActions.Player.LookUp.performed += OnLookUpPerformed;
        //inputActions.Player.LookUp.canceled += OnLookUpCanceled;
        //inputActions.Player.LookDown.performed += OnLookDownPerformed;
        //inputActions.Player.LookDown.canceled += OnLookDownCanceled;
    }

    private void OnDisable()
    {   //Move
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        //Jump
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Jump.canceled -= OnJump;
        //Dash
        inputActions.Player.Dash.performed -= OnDash;
        //Teleport/look
        //inputActions.Player.Teleport.performed -= OnTeleport;
        //inputActions.Player.LookUp.performed -= OnLookUpPerformed;
        //inputActions.Player.LookUp.canceled -= OnLookUpCanceled;
        //inputActions.Player.LookDown.performed -= OnLookDownPerformed;
        //inputActions.Player.LookDown.canceled -= OnLookDownCanceled;
        inputActions.Disable();

    }

    private void Awake()
    {
        inputActions = new PlayerInput();                   //Input Settings for Movement/Abilities
        rb = GetComponent<Rigidbody2D>();                   //Rigidbody for moving
        groundTransform = transform.Find("GroundCheck");    //Gets the GroundCheck Component
        groundLayer = LayerMask.GetMask("Ground");          //Layer that triggers GroundCheck
    }
    private void Start()
    {
        
    }

    //Physics Calculations
    private void FixedUpdate()
    {
        //Ground Detection
        onGround = Physics2D.Raycast(transform.position, Vector2.down, groundDetectRange, groundLayer);

        //Left and Right Movement
        Vector2 vel = rb.velocity;
        if (!dashed)
        {
            vel.x = moveDirection.x * playerSpeed;
        }
        //Dash 
        if (dashed)
        {
            vel.x = dashDir.x * playerDashSpeed;
        }
        rb.velocity = vel;


        //Jumping Detection
        if (isJumping && onGround)
        {
            rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
            isJumping = false;
            jumpTimer = 0f;
        }
        if (jumpHeld && jumpTimer < maxJumpTime)
        {
            rb.AddForce(Vector2.up * 14, ForceMode2D.Force);
            jumpTimer += Time.fixedDeltaTime;
        }

        //DoubleJump
        if (doubleJumped && canDoubleJump)
        {
            canDoubleJump = false;
            Vector2 x = new Vector2(rb.velocity.x, 0f);
            rb.velocity = x;
            rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
            doubleJumped = false;
        }

        if (!isJumping) //Changes gravity on player to fall faster
        {
            rb.gravityScale = gravityMulti;
        }





    }

    private void Update()
    {
        if (onGround)
        {
            canDoubleJump = true;
        }

        //Cooldowns
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    //Movement
    //Left/Right
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = new Vector2 (context.ReadValue<float>(), 0f);
        if (moveDirection != Vector2.zero) //Records last moved direction for dash and teleport
        {
            lastMoveDirection = moveDirection;
        }
    }

    //Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && onGround) //Performed when grounded
        {
            isJumping = true;
            jumpHeld = true;
        }
        if (context.performed && !onGround && playerDoubleJump && !doubleJumped && canDoubleJump) //performed mid air with double jump unlocked
        {
            doubleJumped = true;
        }
        if (context.canceled) //player stops holding jump
        {
            jumpHeld = false;
        }
    }

    //Dash
    public void OnDash(InputAction.CallbackContext context)
    {
        if (canDash && playerDash && !dashed && dashCooldownTimer <= 0f && singleDash)
        {
            singleDash = false;
            StartCoroutine(Dash());
        }
        else
        {

        }
    }

    private IEnumerator Dash()
    {
        playerDashLayer();
        if (moveDirection == Vector2.zero)
        {
            dashDir = lastMoveDirection.normalized;
        }
        else
        {
            dashDir = moveDirection.normalized;
        }

        float startTime = Time.time;
        dashed = true;
        while (Time.time < startTime + playerDashDuration)
        {
            yield return null;
        }

        dashed = false;
        dashCooldownTimer = playerDashCooldown;
        singleDash = true;
        playerBasicLayer();
    }

    //Player Layers
    private void playerDashLayer() //Moves to Dash layer 
    {
        gameObject.layer = pDashLayer;
    }

    private void playerBasicLayer() //Moves to main player layer
    {
        gameObject.layer = playerLayer;
    }



}
