using System;
using System.Collections;
using System.Timers;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerInteractionScript : MonoBehaviour
{
    
    #region References
    
    //General References
    private Rigidbody2D rb;
    private PlayerScript playerScript;
    private GroundedScript groundedScript;
    private InputHandlerScript inputHandler;
    [SerializeField] private Renderer renderer;
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private Volume playerVolume;
    
    #endregion

    #region LayerMasks

    //Different LayerMasks for Raycasts
    [Header("LayerMasks")] 
    public LayerMask groundMask;
    public LayerMask destructiblePlatformMask;
    public LayerMask fallThroughPlatformMask;
    public LayerMask enemyMask;

    #endregion
    
    //Destructible Ground variables
    [SerializeField] private float destructiblePlatformSpeedRequirement;
    [HideInInspector] public bool isHeadingDownwards;
    
    

    #region Damage System

    //Variables for the damage system
    [Header("Damage System")] 
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float damagedTime;
    [SerializeField] private float invincibilityFlashIntervals;
    [SerializeField] private float reboundForce;
    [HideInInspector] public bool isInvincible;
    private bool isHittingEnemy;
    private bool isColliding;
    private bool isDamaged;
    private float ySpeed;
    private float tempInvincibilityFlashIntervals;

    #endregion
    
    //Variables for the slopes
    [Header("Slope")]
    [Range(0, 5)] [SerializeField] float slopeJumpForce;
    private float tempJumpForce;
    private float rotation;

    //Miscellaneous variables and References
    [Header("Misc")] 
    private float hardHitOnGroundSpeed;
    [SerializeField] private GameObject destructibleGroundAnim;
    private AudioManager audioManager;
    
    
    private void Awake()
    {
        //Gets the corresponding Components
        rb = GetComponent<Rigidbody2D>();
        playerScript = GetComponent<PlayerScript>();
        groundedScript = GetComponentInChildren<GroundedScript>();
        inputHandler = GetComponent<InputHandlerScript>();
    }

    private void Start()
    {
        //Gets the corresponding Components
        audioManager = FindObjectOfType<AudioManager>();
        
        //assign temporary variables that can be reset to when the corresponding variables get changed
        tempInvincibilityFlashIntervals = invincibilityFlashIntervals;
        tempJumpForce = playerScript.jumpForce;
    }

    private void FixedUpdate()
    {
        //Calls Raycast Method
        Raycasts();

        //If the player is moving vertically update the ySpeed variable
        if (Mathf.Abs(rb.velocity.y) > 1)
        {
            ySpeed = Mathf.Abs(rb.velocity.y);
        }
    }

    private void LateUpdate()
    {
        //Resets variables responding for the damage system => is in late update so the damage calculation is sure to be finished till then
        isHittingEnemy = false;
        isColliding = false;
    }

    void Raycasts()
    {
        //SlopeRaycast
        #region Slopes

        //Raycast
        RaycastHit2D slopeHit = Physics2D.Raycast(transform.position, -Vector2.up, 1.5f, groundMask);
        if (slopeHit.collider)
        {
            //Applies the slopeHits Normal to a local variable
            var surfaceNormal = slopeHit.normal;
            
            //Rotates the player perpendicular to the surfaces' Normal 
            if (transform.up.y != surfaceNormal.y)
            {
                transform.localRotation = Quaternion.FromToRotation(Vector2.up, surfaceNormal);
            }

            //Adds more jumpForce the faster and steeper the angle is
            if (surfaceNormal != Vector2.up && Mathf.Abs(surfaceNormal.x) * rb.velocity.magnitude * slopeJumpForce > tempJumpForce)
            {
                playerScript.jumpForce = Mathf.Abs(surfaceNormal.x) * rb.velocity.magnitude * slopeJumpForce;
                Debug.Log(playerScript.jumpForce);
            }
            else
            {
                //Resets the jumpForce
                playerScript.jumpForce = tempJumpForce;
            }
        }
        else
        {
            //Resets the Players Rotation if not on slope
            transform.localRotation = Quaternion.identity;
        }
        #endregion
        
        //DestructiblePlatform
        #region DestructiblePlatform

        //The faster the player falls the longer the raycast so that it is sure to hit before the player will collide
        var destructiblePlatformHitRange = Mathf.Abs(rb.velocity.y / 20) + 1.5f;
        
        //Raycast
        RaycastHit2D destructiblePlatformHit = Physics2D.Raycast(transform.position, -Vector2.up, destructiblePlatformHitRange, destructiblePlatformMask);
        if (destructiblePlatformHit.collider)
        {
            //If the Player Shifted Downwards and is falling fast enough
            if (isHeadingDownwards && destructiblePlatformSpeedRequirement <= Mathf.Abs(rb.velocity.y))
            {
                //Spawns an animation with the same transform as the destructible platform, then destroys the original
                var destructiblePlatformDestructionAnim = Instantiate(destructibleGroundAnim, destructiblePlatformHit.collider.gameObject.transform.position, Quaternion.identity);
                Destroy(destructiblePlatformHit.collider.gameObject);
                Destroy(destructiblePlatformDestructionAnim, .5f);
                
                //Turns this variable true so you can shift again after destroying a platform
                playerScript.touchedGround = true;
                
                //If A Gamepad is connected, use rumble
                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(.5f, .5f);
                    StartCoroutine(GamePadVibration());
                }
                
                //Plays a soundEffect
                audioManager.Play("BreakThroughPlatform");
            }
        }

        #endregion
        
        //FallThrough-Platform
        #region SemiSolidPlatforms

        //The faster the player falls the longer the raycast so that it is sure to hit before the player will collide
        var fallThroughPlatformHitRange = Mathf.Abs(rb.velocity.y / 20) + 1;
        
        //Rayacast
        RaycastHit2D fallThroughPlatformHit = Physics2D.Raycast(groundedScript.transform.position, -Vector2.up, fallThroughPlatformHitRange, fallThroughPlatformMask);
        if (fallThroughPlatformHit.collider)
        {
            //If the player is holding downwards/mostly downwards it disables the Semi-Solid Platforms collider
            if (inputHandler.MovementInput().y < -.7)
            {
                fallThroughPlatformHit.collider.gameObject.GetComponent<FallThroughPlatformsScript>().canFallThrough = true;
            }
        }
        
        #endregion
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //If the player Enters a checkpoint that is not the currently active checkpoint
        if (other.CompareTag("Checkpoint") && GameManagerScript.checkPointPosition != other.transform.position)
        {
            //Updates the checkpointPos
            GameManagerScript.checkPointPosition = other.transform.position;
            
            //If the playerSpeed is over 20 it plays a fast Spinning Animation
            if (rb.velocity.magnitude >= 20)
            {
                other.GetComponent<Animator>().Play("SignAnim");
                audioManager.Play("Checkpoint");
            }
            else if (rb.velocity.magnitude < 20)
            {
                other.GetComponent<Animator>().Play("SignSlowAnim");
                audioManager.Play("Checkpoint");
            }
        }
        //Kills the Player
        else if (other.CompareTag("DeathPlane"))
        {
            gameManager.RestartFromCheckpoint();
        }
    }

    #region EnemyInteraction

    //gets called whenever the Player gets damage
    void Damage()
    {
        //When the player gets hit for the first time
        if (!isInvincible && !isDamaged)
        {
            audioManager.Play("Hit");
            //Turns the Player Invincible
            isInvincible = true;
            StartCoroutine(InvincibilityTimer());
            StartCoroutine(InvincibilityFlashing());

            //Flings the character back a bit
            if (Mathf.Sign(rb.velocity.x) > 0)
            {
                rb.velocity = new Vector2(-1, 2) * reboundForce;
                playerScript.isHit = true;
            }
            else if (Mathf.Sign(rb.velocity.x) < 0)
            {
                rb.velocity = new Vector2(1, 2) * reboundForce;
                playerScript.isHit = true;
            }
        }
        //If the player is already damaged and gets hit again, restart
        if (!isInvincible && isDamaged)
        {
            gameManager.RestartFromCheckpoint();
        }
    }

    public void EnemyHit()
    {
        //If the player manages to land on an enemy, rebound the player 
        rb.velocity = new Vector2(rb.velocity.x, ySpeed * 1.35f);
        isHittingEnemy = true;
        isHeadingDownwards = false;
        
        //Adds a killed enemy for the achievement
        AchievementManagerScript.enemiesKilled++;
        audioManager.Play("EnemyDeath");
        
        //If A Gamepad is connected, use rumble
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(.075f, .075f);
            StartCoroutine(GamePadVibration());
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        //If the player is colliding with an enemy and is not landing on it
        if (other.gameObject.CompareTag("Enemy") && !isHittingEnemy)
        {
            //Damage the Player
            Damage();
            
            //Raise the PlayerVolume for an added effect
            StartCoroutine(PlayerVolumeUp());
            
            //If A Gamepad is connected, use rumble
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(.4f, .4f);
                StartCoroutine(GamePadVibration());
            }
        }
        //If the player is something like a spike
        else if (other.gameObject.CompareTag("DangerZone"))
        {
            //Damage the Player
            Damage();
            
            //Turns is Grounded true so the player can jump in case they are stuck on spikes
            groundedScript.isGrounded = true;
            
            //Raise the PlayerVolume for an added effect
            StartCoroutine(PlayerVolumeUp());

            //If A Gamepad is connected, use rumble
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(.4f, .4f);
                StartCoroutine(GamePadVibration());
            }
        }
    }

    IEnumerator InvincibilityTimer()
    {
        //For how long the player is is Invincible
        yield return new WaitForSecondsRealtime(invincibilityTime / 2);
        playerScript.isHit = false;
        
        yield return new WaitForSecondsRealtime(invincibilityTime / 2);
        
        isInvincible = false;
        isDamaged = true;

        //After being Invincible the Player is Damaged
        StartCoroutine(DamagedTimer());
    }

    IEnumerator InvincibilityFlashing()
    {
        //Disables and ReEnables the renderer, making the player flash
        renderer.enabled = false;
        yield return new WaitForSecondsRealtime(invincibilityFlashIntervals);
        invincibilityFlashIntervals -= .05f;
        
        renderer.enabled = true;
        yield return new WaitForSecondsRealtime(invincibilityFlashIntervals);
        
        //if the player is still invincible, keep flashing, else stop it
        if (isInvincible)
        {
            StartCoroutine(InvincibilityFlashing());
        }
        else
        {
            invincibilityFlashIntervals = tempInvincibilityFlashIntervals;
        }
    }

    IEnumerator DamagedTimer()
    {
        //For how long the player is damaged
        yield return new WaitForSeconds(damagedTime);
        isDamaged = false;
    }

    IEnumerator PlayerVolumeUp()
    {
        //Turns up the playerVolume
        while (playerVolume.weight < 0.99f)
        {
            playerVolume.weight = Mathf.Lerp(playerVolume.weight, 1, 10 * Time.deltaTime);
            yield return null;
        }

        //sets it to 1 and makes it go back down
        playerVolume.weight = 1;
        yield return new WaitForSeconds(.5f);
        
        StartCoroutine(PlayerVolumeDown());
    }
    
    IEnumerator PlayerVolumeDown()
    {
        //Turns down the playerVolume
        while (playerVolume.weight > 0.81f)
        { 
            playerVolume.weight = Mathf.Lerp(playerVolume.weight, .8f, 5 * Time.deltaTime);
            yield return null;
        }
        
        //sets it to .8 and makes it go back up if the player is still damaged
        playerVolume.weight = .8f;
        if(isDamaged)
        {
            yield return new WaitForSeconds(.5f); 
            StartCoroutine(PlayerVolumeUp());
        }
        //Resets the playerVolume to 0
        else
        {
            while (playerVolume.weight > 0.01f)
            {
                playerVolume.weight = Mathf.Lerp(playerVolume.weight, 0, 5 * Time.deltaTime);
                yield return null;
            }
            playerVolume.weight = 0;
        }
    }

    #endregion

    IEnumerator GamePadVibration()
    {
        //Turns of the Rumble
        yield return new WaitForSecondsRealtime(.25f);
        Gamepad.current.SetMotorSpeeds(0,0);
    }
}