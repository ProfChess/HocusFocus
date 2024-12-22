using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
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
    private GameManager GM;

    [Header("Player Movement/Stats")]
    //Left/Right
    public float playerSpeed = 5f;                  //Movement speed of player
    public Vector2 moveDirection;                   //Direction player moves in
    private Vector2 lastMoveDirection = Vector2.right;
    //Jumping
    private float playerJumpforce = 9f;             //Force for initial jump of player
    private float maxJumpTime = 0.3f;               //Length of time the jump height can be increased
    private float jumpTimer;                        //Countdown for extending jump height
    private float gravityMulti = 1.5f;              //Gravity multiplier for falling faster
    private bool isJumping = false;                 //Tracks if player performes jump from ground
    private bool jumpHeld = false;                  //Tracks if player is holding jump button
    private bool canDoubleJump = false;             //Tracks if player has double jumped -- resets upon touching ground
    public Vector2 groundDetectBox;
    private float groundDetectDistance = 0.7f;
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
    private bool revealPlayer = true;
    public Vector2 revealSize;
    private bool teleportHide = false;
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
    [Header("Animation")]
    public SpriteRenderer playerVisual;
    public Animator playerAnim;

    //Menu
    private bool isPaused = false;

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

        //Menus
        inputActions.Player.PauseGame.performed += OnPause;
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

        //Menus
        inputActions.Player.PauseGame.performed -= OnPause;
        inputActions.Disable();

    }

    private void Awake()
    {
        inputActions = new PlayerInput();                   //Input Settings for Movement/Abilities
        rb = GetComponent<Rigidbody2D>();                   //Rigidbody for moving
        groundLayer = LayerMask.GetMask("Ground");          //Layer that triggers GroundCheck
        playerLayer = LayerMask.NameToLayer("Player");
        pDashLayer = LayerMask.NameToLayer("PlayerDashLayer");
        pTeleportLayer = LayerMask.NameToLayer("PlayerTeleportLayer");
        
    }
    private void Start()
    {
        GM = GameManager.Instance;
        GM.player = gameObject;
        if (GM.respawn)
        {
            transform.position = GM.respawnLocation;
            gameObject.GetComponent<PlayerHealth>().pickupHealth();
            GM.savedHealth = gameObject.GetComponent<PlayerHealth>().maxHealth;
            GM.respawn = false;
        }
        revealSize = new Vector2(0.72f, 1.33f);
        playerDash = GM.getDashBool();
        playerDoubleJump = GM.getJumpBool();
        playerTeleport = GM.getTeleportBool();
    }

    public void travelPlace()
    {
        transform.position = GameManager.Instance.TempFastTravelLocation;
        gameObject.GetComponent<PlayerHealth>().pickupHealth();
        GameManager.Instance.savedHealth = gameObject.GetComponent<PlayerHealth>().maxHealth;
    }
    public void startLocation(Vector2 spawn)
    {
        transform.position = spawn;
    }

    //Physics Calculations
    private void FixedUpdate()
    {
        //Ground Detection
        onGround = Physics2D.BoxCast(transform.position, groundDetectBox, 0, -transform.up, groundDetectDistance, groundLayer);

        //Overlap Detection
        revealPlayer = Physics2D.OverlapBox(transform.position, revealSize, 0, groundLayer);

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
            vel.y = 0f;
        }
        //Teleport
        if (teleported)
        {
            if (teleDir == Vector2.up || teleDir == Vector2.down)
            {
                vel.y = teleDir.y * playerTeleportSpeed;
                vel.x = 0f;
            }
            else
            {
                vel.x = teleDir.x * playerTeleportSpeed;
                vel.y = 0f;
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
        if (jumpHeld && jumpTimer < maxJumpTime && rb.velocity.y >= 0)
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

        //Disable Gravity if Dashing or Teleporting
        if (dashed || teleported)
        {
            rb.gravityScale = 0f;
        }
        else if (!onGround) //Changes gravity on player to fall faster
        {
            rb.gravityScale = gravityMulti;
        }

    }

    private void Update()
    {
        if (Time.deltaTime == 0f)
        {
            return;
        }
        //Collision Detection for Teleport 
        if (teleportHide)
        {
            if (revealPlayer) 
            {
                playerVisual.sortingOrder = -16;
            }
            else if (!revealPlayer && !teleported)//Hide player until teleport is finished and not inside a collider
            {
                playerVisual.sortingOrder = 3;
                teleportHide = false;
            }
        }

        //Ground Detection -> Animation/Sound Triggers
        if (onGround)
        {
            canDoubleJump = true;
            playerAnim.SetBool("PlayerJump", false);
        }
        if (!onGround)
        {
            playerAnim.SetBool("PlayerJump", true);
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

        //Sounds
        if (AudioManager.Instance != null)
        {
            if (onGround && moveDirection.magnitude > 0 && !AudioManager.Instance.checkSoundPlaying())
            {
                AudioManager.Instance.startMovingSound();
            }
            else if (!onGround || moveDirection.magnitude == 0)
            {
                AudioManager.Instance.stopMovingSound();
            }
        }
        
    }

    //Movement
    //Left/Right
    public void OnMove(InputAction.CallbackContext context)
    {
        if (Time.deltaTime == 0)
        {
            return;
        }
        moveDirection = new Vector2 (context.ReadValue<float>(), 0f);
        if (moveDirection != Vector2.zero) //Records last moved direction for dash and teleport
        {
            lastMoveDirection = moveDirection;
        }

        //Animation Changes
        if (moveDirection == Vector2.zero)
        {
            playerAnim.SetBool("PlayerMoving", false);
        }
        else
        {
            playerAnim.SetBool("PlayerMoving", true);
            if (moveDirection.x > 0f)
            {
                playerVisual.flipX = false;
            }
            else
            {
                playerVisual.flipX = true;
            }
        }
    }

    //Jump
    public void OnJump(InputAction.CallbackContext context)
    {
        if (Time.deltaTime == 0)
        {
            return;
        }
        if (context.performed && onGround) //Performed when grounded
        {
            isJumping = true;
            jumpHeld = true;
            playJumpSound();
        }
        if (context.performed && !onGround && playerDoubleJump && !doubleJumped && canDoubleJump) //performed mid air with double jump unlocked
        {
            doubleJumped = true;
            playerAnim.Play("PlayerJump", 0, 0f);
            playJumpSound();
        }
        if (context.canceled) //player stops holding jump
        {
            jumpHeld = false;
        }
    }

    //Jump Sound
    private void playJumpSound()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.playPlayerSound("Jump");
        }
    }


    //Dash
    public void OnDash(InputAction.CallbackContext context)
    {
        if (Time.deltaTime == 0)
        {
            return;
        }
        //Dash must be unlocked
        //Not currently dashing
        //Dash not on cooldown
        if (playerDash && !dashed && dashCooldownTimer <= 0f)
        {
            StartCoroutine(Dash());
            playerAnim.SetTrigger("PlayerDash"); //Animation Trigger
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.playPlayerSound("Dash");  //Sound Trigger
            }
            
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
        if (Time.deltaTime == 0)
        {
            return;
        }
        //Teleport must be unlocked
        //Not currently teleporting
        //teleport not on cooldown
        if (playerTeleport && !teleported && teleportCooldownTimer <= 0f)
        {
            playerAnim.Play("PlayerTeleport");
        }
        else
        {

        }
    }
    public void playerTeleportTrigger() //Used to trigger teleport at specific anim frame
    {
        StartCoroutine (Teleport());

        //Sound Trigger
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.playPlayerSound("Teleport");
        }
    }
    private IEnumerator Teleport()
    {
        playerTeleportLayer();
        setTeleportDirection();
        float startTime = Time.time;
        teleported = true;
        playerVisual.sortingOrder = -16;
        teleportHide = true;
        while (Time.time < startTime + playerTeleportDuration)
        {
            yield return null;
        }
        teleported = false;
        reVel = true;
        teleportCooldownTimer = playerTeleportCooldown;
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

    public bool getLookingRight() //used for spawning spell in correct direction/location in BaseSpellCast
    {
        if (lastMoveDirection == Vector2.right)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //Setter for lastmovedirection -> used by game manager to set upon entering room and switching direction at spawn
    public void setMoveDirection(Vector2 move)
    {
        lastMoveDirection = move;
    }

    //ESC Button
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed && !isPaused)
        {
            UIManager.Instance.pauseGame();
            isPaused = true;
        }
        else if (context.performed && isPaused)
        {
            UIManager.Instance.UnpauseGame();
            isPaused = false;
        }
    }

}
