using System.Collections.Generic;
using UnityEngine;

public class ShiftPreventionScript : MonoBehaviour
{
    //References and Variables
    [SerializeField] List<Collider2D> colList = new();
    private PlayerScript playerScript;
    private void Start()
    {
        //Gets the PlayerScript and teleports the ShiftPrevention to the Shift Destination 
        playerScript = GetComponentInParent<PlayerScript>();
        transform.position = new Vector3(transform.position.x, transform.position.y + playerScript.shiftHeight);
        playerScript.isNotBlocked = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Adds the collider to a list
        if (other.CompareTag("Ground"))
        {
            if (!colList.Contains(other))
                colList.Add(other);

            if (colList.Count > 0)
                playerScript.isNotBlocked = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        //Removes the collider from the list
        if (other.CompareTag("Ground"))
        {
            if (colList.Contains(other))
                colList.Remove(other);

            if (colList.Count <= 0)
                playerScript.isNotBlocked = true;
        }
    }
}
