using System.Collections.Generic;
using UnityEngine;

public class GroundedScript : MonoBehaviour
{
    //Variables
    [HideInInspector] public bool isGrounded;
    [SerializeField] List<Collider2D> colList = new();
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Adds the collider to a list
        if (other.CompareTag("Ground"))
        {
            if (!colList.Contains(other))
                colList.Add(other);

            if (colList.Count > 0)
                isGrounded = true;
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
                isGrounded = false;
        }
    }
}
