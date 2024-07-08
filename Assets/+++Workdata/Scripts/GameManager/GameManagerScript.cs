using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    //References/Variables
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
    private AudioManager audioManager;


    [Header("Global Volume")] 
    [SerializeField] private Volume globalVolume;

    private void Awake()
    {
        //Turns on the canvas (because I was lazy and didnt wanna turn it on and off again) and resets the globalVolume
        canvas.SetActive(true);
        globalVolume.weight = 0;

        //Resets the Gamepad Rumble just in case
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }

        //Gets the audioManager
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        //Plays the music
        audioManager.Play("Music");
    }

    private void Update()
    {
        //Updates the timer
        if (timerIsRunning)
        {
            Timer();
        }

        //Debugging with framerate
        if (Application.targetFrameRate != fps)
        {
            Application.targetFrameRate = fps;
        }
    }


    private void Timer()
    {
        //Adds scaled time
        if (!playerIsTeleporting)
        {
            currentTime += Time.deltaTime;
        }
        //If the player is shifting, adds unscaled time
        else if (playerIsTeleporting)
        {
            currentTime += Time.unscaledDeltaTime;
        }
    }

    //Reloads the Level
    public void RestartLevel()
    {
        //Resets variables
        checkPointPosition = startOfLevelOne;
        hasHitCheckpoint = false;

        AchievementManagerScript.enemiesKilled = 0;
        timerIsRunning = false;
        currentTime = 0;
        minutes = 0;
        
        //Resets the Gamepad Rumble just in case
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }
        
        //Reloads current Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    //Reloads from the last Checkpoint
    public void RestartFromCheckpoint()
    {
        //Resets the Gamepad Rumble just in case
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }
        
        //Reloads current Scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void BackToMainMenu()
    {
        //Resets variables
        checkPointPosition = startOfLevelOne;
        AchievementManagerScript.enemiesKilled = 0;
        hasHitCheckpoint = false;
        timerIsRunning = false;
        currentTime = 0;
        minutes = 0;
        
        audioManager.Stop("Music");
        //Resets the Gamepad Rumble just in case
        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0,0);
        }
        
        //Loads the MainMenu Scene
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    
    private void OnDrawGizmos()
    {
        //Shows where the start of the level is
        Gizmos.DrawWireSphere(startOfLevelOne, .1f);
    }
}
