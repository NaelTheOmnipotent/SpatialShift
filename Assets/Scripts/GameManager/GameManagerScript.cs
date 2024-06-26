using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    [Header("Timer")]
    public static bool timerIsRunning;
    public static float currentTime;
    
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
    [SerializeField] private int fps;
    [Range(0, 10)] [SerializeField]private float timeScale = 1;
    
    [Header("Misc")]
    [HideInInspector] public bool playerIsTeleporting;

    private void Awake()
    {
        canvas.SetActive(true);
        
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
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    //Reloads from the last Checkpoint
    public void RestartFromCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startOfLevelOne, .1f);
    }
}