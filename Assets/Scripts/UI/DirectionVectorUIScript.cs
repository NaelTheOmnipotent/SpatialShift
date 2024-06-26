using UnityEngine;
using UnityEngine.UI;

public class DirectionVectorUIScript : MonoBehaviour
{
    private PlayerScript playerScript;
    private InputHandlerScript inputHandler;

    [SerializeField] Image directionUp;
    [SerializeField] Image directionDown;
    [SerializeField] Image directionLeft;
    [SerializeField] Image directionRight;
    [SerializeField] Image directionUpRight;
    [SerializeField] Image directionUpLeft;
    [SerializeField] Image directionDownLeft;
    [SerializeField] Image directionDownRight;
    
    
    // Start is called before the first frame update
    void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
        inputHandler = GetComponentInParent<InputHandlerScript>();
        
        directionDown.enabled = false;
        directionLeft.enabled = false;
        directionRight.enabled = false;
        directionUp.enabled = false;
        directionDownLeft.enabled = false;
        directionDownRight.enabled = false;
        directionUpLeft.enabled = false;
        directionUpRight.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScript.hasTeleported)
        {
            //Right
            if (inputHandler.ShiftingVector() == new Vector2(1, 0))
            {
                directionRight.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(1, 0))
            {
                directionRight.enabled = false;
            }
            
            //Up
            if (inputHandler.ShiftingVector() == new Vector2(0, 1))
            {
                directionUp.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(0, 1))
            {
                directionUp.enabled = false;
            }
            
            //Left
            if (inputHandler.ShiftingVector() == new Vector2(-1, 0))
            {
                directionLeft.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(-1, 0))
            {
                directionLeft.enabled = false;
            }
            
            //Down
            if (inputHandler.ShiftingVector() == new Vector2(0, -1))
            {
                directionDown.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(0, -1))
            {
                directionDown.enabled = false;
            }
            
            //DownRight
            if (inputHandler.ShiftingVector() == new Vector2(1, -1))
            {
                directionDownRight.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(1, -1))
            {
                directionDownRight.enabled = false;
            }
            
            //UpRight
            if (inputHandler.ShiftingVector() == new Vector2(1, 1))
            {
                directionUpRight.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(1, 1))
            {
                directionUpRight.enabled = false;
            }
            
            //UpLeft
            if (inputHandler.ShiftingVector() == new Vector2(-1, 1))
            {
                directionUpLeft.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(-1, 1))
            {
                directionUpLeft.enabled = false;
            }
            
            //DownLeft
            if (inputHandler.ShiftingVector() == new Vector2(-1, -1))
            {
                directionDownLeft.enabled = true;
            }
            else if (inputHandler.ShiftingVector() != new Vector2(-1, -1))
            {
                directionDownLeft.enabled = false;
            }
        }
    }
}
