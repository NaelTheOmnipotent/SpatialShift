using UnityEngine;

public class PlayerCanvasScript : MonoBehaviour
{
    //References
    [SerializeField] private CanvasGroup shiftingCanvasGroup;
    private PlayerScript playerScript;

    private void Start()
    {
        //Get Components and Hide CanvasGroup
        playerScript = GetComponentInParent<PlayerScript>();
        shiftingCanvasGroup.HideCanvasGroup();
    }

    private void Update()
    {
        if (playerScript.hasTeleported)
        {
            //Shows ShiftingCanvas when the player is Shifting
            shiftingCanvasGroup.ShowCanvasGroup();
        }
        else
        {
            //Hides ShiftingCanvas when the player isn't Shifting
            shiftingCanvasGroup.HideCanvasGroup();
        }
    }
}
