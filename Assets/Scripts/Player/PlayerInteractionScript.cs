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
    
    private Rigidbody2D rb;
    private PlayerScript playerScript;
    private GroundedScript groundedScript;
    private InputHandlerScript inputHandler;
    [SerializeField] private Renderer renderer;
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private Volume playerVolume;
    
    #endregion

    #region LayerMasks

    [Header("LayerMasks")] 
    public LayerMask groundMask;
    public LayerMask destructiblePlatformMask;
    public LayerMask fallThroughPlatformMask;
    public LayerMask enemyMask;

    #endregion
    
    [SerializeField] private float destructiblePlatformSpeedRequirement;
    [HideInInspector] public bool isHeadingDownwards;
    [HideInInspector] public int coinCount;
    private bool isHittingEnemy;
    private bool isColliding;
    private float tempJumpForce;

    #region Damage System

    [Header("Damage System")] 
    [SerializeField] private float invincibilityTime;
    [SerializeField] private float damagedTime;
    [SerializeField] private float invincibilityFlashIntervals;
    [SerializeField] private float reboundForce;
    [HideInInspector] public bool isInvincible;
    private bool isDamaged;
    private float ySpeed;
    private float tempInvincibilityFlashIntervals;

    #endregion
    

    [Header("Slope")]
    [Range(0, 5)] [SerializeField] float slopeJumpForce;
    [SerializeField] private float rotationSpeed;
    private float rotation;

    [Header("Misc")] 
    private float hardHitOnGroundSpeed;
    [SerializeField] private GameObject destructibleGroundAnim;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerScript = GetComponent<PlayerScript>();
        groundedScript = GetComponentInChildren<GroundedScript>();
        inputHandler = GetComponent<InputHandlerScript>();
    }

    private void Start()
    {
        tempInvincibilityFlashIntervals = invincibilityFlashIntervals;
        tempJumpForce = playerScript.jumpForce;
    }

    private void FixedUpdate()
    {
        Raycasts();

        if (Mathf.Abs(rb.velocity.y) > 1)
        {
            ySpeed = Mathf.Abs(rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            isHeadingDownwards = true;
        }
    }

    private void LateUpdate()
    {
        isHittingEnemy = false;
        isColliding = false;
    }

    void Raycasts()
    {
        //SlopeRaycast
        RaycastHit2D slopeHit = Physics2D.Raycast(transform.position, -Vector2.up, 1.5f, groundMask);
        if (slopeHit.collider)
        {
            //Smoothly Turns the player to face the same normal as the ground
            var surfaceNormal = slopeHit.normal;
            var angle = Vector3.Angle(Vector2.up, surfaceNormal);
            
            if (transform.up.y != surfaceNormal.y)
            {
                transform.localRotation = Quaternion.FromToRotation(Vector2.up, surfaceNormal);
                
                
                //transform.Rotate(0,0,angle * Time.deltaTime);                                                                                         BUG
                #region Not Working Code
                
                //transform.localRotation = Quaternion.FromToRotation(Vector2.up, Vector2.Lerp(transform.up, surfaceNormal, 10 * Time.fixedDeltaTime));
                
                /*
                //If the rotation is lower than the angle, increase the rotation
                if (rotation < angle)
                {
                    rotation += Time.deltaTime * rotationSpeed;
                }

                //If the rotation is larger than the angle, decrease the rotation
                if (rotation > angle)
                {
                    rotation -= Time.deltaTime * rotationSpeed;
                }

                //Depending on the slope normal it rotates the Rigidbody in the right direction
                if (Mathf.Sign(surfaceNormal.x) > 0 && rotation < angle)
                {
                    rb.rotation = -rotation;
                }
                else if (Mathf.Sign(surfaceNormal.x) < 0 && rotation < angle)
                {
                    rb.rotation = rotation;
                }

                
                //Rotates the Rigidbody if the ground is level depending in which direction the player is rotated
                if (angle == 0 && rb.rotation < angle)
                {
                    rb.rotation = -rotation;
                }
                if (angle == 0 && rb.rotation > angle)
                {
                    rb.rotation = rotation;
                }


                //Making sure the player is actually rotated perfectly upright
                if (angle == 0 && Mathf.Abs(rb.rotation) < 1f)
                {
                    rb.rotation = 0;
                    rotation = 0;
                }
                */
                
                #endregion
            }

            //Adds more jumpForce the faster and steeper the angle is
            if (surfaceNormal != Vector2.up && Mathf.Abs(surfaceNormal.x) * rb.velocity.magnitude * slopeJumpForce > tempJumpForce)
            {
                playerScript.jumpForce = Mathf.Abs(surfaceNormal.x) * rb.velocity.magnitude * slopeJumpForce;
                Debug.Log(playerScript.jumpForce);
            }
            else
            {
                playerScript.jumpForce = tempJumpForce;
            }
        }
        else
        {
            transform.localRotation = Quaternion.identity;
        }
        
        //DestructiblePlatform
        var destructiblePlatformHitRange = Mathf.Abs(rb.velocity.y / 20) + 1.5f;
        
        RaycastHit2D destructiblePlatformHit = Physics2D.Raycast(transform.position, -Vector2.up, destructiblePlatformHitRange, destructiblePlatformMask);
        if (destructiblePlatformHit.collider)
        {
            if (isHeadingDownwards && destructiblePlatformSpeedRequirement <= Mathf.Abs(rb.velocity.y))
            {
                var destructiblePlatformDestructionAnim = Instantiate(destructibleGroundAnim, destructiblePlatformHit.collider.gameObject.transform.position, Quaternion.identity);
                Destroy(destructiblePlatformHit.collider.gameObject);
                Destroy(destructiblePlatformDestructionAnim, .5f);
                playerScript.touchedGround = true;

                if (Gamepad.current != null)
                {
                    Gamepad.current.SetMotorSpeeds(.5f, .5f);
                    StartCoroutine(GamePadVibration());
                }
            }
        }
        

        //FallThrough-Platform
        var fallThroughPlatformHitRange = Mathf.Abs(rb.velocity.y / 20) + 1;
        
        RaycastHit2D fallThroughPlatformHit = Physics2D.Raycast(groundedScript.transform.position, -Vector2.up, fallThroughPlatformHitRange, fallThroughPlatformMask);
        if (fallThroughPlatformHit.collider)
        {
            if (inputHandler.MovementInput().y < -.7)
            {
                fallThroughPlatformHit.collider.gameObject.GetComponent<FallThroughPlatformsScript>().canFallThrough = true;
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            if (isColliding)
            {
                return;
            }
            isColliding = true;
            
            Destroy(other.gameObject);
            coinCount++;
        }
        
        else if (other.CompareTag("Checkpoint") && GameManagerScript.checkPointPosition != other.transform.position)
        {
            GameManagerScript.checkPointPosition = other.transform.position;
            if (rb.velocity.magnitude > 20)
            {
                other.GetComponent<Animator>().Play("SignAnim");
            }
        }
        else if (other.CompareTag("DeathPlane"))
        {
            gameManager.RestartFromCheckpoint();
        }
    }

    #region EnemyInteraction

    //gets called whenever the Player gets damage
    void Damage()
    {
        if (!isInvincible && !isDamaged)
        {
            isInvincible = true;
            StartCoroutine(InvincibilityTimer());
            StartCoroutine(InvincibilityFlashing());

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

        if (!isInvincible && isDamaged)
        {
            gameManager.RestartFromCheckpoint();
        }
    }

    public void EnemyHit()
    {
        rb.velocity = new Vector2(rb.velocity.x, ySpeed * 1.35f);
        isHittingEnemy = true;
        isHeadingDownwards = false;
        AchievementManagerScript.enemiesKilled++;
        
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(.075f, .075f);
            StartCoroutine(GamePadVibration());
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
       
        if (other.gameObject.CompareTag("Enemy") && !isHittingEnemy)
        {
            Damage();
            StartCoroutine(PlayerVolumeUp());
            
            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(.4f, .4f);
                StartCoroutine(GamePadVibration());
            }
        }
        else if (other.gameObject.CompareTag("DangerZone"))
        {
            Damage();
            groundedScript.isGrounded = true;
            StartCoroutine(PlayerVolumeUp());

            if (Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(.4f, .4f);
                StartCoroutine(GamePadVibration());
            }
        }
    }

    IEnumerator InvincibilityTimer()
    {
        yield return new WaitForSecondsRealtime(invincibilityTime / 2);
        playerScript.isHit = false;
        
        yield return new WaitForSecondsRealtime(invincibilityTime / 2);
        
        isInvincible = false;
        isDamaged = true;

        StartCoroutine(DamagedTimer());
    }

    IEnumerator InvincibilityFlashing()
    {
        renderer.enabled = false;
        yield return new WaitForSecondsRealtime(invincibilityFlashIntervals);
        invincibilityFlashIntervals -= .05f;
        
        renderer.enabled = true;
        yield return new WaitForSecondsRealtime(invincibilityFlashIntervals);
        
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
        yield return new WaitForSeconds(damagedTime);
        isDamaged = false;
    }

    IEnumerator PlayerVolumeUp()
    {
        while (playerVolume.weight < 0.99f)
        {
            playerVolume.weight = Mathf.Lerp(playerVolume.weight, 1, 10 * Time.deltaTime);
            yield return null;
        }

        playerVolume.weight = 1;
        yield return new WaitForSeconds(.5f);
        
        StartCoroutine(PlayerVolumeDown());
    }
    
    IEnumerator PlayerVolumeDown()
    {
        
        while (playerVolume.weight > 0.81f)
        { 
            playerVolume.weight = Mathf.Lerp(playerVolume.weight, .8f, 5 * Time.deltaTime);
            yield return null;
        }
        
        playerVolume.weight = .8f;
           
        if(isDamaged)
        {
            yield return new WaitForSeconds(.5f); 
            StartCoroutine(PlayerVolumeUp());
        }
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
        yield return new WaitForSecondsRealtime(.25f);
        Gamepad.current.SetMotorSpeeds(0,0);
    }
}
















/*

//If the rotation is lower than the angle, increase the rotation
if (rotation < angle)
{
    rotation += Time.deltaTime * rotationSpeed;
}

//If the rotation is larger than the angle, decrease the rotation
if (rotation > angle)
{
    rotation -= Time.deltaTime * rotationSpeed;
}

//Depending on the slope normal it rotates the Rigidbody in the right direction
if (Mathf.Sign(surfaceNormal.x) > 0 && rotation < angle)
{
    rb.rotation = -rotation;
}
else if (Mathf.Sign(surfaceNormal.x) < 0 && rotation < angle)
{
    rb.rotation = rotation;
}




//Rotates the Rigidbody if the ground is level depending in which direction the player is rotated
if (angle == 0 && rb.rotation < angle)
{
    rb.rotation = -rotation;
}
if (angle == 0 && rb.rotation > angle)
{
    rb.rotation = rotation;
}


//Making sure the player is actually rotated perfectly upright
if (angle == 0 && Mathf.Abs(rb.rotation) < 1f)
{
    rb.rotation = 0;
    rotation = 0;
}
*/