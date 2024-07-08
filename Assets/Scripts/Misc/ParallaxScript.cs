using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    //Variables
    private float startingPosX;
    private float startingPosY;
    private float length;
    private Vector2 cameraPos;

    //Set the parameters for the parallax
    [SerializeField] private Camera cam;
    [Range(0, 1)] [SerializeField] private float parallaxEffectStrengthX;
    [Range(0, 1)] [SerializeField] private float parallaxEffectStrengthY;

    private void Start()
    {
        //defines where the starting pos is + the size of the background
        startingPosX = transform.position.x;
        startingPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        //Lerps the Camera movement to fix stutteriness
        cameraPos = Vector2.Lerp(cameraPos, cam.transform.position, 25 * Time.deltaTime);
        
        //How much the layer moves
        float distanceX = cameraPos.x * parallaxEffectStrengthX;
        float distanceY = (cameraPos.y + 33) * parallaxEffectStrengthY;

        //The movement of the parralax
        float movement = cameraPos.x * (1 - parallaxEffectStrengthX);

        //Actually moves the calculated values
        transform.position = new Vector3(startingPosX + distanceX, startingPosY + distanceY, transform.position.z);

        //Teleports the background to the correct place if they are offscreen
        if (movement > startingPosX + length)
        {
            startingPosX += length;
        }
        else if (movement < startingPosX - length)
        {
            startingPosX -= length;
        }
    }
}