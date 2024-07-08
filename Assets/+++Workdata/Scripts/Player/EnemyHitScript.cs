using System;
using UnityEngine;

public class EnemyHitScript : MonoBehaviour
{
    //Reference
    private Rigidbody2D rb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            //Disables the enemies collider so that the player doesnt accidentally get damaged
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            //Destroys Enemy
            Destroy(other.gameObject);
            
            //Calls EnemyHitMethod
            GetComponentInParent<PlayerInteractionScript>().EnemyHit();
        }
    }
}
