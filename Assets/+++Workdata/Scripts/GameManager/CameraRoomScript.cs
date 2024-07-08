using UnityEngine;

public class CameraRoomScript : MonoBehaviour
{
    [SerializeField] private GameObject virtualCam;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //If the player enters the trigger, activate the camera
            virtualCam.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //If the Player exits the trigger, deactivate the camera
            virtualCam.SetActive(false);
        }
    }
}
