using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [Header("Timer")]
    public static bool timerIsRunning;
    public static float currentTime;
    public static int minutes;
    
    [Header("Canvas")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject playerCanvas;
    [SerializeField] private GameObject tutorialCanvas;
    [SerializeField] private CanvasGroup blackScreen;
    
    [Header("SceneStart")]
    [SerializeField] private string[] sceneNamesLevel;
    [SerializeField] private Vector3 startOfLevelOne;
    public static Vector3 checkPointPosition;
    public static bool hasHitCheckpoint;
    
    [Header("Debug")]
    [Range(0, 500)][SerializeField] private int fps;
    [Range(0, 10)] [SerializeField] private float timeScale = 1;
    
    [Header("Misc")]
    [HideInInspector] public bool playerIsTeleporting;


    [Header("Global Volume")] 
    [SerializeField] private Volume globalVolume;

    private void Awake()
    {
        canvas.SetActive(true);

        globalVolume.weight = 0;

        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }
    }


    private void Update()
    {
        if (timerIsRunning)
        {
            Timer();
        }

        if (Application.targetFrameRate != fps)
        {
            Application.targetFrameRate = fps;
        }
    }


    private void Timer()
    {
        if (!playerIsTeleporting)
        {
            currentTime += Time.deltaTime;
        }
        else if (playerIsTeleporting)
        {
            currentTime += Time.unscaledDeltaTime;
        }
    }

    //Reloads the Level
    public void RestartLevel()
    {
        checkPointPosition = startOfLevelOne;
        hasHitCheckpoint = false;
        blackScreen.gameObject.GetComponent<BlackScreenScript>().isFadingOut = true;

        timerIsRunning = false;
        currentTime = 0;
        minutes = 0;
        
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    //Reloads from the last Checkpoint
    public void RestartFromCheckpoint()
    {
        
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        checkPointPosition = startOfLevelOne;
        hasHitCheckpoint = false;
        timerIsRunning = false;
        currentTime = 0;
        minutes = 0;
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }
        
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startOfLevelOne, .1f);
    }
}
