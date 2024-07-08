using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    //References
    private Rigidbody2D rb;
    private Animator animator;
    [HideInInspector] public bool playerHasJumped;
    [HideInInspector] public float jumpForce;

    [SerializeField] private GameObject jumpingDeathAnim;

    private void Awake()
    {
        //Gets the components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    
    private void FixedUpdate()
    {
        //Makes the Enemy Jump when the player has jumped
        if (playerHasJumped && rb.velocity.y == 0) 
        { 
            rb.velocity = Vector2.up * jumpForce;
            
            //Plays the enemyJump Sfx
            FindObjectOfType<AudioManager>().Play("EnemyJump");
        }

        //Sets animation stuff
        if (rb.velocity.y != 0)
        {
            animator.SetBool("Jumping", true);
        }
        else
        {
            animator.SetBool("Jumping", false);
        }
        animator.SetFloat("YSpeed", rb.velocity.y);
    }

    //turns the enemy
    public void FacingRight()
    {
        Vector3 currentScale = transform.localScale;

        currentScale.x = Mathf.Abs(currentScale.x);

        transform.localScale = currentScale;
    }

    //turns the enemy
    public void FacingLeft()
    {
        Vector3 currentScale = transform.localScale;

        currentScale.x = -Mathf.Abs(currentScale.x);

        transform.localScale = currentScale;
    }
    
    private void OnDestroy()
    {
        //Spawns death animation and rotates it the right way
        if (jumpingDeathAnim != null)
        {
            var spawnedDeathAnim = Instantiate(jumpingDeathAnim, transform.position, Quaternion.identity);
            if (transform.localScale.x > 0)
            {
                spawnedDeathAnim.GetComponent<SpriteRenderer>().flipX = true;
            }
        }
    }
}
