using UnityEngine;

public class TimerStartScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        GameManagerScript.timerIsRunning = true;
        Debug.Log("Timer Started");
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position,transform.localScale);
    }
}
