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
    private float groundDetectRange = 1f;
    //Dashing
    public float playerDashSpeed = 10f;             //Movement speed during dash
    private float playerDashDuration = 0.3f;        //Duration of dash
    private float playerDashCooldown = 1f;          //Cooldown of dash
    private float dashCooldownTimer;                //Tracks cooldown for dash 
    private Vector2 dashDir;                        //Direction of dash
    //Teleporting
    public float playerTeleportSpeed = 30f;         //Movement speed during teleport
    private float playerTeleportDuration = 0.15f;   //Duration of teleport
    private float playerTeleportCooldown = 3f;      //Cooldown of teleport
    private float teleportCooldownTimer;            //Tracks cooldown for teleport
    private Vector2 teleDir;                        //Direction of teleport
    //Looking
    private bool lookUp;                            //Player is looking up (Holding W)
    private bool lookDown;                          //Player is looking down (Holding S)
    public bool lookingRight = true;
    //Physics
    private bool reVel = false;                     //Resets player velocity


    [Header("Player bool Abilites")]
    public bool playerDash = false;
    public bool playerDoubleJump = false;
    public bool playerTeleport = false;
    private bool teleported = false;
    private bool doubleJumped = false;
    private bool dashed = false;


    //Animation/Visual Components
    public SpriteRenderer playerVisual;
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
        inputActions.Player.Teleport.performed += OnTeleport;
        inputActions.Player.LookUp.performed += OnLookUp;
        inputActions.Player.LookUp.canceled += OnLookUp;
        inputActions.Player.LookDown.performed += OnLookDown;
        inputActions.Player.LookDown.canceled += OnLookDown;
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
        inputActions.Player.Teleport.performed -= OnTeleport;
        inputActions.Player.LookUp.performed -= OnLookUp;
        inputActions.Player.LookUp.canceled -= OnLookUp;
        inputActions.Player.LookDown.performed -= OnLookDown;
        inputActions.Player.LookDown.canceled -= OnLookDown;
        inputActions.Disable();

    }

    private void Awake()
    {
        inputActions = new PlayerInput();                   //Input Settings for Movement/Abilities
        rb = GetComponent<Rigidbody2D>();                   //Rigidbody for moving
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

        //Reset Velocity if Needed
        if (reVel)
        {
            reVel = false;
            rb.velocity = Vector2.zero;
        }

        //Left and Right Movement
        Vector2 vel = rb.velocity;
        if (!dashed && !teleported)
        {
            vel.x = moveDirection.x * playerSpeed;
        }
        //Dash 
        if (dashed)
        {
            vel.x = dashDir.x * playerDashSpeed;
        }
        //Teleport
        if (teleported)
        {
            if (teleDir == Vector2.up || teleDir == Vector2.down)
            {
                vel.y = teleDir.y * playerTeleportSpeed;
            }
            else
            {
                vel.x = teleDir.x * playerTeleportSpeed;
            }
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
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
            doubleJumped = false;
        }

        if (!onGround) //Changes gravity on player to fall faster
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
        //Dash
        if (dashCooldownTimer > 0f)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
        //Teleport
        if (teleportCooldownTimer > 0f)
        {
            teleportCooldownTimer -= Time.deltaTime;
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
        //Dash must be unlocked
        //Not currently dashing
        //Dash not on cooldown
        if (playerDash && !dashed && dashCooldownTimer <= 0f)
        {
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
        playerBasicLayer();
    }

    //Teleport
    public void OnTeleport(InputAction.CallbackContext context)
    {
        //Teleport must be unlocked
        //Not currently teleporting
        //teleport not on cooldown
        if (playerTeleport && !teleported && teleportCooldownTimer <= 0f)
        {
            StartCoroutine(Teleport());
        }
        else
        {

        }
    }
    private IEnumerator Teleport()
    {
        playerTeleportLayer();
        setTeleportDirection();
        float startTime = Time.time;
        teleported = true;
        playerVisual.sortingOrder = -16;
        while (Time.time < startTime + playerTeleportDuration)
        {
            yield return null;
        }
        teleported = false;
        reVel = true;
        teleportCooldownTimer = playerTeleportCooldown;
        playerVisual.sortingOrder = 1;
        playerBasicLayer();
    }
    //Set direction of teleport based on if player is looking up/down/left/right
    private void setTeleportDirection()
    {
        if (lookUp)
        {
            teleDir = Vector2.up;
        }
        else if (lookDown)
        {
            teleDir = Vector2.down;
        }
        else if (moveDirection == Vector2.zero)
        {
            teleDir = lastMoveDirection;
        }
        else
        {
            teleDir = moveDirection;
        }
    } 


    //Looking Direction (Used to teleport up/down)
    //Look Up
    private void OnLookUp(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lookUp = true;
        }
        if (context.canceled)
        {
            lookUp = false;
        }
    }
    //Look Down
    private void OnLookDown(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            lookDown = true;
        }
        if (context.canceled)
        {
            lookDown = false;
        }
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

    private void playerTeleportLayer() //Moves to teleport layer
    {
        gameObject.layer = pTeleportLayer;
    }

}
