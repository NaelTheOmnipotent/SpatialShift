using System;
using UnityEngine;

public class SpriteRendererScript : MonoBehaviour
{
    #region References

    private PlayerScript playerScript;
    private Animator animator;
    [SerializeField] private GroundedScript groundChecker;
    private InputHandlerScript inputHandler;
    private SpriteRenderer renderer;

    #endregion

    #region Squashing

    [SerializeField] private float squashAmount;
    private Vector2 originalScale;
    private Vector2 scale;
    private Vector2 additionalScale;

    #endregion

    #region Animation

    [SerializeField] private GameObject cape;
    [SerializeField] private Rigidbody2D rig;
    
    [SerializeField] private float rayCastDistance;
    [SerializeField] private LayerMask groundMask;
    
    [HideInInspector] public bool isBraking;
    [HideInInspector] public bool isShiftingDown;
    [HideInInspector] public bool isShiftingUp;
    [HideInInspector] public bool isShiftingDiagonallyUp;
    [HideInInspector] public bool isShiftingDiagonallyDown;
    [HideInInspector] public bool isShiftingSideways;
    [HideInInspector] public bool isShifting;
    [HideInInspector] public bool isJumping;
    
    private bool isFacingRight = true;
    private bool isGrounded;
    private Vector3 startingPos;

    #endregion

    private void Awake()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        renderer = GetComponent<SpriteRenderer>();
        inputHandler = GetComponentInParent<InputHandlerScript>();
        
        scale = originalScale = transform.localScale;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        CalculateScaleOnAirTime();
        
        ApplyScale();
        
        AnimationStuff();
        
        ShiftingBools();
    }

    void CalculateScaleOnAirTime()
    {
        // If isn't grounded, calculates how much the player is scaled on the x-axis and the additional y-axis
        if (!groundChecker.isGrounded && isFacingRight)
        {
            
            scale.x = originalScale.x - Mathf.Abs(playerScript.rb.velocity.y) / squashAmount;
            additionalScale.y = originalScale.x - scale.x;
        }
        else if (!groundChecker.isGrounded && !isFacingRight)
        {
            scale.x = originalScale.x + Mathf.Abs(playerScript.rb.velocity.y) / squashAmount;
            additionalScale.y = scale.x - originalScale.x;
        }
        else
        {
            scale = originalScale;
        }
    }
    
    private void ApplyScale()
    {
        //Applies the calculated Scale
        if (!groundChecker.isGrounded)
        {
            transform.localScale = new Vector3(scale.x, scale.y + additionalScale.y);
        }
        else
        {
            transform.localScale = originalScale;
        }
    }

    void AnimationStuff()
    {
        if (playerScript.hasTeleported)
        {
            //animator.enabled = false;
        }
        else
        {
            //animator.enabled = true;
        }
        
        if (inputHandler.MovementInput().x > 0 && !isFacingRight && Time.timeScale != 0)
        {
            FlipCharacter();
        }
        else if (inputHandler.MovementInput().x < 0 && isFacingRight && Time.timeScale != 0)
        {
            FlipCharacter();
        }

        if (!playerScript.hasTeleported)
        {
            animator.SetFloat("Speed", Mathf.Abs(playerScript.rb.velocity.x));
        }
        else
        {
            animator.SetFloat("Speed", Mathf.Abs(playerScript.trueVelocity));
        }

        animator.SetBool("Braking", isBraking);
        animator.SetBool("Jumping", isJumping);
        animator.SetFloat("YSpeed", playerScript.rb.velocity.y);
        animator.SetBool("Grounded", isGrounded);
        
        animator.SetBool("ShiftUp", isShiftingUp);
        animator.SetBool("ShiftDiagonallyUp", isShiftingDiagonallyUp);
        animator.SetBool("ShiftSideways", isShiftingSideways);
        animator.SetBool("ShiftDiagonallyDown", isShiftingDiagonallyDown);
        animator.SetBool("ShiftDown", isShiftingDown);
        animator.SetBool("Shifting", isShifting);
        animator.SetBool("Hit", GetComponentInParent<PlayerInteractionScript>().isInvincible);

        if (GetComponentInParent<PlayerInteractionScript>().isInvincible)
        {
            cape.SetActive(false);
        }
        else
        {
            cape.SetActive(true);
        }
        
        
    }

    void ShiftingBools()
    {
        if (groundChecker.isGrounded)
        {
            isShiftingDiagonallyDown = false;
            isShiftingDown = false;
            isShifting = false;
            isShiftingSideways = false;
            isShiftingUp = false;
            isShiftingDiagonallyUp = false;
        }
    }
    private void FlipCharacter()
    {
        Vector3 currentScale = transform.localScale;
        
        currentScale.x *= -1;
        originalScale.x *= -1;
        
        transform.localScale = currentScale;
        isFacingRight = !isFacingRight;
    }

    private void FixedUpdate()
    {
        RaycastHit2D leftGroundHit = Physics2D.Raycast(rig.transform.position + new Vector3(-0.25f, 0, 0), -Vector2.up, rayCastDistance, groundMask);
        RaycastHit2D rightGroundHit = Physics2D.Raycast(rig.transform.position + new Vector3(0.25f, 0, 0), -Vector2.up, rayCastDistance, groundMask);

        if (leftGroundHit.collider || rightGroundHit.collider)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
