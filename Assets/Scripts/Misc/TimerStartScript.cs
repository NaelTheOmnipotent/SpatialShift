using UnityEngine;

public class TimerStartScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManagerScript.timerIsRunning = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }
}
