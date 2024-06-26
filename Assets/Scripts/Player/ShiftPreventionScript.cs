using System.Collections.Generic;
using UnityEngine;

public class ShiftPreventionScript : MonoBehaviour
{
    [SerializeField] List<Collider2D> colList = new();
    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + GetComponentInParent<PlayerScript>().shiftHeight);
        GetComponentInParent<PlayerScript>().isNotBlocked = true;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Adds the collider to a list
        if (other.CompareTag("Ground"))
        {
            if (!colList.Contains(other))
                colList.Add(other);

            if (colList.Count > 0)
                GetComponentInParent<PlayerScript>().isNotBlocked = false;
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
                GetComponentInParent<PlayerScript>().isNotBlocked = true;
        }
    }
}
