using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class PlayerScript : MonoBehaviour
{
    
    #region Movement Variable Declarations

    //General References to Scripts and RigidBody
    [Header("References")]
    [HideInInspector] public Rigidbody2D rb;
    GroundedScript groundedScript;
    InputHandlerScript inputHandler;
    PlayerInteractionScript playerInteractionScript;

    //Variables to adjust the Gravity And Jump Forces
    [Header("Jumping/Gravity")]
    public float jumpForce;
    [SerializeField] private float jumpBuffer;
    [SerializeField] private float coyoteTime;
    private float coyoteTimeCounter;

    [SerializeField] private float gravity;
    [SerializeField] private float afterApexGravity;
    [SerializeField] private float variableJumpForce;

    [HideInInspector] public bool hasJumped;

    //Variables to adjust movement on Ground
    [Header("Movement On Ground")]
    [SerializeField] private float groundSpeed;
    [SerializeField] private float maxSpeedOnGround;
    [SerializeField] private float decelerationOnGround;
    [SerializeField] private float turningSpeedOnGround;
    [SerializeField] private float resetGroundSpeed;

    //Variables to adjust movement in Air
    [Header("Movement In Air")]
    [SerializeField] private float airSpeed;
    [SerializeField] private float maxSpeedInAir;
    [SerializeField] private float decelerationInAir;
    [SerializeField] private float turningSpeedInAir;
    [SerializeField] private float resetAirSpeed;

    #endregion

    #region ShiftingMechanic Variable Declerations

    //Variables for the shifting mechanic
    [Header("Shifting")] 
    public float shiftHeight;
    [HideInInspector] public bool touchedGround;
    [HideInInspector] public float trueVelocity;
    [HideInInspector] public bool hasTeleported;
    [HideInInspector] public bool isNotBlocked;
    [SerializeField] private float shiftCountDownInSeconds;
    private float shiftCountDown;

    // Animations for shifting
    [Space(10)]
    [SerializeField] private GameObject shiftingAnim;
    [SerializeField] private GameObject shiftingAnim1;
    [SerializeField] private float shiftingAnimDestruction;
    private GameObject spawnedShiftingAnim;
    private GameObject spawnedShiftingAnim1;
    
    #endregion

    //Miscellaneous references and variables that dont really fit anywhere else 
    [Header("Misc")] 
    [SerializeField] MomentumWheelScript momentumWheel;
    [SerializeField] private SpriteRendererScript spriteRendererScript;
    [SerializeField] private GameManagerScript gameManager;
    [HideInInspector] public bool isHit;
    private AudioManager audioManager;
    private bool isPlayingFootSteps;
    
    private void Awake()
    {
        //Puts the Player at either the start of the level or the last Checkpoint
        transform.position = GameManagerScript.checkPointPosition;
    }

    private void Start()
    {
        //Gets the corresponding Components
        audioManager = FindObjectOfType<AudioManager>();
        groundedScript = GetComponentInChildren<GroundedScript>();
        inputHandler = GetComponent<InputHandlerScript>();
        rb = GetComponent<Rigidbody2D>();
        playerInteractionScript = GetComponent<PlayerInteractionScript>();
    }

    private void Update()
    {
        //Calls the different Methods responsible for different functions
        AudioManager();
        
        VariablesReset();

        Shifting();
    }

    private void FixedUpdate()
    {
        //Calls the MovePlayer Method
        MovePlayer();
    }

    void MovePlayer()
    {
        #region Jumping
        
        //Jumping if the Jump Button is being held or the player is grounded/coyote Timed
        if ((inputHandler.JumpInput() || inputHandler.jumpBuffer) && (coyoteTimeCounter > 0))
        {
            //Adds an upwards force perpendicular to the ground normal
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            
            //Setting Variables
            hasJumped = true;
            inputHandler.jumpBuffer = false;
            coyoteTimeCounter = 0;
            groundedScript.isGrounded = false;
            spriteRendererScript.isJumping = true;
            
            //Play The Jump sound
            audioManager.Play("Jump");
        }

        //Adds an additional downwards force when the jump button is released
        if (hasJumped && rb.velocity.y > 0 && (!inputHandler.JumpInput() && !inputHandler.jumpHeld))
        {
            rb.AddForce(-Vector2.up * variableJumpForce);
        }


        #endregion

        #region Gravity

        //Normal Gravity (I like to use this instead of the rigidbodies gravityScale)
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
                //Resets a bool for the spriteRenderer
                spriteRendererScript.isBraking = false;
            }
            
            //Extra Deceleration when pressing in the opposite way of travel
            if (rb.velocity.x < -resetGroundSpeed && inputHandler.MovementInput().x > 0)
            {
                spriteRendererScript.isBraking = true;
                rb.AddForce(Vector2.right * turningSpeedOnGround);
            }
            else
            {
                //Resets a bool for the spriteRenderer
                spriteRendererScript.isBraking = false;
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
        //Resets a bool for the spriteRenderer
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
                audioManager.Play("Shifting");

                //Spawns a Shifting Animation that gets played and afterwords deleted
                spawnedShiftingAnim = Instantiate(shiftingAnim, transform.position, quaternion.identity);
                StartCoroutine(DestroyShiftAnim());
                
                //Stops Time and teleports the Player up
                Time.timeScale = 0.0f;
                transform.position = new Vector3(transform.position.x, transform.position.y + shiftHeight);
                
                //Spawns another Shifting Animation that gets played and afterwords deleted
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

                //Adds a cooldown to the ShiftAbility
                shiftCountDown = Time.fixedTime + shiftCountDownInSeconds;
            }
        }
        //When the Player has Shifted, they can select which direction the want to Shift in
        if (hasTeleported)
        {
            //Right
            if (inputHandler.ShiftingVector() == new Vector2(1, 0) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false; 
                Time.timeScale = 1; 
                rb.velocity = new Vector2(momentumWheel.momentum, 0);

                //Sets some variables for the SpriteRenderer
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingSideways = true;
                gameManager.playerIsTeleporting = false;
                
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
            //Up-Right
            if (inputHandler.ShiftingVector() == new Vector2(1, 1) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false;
                Time.timeScale = 1;
                rb.velocity = new Vector2(momentumWheel.momentum * 0.71f, momentumWheel.momentum * 1f);

                //Sets some variables for the SpriteRenderer
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDiagonallyUp = true;
                gameManager.playerIsTeleporting = false;
                
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
            //Up
            if (inputHandler.ShiftingVector() == new Vector2(0, 1) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false; 
                Time.timeScale = 1;
                rb.velocity = new Vector2(0, momentumWheel.momentum * 1.5f);

                //Sets some variables for the SpriteRenderer
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingUp = true;
                gameManager.playerIsTeleporting = false;
                
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
            //Up-Left
            if (inputHandler.ShiftingVector() == new Vector2(-1, 1) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false; 
                Time.timeScale = 1; 
                rb.velocity = new Vector2(-momentumWheel.momentum * 0.71f, momentumWheel.momentum * 1f);
                
                //Sets some variables for the SpriteRenderer
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDiagonallyUp = true;
                gameManager.playerIsTeleporting = false;
                 
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
            //Left
            if (inputHandler.ShiftingVector() == new Vector2(-1, 0) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false; 
                Time.timeScale = 1; 
                rb.velocity = new Vector2(-momentumWheel.momentum, 0);
                
                //Sets some variables for the SpriteRenderer
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingSideways = true;
                gameManager.playerIsTeleporting = false;
                
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
            //Down-Left
            if (inputHandler.ShiftingVector() == new Vector2(-1, -1) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false; 
                Time.timeScale = 1;
                rb.velocity = new Vector2(-momentumWheel.momentum, -momentumWheel.momentum * 0.6f);

                //Sets some variables for the SpriteRenderer
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDiagonallyDown = true;
                gameManager.playerIsTeleporting = false;
                
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
            //Down
            if (inputHandler.ShiftingVector() == new Vector2(0, -1) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false;
                Time.timeScale = 1;
                rb.velocity = new Vector2(0, -momentumWheel.momentum);
                
                //Sets some variables for the SpriteRenderer
                playerInteractionScript.isHeadingDownwards = true;
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDown = true;
                gameManager.playerIsTeleporting = false;
                
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
            //Down-Right
            if (inputHandler.ShiftingVector() == new Vector2(1, -1) && !inputHandler.isHoldingTeleport())
            { 
                //Resets variables and adds velocity to the rigidbody in the direction the player held
                hasTeleported = false;
                Time.timeScale = 1; 
                rb.velocity = new Vector2(momentumWheel.momentum, -momentumWheel.momentum * 0.6f);

                //Sets some variables for the SpriteRenderer
                spriteRendererScript.isShifting = false;
                spriteRendererScript.isShiftingDiagonallyDown = true;
                gameManager.playerIsTeleporting = false;
                
                //Plays a soundEffect
                audioManager.Play("ShiftExit");
            }
        }
    }

    void OnResetLevel()
    {
        //Restarts the level
        FindObjectOfType<GameManagerScript>().RestartLevel();
    }

    void AudioManager()
    {
        //If the player has Shifted downwards and touches ground the Stomp soundEffect is played
        if (playerInteractionScript.isHeadingDownwards && groundedScript.isGrounded)
        {
            audioManager.Play("Stomp");
        }

        //If the player is walking the footstep sound is played on loop
        if (groundedScript.isGrounded && Mathf.Abs(rb.velocity.x) > 5 && !isPlayingFootSteps)
        {
            audioManager.Play("WalkOnGrass");
            isPlayingFootSteps = true;
        }
        else if (!groundedScript.isGrounded || Mathf.Abs(rb.velocity.x) < 5)
        {
            audioManager.Stop("WalkOnGrass");
            isPlayingFootSteps = false;
        }
        
        
    }
    
    #region IEnumerators

    //Returns the Jump Buffer false after a certain Time
    IEnumerator JumpBuffer()
    {
        yield return new WaitForSeconds(jumpBuffer);
        inputHandler.jumpBuffer = false;
    }

    //Destroys the Spawned ShiftingAnim
    IEnumerator DestroyShiftAnim()
    {
        yield return new WaitForSecondsRealtime(shiftingAnimDestruction);
        if (spawnedShiftingAnim != null)
        {
            Destroy(spawnedShiftingAnim);
        }
    }

    //Destroys the Spawned ShiftingAnim
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
