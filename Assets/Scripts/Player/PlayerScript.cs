using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    #region Movement Variable Declarations

    [Header("References")]
    [HideInInspector] public Rigidbody2D rb;
    GroundedScript groundedScript;
    InputHandlerScript inputHandler;
    PlayerInteractionScript playerInteractionScript;

    [Header("Jumping/Gravity")]
    public float jumpForce;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    [SerializeField] private float gravity;
    [SerializeField] private float afterApexGravity;
    [SerializeField] private float variableJumpForce;

    [HideInInspector] public bool hasJumped;

    [Header("Movement On Ground")]
    [SerializeField] private float groundSpeed;
    [SerializeField] private float maxSpeedOnGround;
    [SerializeField] private float decelerationOnGround;
    [SerializeField] private float turningSpeedOnGround;
    [SerializeField] private float resetGroundSpeed;

    [Header("Movement In Air")]
    [SerializeField] private float airSpeed;
    [SerializeField] private float maxSpeedInAir;
    [SerializeField] private float decelerationInAir;
    [SerializeField] private float turningSpeedInAir;
    [SerializeField] private float resetAirSpeed;

    #endregion

    #region ShiftingMechanic Variable Declerations

    [Header("Shifting")] 
    public float shiftHeight;
    private bool touchedGround;
    [HideInInspector] public float trueVelocity;
    [HideInInspector] public bool hasTeleported;
    [HideInInspector] public bool isNotBlocked;
    [SerializeField] private float shiftCountDownInSeconds;
    private float shiftCountDown;

    [Space(10)]
    [SerializeField] private GameObject shiftingAnim;
    [SerializeField] private GameObject shiftingAnim1;
    [SerializeField] private float shiftingAnimDestruction;
    private GameObject spawnedShiftingAnim;
    private GameObject spawnedShiftingAnim1;
    
    #endregion

    [Header("Misc")] 
    [SerializeField] MomentumWheelScript momentumWheel;
    [SerializeField] private SpriteRendererScript spriteRendererScript;
    [SerializeField] private GameManagerScript gameManager;
    [HideInInspector] public bool isHit;

    private void Awake()
    {
        //Puts the Player at either the start of the level or the last Checkpoint
        transform.position = GameManagerScript.checkPointPosition;
    }

    private void Start()
    {
        //Gets the references
        groundedScript = GetComponentInChildren<GroundedScript>();
        inputHandler = GetComponent<InputHandlerScript>();
        rb = GetComponent<Rigidbody2D>();
        playerInteractionScript = GetComponent<PlayerInteractionScript>();
    }

    private void Update()
    {
        VariablesReset();

        Shifting();
        
        //Debug.Log(playerInteractionScript.isHeadingDownwards);
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        #region Jumping
        
        //Jumping
        if ((inputHandler.JumpInput() || inputHandler.jumpBuffer) && (coyoteTimeCounter > 0))
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            hasJumped = true;
            inputHandler.jumpBuffer = false;
            
            coyoteTimeCounter = 0;
            groundedScript.isGrounded = false;

            spriteRendererScript.isJumping = true;
        }

        //Adds an additional downwards force when the jump button is released
        if (hasJumped && rb.velocity.y > 0 && (!inputHandler.JumpInput() && !inputHandler.jumpHeld))
        {
            rb.AddForce(-Vector2.up * variableJumpForce);
        }


        #endregion

        #region Gravity

        //Normal Gravity
        rb.AddForce(-Vector2.up * gravity);

        //Gravity After JumpApex
        if (!groundedScript.isGrounded && rb.velocity.y < 0)
        {
            rb.AddForce(-Vector2.up * afterApexGravity);
        }

        #endregion

        #region Movement

        //Movement On Ground
        if (groundedScript.isGrounded)
        {
            //Accelerates into the input direction as long as its under the maxSpeed
            if (inputHandler.MovementInput().x < 0 && Mathf.Abs(rb.velocity.x) <= maxSpeedOnGround)
            {
                rb.AddForce(new Vector2(inputHandler.MovementInput().x * groundSpeed, 0));
            }
            if (inputHandler.MovementInput().x > 0 && Mathf.Abs(rb.velocity.x) <= maxSpeedOnGround)
            {
                rb.AddForce(new Vector2(inputHandler.MovementInput().x * groundSpeed, 0));
            }

            //Decelerates when no Input
            if (inputHandler.MovementInput().x == 0 && Mathf.Abs(rb.velocity.x) > resetGroundSpeed)
            {
                rb.AddForce(new Vector2(-Mathf.Sign(rb.velocity.x) * decelerationOnGround, 0));
            }

            //Resets the velocity if barely moving
            if (inputHandler.MovementInput().x == 0 && Mathf.Abs(rb.velocity.x) <= resetGroundSpeed)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            //Extra Deceleration when pressing in the opposite way of travel
            if (rb.velocity.x > resetGroundSpeed && inputHandler.MovementInput().x < 0)
            {
                rb.AddForce(Vector2.left * turningSpeedOnGround); 
                spriteRendererScript.isBraking = true;
            }
            else
            {
                spriteRendererScript.isBraking = false;
            }
            
            if (rb.velocity.x < -resetGroundSpeed && inputHandler.MovementInput().x > 0)
            {
                spriteRendererScript.isBraking = true;
                rb.AddForce(Vector2.right * turningSpeedOnGround);
            }
        }

        //Movement In Air
        if (!groundedScript.isGrounded && !isHit)
        {
            //Accelerates into the input direction as long as its under the maxSpeed
            if (inputHandler.MovementInput().x < 0 && Mathf.Abs(rb.velocity.x) <= maxSpeedInAir)
            {
                rb.AddForce(new Vector2(inputHandler.MovementInput().x * airSpeed, 0));
            }
            if (inputHandler.MovementInput().x > 0 && Mathf.Abs(rb.velocity.x) <= maxSpeedInAir)
            {
                rb.AddForce(new Vector2(inputHandler.MovementInput().x * airSpeed, 0));
            }

            //Decelerates when no Input
            if (inputHandler.MovementInput().x == 0 && Mathf.Abs(rb.velocity.x) > resetAirSpeed)
            {
                rb.AddForce(new Vector2(-Mathf.Sign(rb.velocity.x) * decelerationInAir, 0));
            }

            //Resets the velocity if barely moving
            if (inputHandler.MovementInput().x == 0 && Mathf.Abs(rb.velocity.x) <= resetAirSpeed)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }

            //Extra Deceleration when pressing in the opposite way of travel
            if (rb.velocity.x > resetAirSpeed)
            {
                if (inputHandler.MovementInput().x < 0)
                {
                    rb.AddForce(Vector2.left * turningSpeedInAir);
                }
            }
            else if (rb.velocity.x < -resetAirSpeed)
            {
                if (inputHandler.MovementInput().x > 0)
                {
                    rb.AddForce(Vector2.right * turningSpeedInAir);
                }
            }
        }

        #endregion
    }

    void VariablesReset()
    {
        //Resets variables when the player is Grounded
        if (groundedScript.isGrounded)
        {
            hasJumped = false;
            coyoteTimeCounter = coyoteTime;
            touchedGround = true;

            playerInteractionScript.isHeadingDownwards = false;
        }
        //Counts down the CoyoteTimer
        else 
        {
            coyoteTimeCounter -= Time.deltaTime;
            spriteRendererScript.isBraking = false;
        }

        //Starts the Jumpbuffer Coroutine
        if (inputHandler.jumpBuffer)
        {
            StartCoroutine(JumpBuffer());
        }

        //Resets the CoyoteTimer to 0 after jumping
        if (hasJumped)
        {
            coyoteTimeCounter = 0;
        }
        
        if (rb.velocity.y < 0)
        {
            spriteRendererScript.isJumping = false;
        }
    }

    void Shifting()
    {
        //When the Player holds down the Shifting Button it starts the Shift
        if (inputHandler.isHoldingTeleport())
        {
            if (touchedGround && !hasTeleported && isNotBlocked && shiftCountDown <= Time.time && Time.timeScale != 0)
            {
                //Gets the Magnitude of the velocity
                trueVelocity = rb.velocity.magnitude;

                spawnedShiftingAnim = Instantiate(shiftingAnim, transform.position, quaternion.identity);
                StartCoroutine(DestroyShiftAnim());
                
                //Stops Time and teleports the Player up
                Time.timeScale = 0.0f;
                transform.position = new Vector3(transform.position.x, transform.position.y + shiftHeight);


                spawnedShiftingAnim1 = Instantiate(shiftingAnim1, transform.position, quaternion.identity);
                StartCoroutine(DestroyShiftAnim1());
                
                //Sets some variables
                touchedGround = false;
                hasTeleported = true;
                gameManager.playerIsTeleporting = true;
                hasJumped = false;
                groundedScript.isGrounded = false;
                spriteRendererScript.isShifting = true;

                //Makes it so the player can jump right after Shifting
                coyoteTimeCounter = coyoteTime;

                //Resets Velocity
                rb.velocity = new Vector2();

                //Adds a timer to prevent a double-Shift-Bug
                shiftCountDown = Time.fixedTime + shiftCountDownInSeconds;
            }
        }
        //When the Player has Shifted, they can select which direction the want to Shift in
        if (hasTeleported)
        {
            //Right
            if (inputHandler.ShiftingVector() == new Vector2(1, 0) && !inputHandler.isHoldingTeleport())
            { 
                hasTeleported = false; 
                Time.timeScale = 1; 
                rb.velocity = new Vector2(momentumWheel.momentum, 0);

                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingSideways = true;
                gameManager.playerIsTeleporting = false;

            }
            //Up-Right
            if (inputHandler.ShiftingVector() == new Vector2(1, 1) && !inputHandler.isHoldingTeleport())
            { 
                hasTeleported = false;
                Time.timeScale = 1;
                rb.velocity = new Vector2(momentumWheel.momentum * 0.71f, momentumWheel.momentum * 1f);

                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDiagonallyUp = true;
                gameManager.playerIsTeleporting = false;
            }
            //Up
            if (inputHandler.ShiftingVector() == new Vector2(0, 1) && !inputHandler.isHoldingTeleport())
            { 
                 hasTeleported = false; 
                Time.timeScale = 1;
                rb.velocity = new Vector2(0, momentumWheel.momentum * 1.5f);

                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingUp = true;
                gameManager.playerIsTeleporting = false;
            }
            //Up-Left
            if (inputHandler.ShiftingVector() == new Vector2(-1, 1) && !inputHandler.isHoldingTeleport())
            { 
                 hasTeleported = false;
                 Time.timeScale = 1;
                 rb.velocity = new Vector2(-momentumWheel.momentum * 0.71f, momentumWheel.momentum * 1f);
                 
                 spriteRendererScript.isShifting = false;
                 spriteRendererScript.isShiftingDiagonallyUp = true;
                 gameManager.playerIsTeleporting = false;
            }
            //Left
            if (inputHandler.ShiftingVector() == new Vector2(-1, 0) && !inputHandler.isHoldingTeleport())
            { 
                hasTeleported = false; 
                Time.timeScale = 1; 
                rb.velocity = new Vector2(-momentumWheel.momentum, 0);
                
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingSideways = true;
                gameManager.playerIsTeleporting = false;
            }
            //Down-Left
            if (inputHandler.ShiftingVector() == new Vector2(-1, -1) && !inputHandler.isHoldingTeleport())
            { 
                 hasTeleported = false; 
                Time.timeScale = 1;
                rb.velocity = new Vector2(-momentumWheel.momentum, -momentumWheel.momentum * 0.6f);

                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDiagonallyDown = true;
                gameManager.playerIsTeleporting = false;
            }
            //Down
            if (inputHandler.ShiftingVector() == new Vector2(0, -1) && !inputHandler.isHoldingTeleport())
            { 
                hasTeleported = false;
                Time.timeScale = 1;
                rb.velocity = new Vector2(0, -momentumWheel.momentum);
                
                playerInteractionScript.isHeadingDownwards = true;
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDown = true;
                gameManager.playerIsTeleporting = false;
            }
            //Down-Right
            if (inputHandler.ShiftingVector() == new Vector2(1, -1) && !inputHandler.isHoldingTeleport())
            { 
                hasTeleported = false;
                Time.timeScale = 1; 
                rb.velocity = new Vector2(momentumWheel.momentum, -momentumWheel.momentum * 0.6f);

                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDiagonallyDown = true;
                gameManager.playerIsTeleporting = false;
            }
        }
    }

    void OnResetLevel()
    {
        FindObjectOfType<GameManagerScript>().RestartLevel();
    }
    
    #region IEnumerators

    //Returns the Jump Buffer falls after a certain Time
    IEnumerator JumpBuffer()
    {
        yield return new WaitForSeconds(jumpBuffer);
        inputHandler.jumpBuffer = false;
    }

    IEnumerator DestroyShiftAnim()
    {
        yield return new WaitForSecondsRealtime(shiftingAnimDestruction);
        if (spawnedShiftingAnim != null)
        {
            Destroy(spawnedShiftingAnim);
        }
    }

    IEnumerator DestroyShiftAnim1()
    {
        yield return new WaitForSecondsRealtime(shiftingAnimDestruction);
        if (spawnedShiftingAnim1 != null)
        {
            Destroy(spawnedShiftingAnim1);
        }
    }

    #endregion
}
