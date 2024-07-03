using UnityEngine;

public class ParallaxScript : MonoBehaviour
{
    private float startingPosX;
    private float startingPosY;
    private float length;

    [SerializeField] private Camera cam;
    [Range(0, 1)] [SerializeField] private float parallaxEffectStrengthX;
    [Range(0, 1)] [SerializeField] private float parallaxEffectStrengthY;

    private void Start()
    {
        startingPosX = transform.position.x;
        startingPosY = transform.position.y;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float distanceX = Time.time * 15 * parallaxEffectStrengthX;
        float distanceY = (cam.transform.position.y + 33) * parallaxEffectStrengthY;

        float movement = cam.transform.position.x * (1 - parallaxEffectStrengthX);

        transform.position = new Vector3(startingPosX + distanceX, startingPosY + distanceY, transform.position.z);

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