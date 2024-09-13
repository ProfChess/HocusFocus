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
    private int playerLayer;
    private int pDashLayer;
    private int pTeleportLayer;

    [Header("Player Movement/Stats")]
    public float playerSpeed = 5f;
    private float playerJumpforce = 9f;
    private float maxJumpTime = 0.3f;
    private float jumpTimer;
    private float gravityMulti = 1.5f;
    private bool isJumping = false;
    public float playerDashSpeed = 10f;
    private float playerDashDuration = 0.3f;
    private float playerDashCooldown = 1f;
    public float playerTeleportSpeed = 30f;
    private float playerTeleportDuration = 0.15f;
    private float playerTeleportCooldown = 3f;

    private float dashCooldownTimer;
    private Vector2 groundDetectRange = new (0.75f, 0.1f);
    public Vector2 moveDirection;
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

    //Animation Variables
    [Header("Player Animation")]
    public Animator playerAnim;
    public GameObject playerVisual;

    //player freeze variable
    private bool isCasting = false;


    private void Awake()
    {
        inputActions = new PlayerInput();
        rb = GetComponent<Rigidbody2D>();
        groundTransform = transform.Find("GroundCheck");
        groundLayer = LayerMask.GetMask("Ground");
        playerLayer = LayerMask.NameToLayer("Player");
        pDashLayer = LayerMask.NameToLayer("PlayerDashLayer");
        pTeleportLayer = LayerMask.NameToLayer("PlayerTeleportLayer");
    }

    private void Start()
    {
        playerDash = GameManager.Instance.getDashBool();
        playerDoubleJump = GameManager.Instance.getJumpBool();
        playerTeleport = GameManager.Instance.getTeleportBool();
    }

    private void OnEnable()
    {
        inputActions.Enable();
        //Move left/right
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        //Jump
        inputActions.Player.Jump.started += OnJumpPressed;
        inputActions.Player.Jump.canceled += OnJumpReleased;
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
        inputActions.Player.Jump.started -= OnJumpPressed;
        inputActions.Player.Jump.canceled -= OnJumpReleased;
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
            //animation 
            playerAnim.SetBool("PlayerMoving", true);
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {   
        if (!isCasting)
        {
            moveDirection = Vector2.zero;
            playerAnim.SetBool("PlayerMoving", false);
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
    private void OnJumpPressed(InputAction.CallbackContext context)
    {   
        if (!isCasting)
        {
            if (playerDoubleJump)
            {
                if (onGround) //Jump Calc
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0f);
                    rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
                    isJumping = true;
                    jumpTimer = maxJumpTime;
                    playerAnim.SetBool("PlayerJump", true);
                }
                if (!onGround && !doubleJumped) //Double Jump Calc
                {
                    rb.velocity = new Vector2(rb.velocity.x, 0);
                    rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
                    doubleJumped = true;
                    playerAnim.Play("PlayerJump", 0, 0f);
                }
            }

            else if (onGround) //No Double Jump Unlocked
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                rb.AddForce(Vector2.up * playerJumpforce, ForceMode2D.Impulse);
                isJumping = true;
                jumpTimer = maxJumpTime;
                playerAnim.SetBool("PlayerJump", true);
            }


        }
        
    }

    private void OnJumpReleased(InputAction.CallbackContext context)
    {
        isJumping = false;
    }

    //Dash
    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (!dashed && dashCooldownTimer <= 0f && playerDash && singleDash && !isCasting)
        {   
            singleDash = false;
            playerAnim.SetTrigger("PlayerDash");
            StartCoroutine(Dash());
        }
    }

    //Teleport
    private void OnTeleport(InputAction.CallbackContext context)
    {
        if (!isCasting)
        {
            if (!teleported && teleportCooldownTimer <= 0f && playerTeleport)
            {
                playerAnim.Play("PlayerTeleport");
            }

        }
    }

    public void teleportPlayer()
    {
        Vector2 teleportDirection = lastMoveDirection;
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
            teleportDirection = Vector2.right;
        }

        if (!teleported && teleportCooldownTimer <= 0f && playerTeleport)
        {
            StartCoroutine(Teleport(teleportDirection));
        }
    }

    private void FixedUpdate()
    {

        //Jumping Detection
        if (isJumping)
        {
            if (jumpTimer > 0f)
            {
                rb.AddForce(Vector2.up * 14f, ForceMode2D.Force);
                jumpTimer -= Time.fixedDeltaTime;
            }
            else
            {
                isJumping = false;
            }
        }

        if (!isJumping)
        {
            rb.gravityScale = gravityMulti;
        }



    }
    private void Update()
    {
        //Ground Detection
        onGround = Physics2D.OverlapBox(groundTransform.position, groundDetectRange, 0, groundLayer);

        if (!dashed && !isCasting)
        {
            transform.Translate(moveDirection * playerSpeed * Time.deltaTime);
        }

        if (onGround)
        {
            doubleJumped = false;
            singleDash = true;
            playerAnim.SetBool("PlayerJump", false);
        }

        if (!onGround)
        {
            playerAnim.SetBool("PlayerJump", true);
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
        playerDashLayer();
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
        playerBasicLayer();
    }

    private IEnumerator Teleport(Vector2 teleportDirection)
    {
        teleported = true;
        playerTeleportLayer();
        float startTime = Time.time;
        playerVisual.GetComponent<SpriteRenderer>().sortingOrder = -16;
        while (Time.time < startTime + playerTeleportDuration)
        {
            transform.Translate(teleportDirection * playerTeleportSpeed *  Time.deltaTime);
            yield return null;
        }

        teleported = false; 
        teleportCooldownTimer = playerTeleportCooldown;
        rb.velocity = Vector2.zero;
        playerVisual.GetComponent<SpriteRenderer>().sortingOrder = 1;
        playerBasicLayer();
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
        playerVisual.GetComponent<SpriteRenderer>().flipX = !lookingRight;
    }

    //Player Immunity / Not immunity
    private void playerDashLayer()
    {
        gameObject.layer = pDashLayer;
    }

    private void playerBasicLayer()
    {
        gameObject.layer = playerLayer;
    }

    private void playerTeleportLayer()
    {
        gameObject.layer = pTeleportLayer;
    }

    //Gets
    public bool getLookingRight()
    {
        return lookingRight;
    }

}
