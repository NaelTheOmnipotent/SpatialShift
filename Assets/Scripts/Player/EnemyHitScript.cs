using System;
using UnityEngine;

public class EnemyHitScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Collider2D>().enabled = false;
            Destroy(other.gameObject);
            
            GetComponentInParent<PlayerInteractionScript>().EnemyHit();
        }
    }
}
