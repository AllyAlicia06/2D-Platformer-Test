// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.Timeline;
//
// public class GameManager : MonoBehaviour
// {
//     public static GameManager instance;
//     
//     public int score = 0;  // Current score
//     public ScoreDisplay[] scoreDisplay;  //basically different images with arrays and scripts for score or health
//     //this is where i dragged the score image from the canvas so it got the actual sprites from the score
//     
//     //paused menu basically
//     public GameObject pauseMenu;
//     private bool isPaused = false;
//
//     void Awake()
//     {
//         if (instance == null)
//         {
//             instance = this;
//             DontDestroyOnLoad(gameObject);
//             Debug.Log("GameManager instance created.");
//         }
//         else
//         {
//             Debug.LogWarning("Duplicate GameManager destroyed.");
//             Destroy(gameObject);
//             return;
//         }
//     
//         Time.timeScale = 1f;
//     }
//     
//     //this is what i added in HOPES OF THE PAUSE MENU TO WORK AFTER COMING BACK FROM THE MAIN MENU
//     void OnEnable()
//     {
//         SceneManager.sceneLoaded += OnSceneLoaded;
//     }
//
//     void OnDisable()
//     {
//         SceneManager.sceneLoaded -= OnSceneLoaded;
//     }
//
//     void OnSceneLoaded(Scene scene, LoadSceneMode mode)
//     {
//         if (scene.name == "SampleScene")  // Gameplay scene
//         {
//             GameObject canvas = GameObject.Find("Canvas"); // or your canvas name
//             if (canvas != null)
//             {
//                 Transform pauseMenuTransform = canvas.transform.Find("PauseMenu");
//                 if (pauseMenuTransform != null)
//                 {
//                     pauseMenu = pauseMenuTransform.gameObject;
//                     pauseMenu.SetActive(false);
//                     Debug.Log("PauseMenu found and set inactive.");
//                 }
//                 else
//                 {
//                     Debug.LogError("PauseMenu not found as child of Canvas!");
//                     pauseMenu = null;
//                 }
//             }
//             else
//             {
//                 Debug.LogError("Canvas not found in the scene!");
//                 pauseMenu = null;
//             }
//             isPaused = false;
//             Time.timeScale = 1f;
//         }
//         else
//         {
//             // Scenes without a pause menu
//             pauseMenu = null;
//             isPaused = false;
//             Time.timeScale = 1f;
//         }
//     }
//
//     //
//     
//     public void AddScore(int scoreToAdd)
//     {
//         score += scoreToAdd;  // Add the score
//         UpdateScoreVisual(score);   // Update the UI (ScoreDisplay)
//     }
//
//     // Update the visuals of the score (UI images)
//     void UpdateScoreVisual(int currentScore)
//     {
//         if (scoreDisplay.Length > 0)
//         {
//             //the clamp thingy prevents my score from going over the array bounds
//             int spriteIndex = Mathf.Clamp(currentScore, 0, scoreDisplay[0].scoreSprites.Length - 1);
//             scoreDisplay[0].SetScore(spriteIndex);
//         }
//     }
//
//     //pause menu basically
//     void Update()
//     {
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             if (!isPaused) PauseGame();
//             else ResumeGame();
//         }
//     }
//
//     // public void PauseGame()
//     // {
//     //     pauseMenu.SetActive(true);
//     //     isPaused = true;
//     //     Time.timeScale = 0f;
//     // }
//     
//     public void PauseGame()
//     {
//         if (pauseMenu != null)
//         {
//             pauseMenu.SetActive(true);
//             isPaused = true;
//             Time.timeScale = 0f;
//         }
//         else
//         {
//             Debug.LogWarning("PauseMenu is null — cannot pause!");
//         }
//     }
//
//
//     // public void ResumeGame()
//     // {
//     //     pauseMenu.SetActive(false);
//     //     isPaused = false;
//     //     Time.timeScale = 1f;
//     // }
//     
//     public void ResumeGame()
//     {
//         if (pauseMenu != null)
//         {
//             pauseMenu.SetActive(false);
//             isPaused = false;
//             Time.timeScale = 1f;
//         }
//         else
//         {
//             Debug.LogWarning("PauseMenu is null — cannot resume!");
//         }
//     }
//
//     // public void MainMenu()
//     // {
//     //     SceneManager.LoadScene("MainMenu");
//     // }
//     
//     public void MainMenu()
//     {
//         Time.timeScale = 1f; // Important!
//         isPaused = false;
//         SceneManager.LoadScene("MainMenu");
//     }
//
//     public void QuitGame()
//     {
//         Application.Quit();
//     }
//
//     // public void PlayGame()
//     // {
//     //     SceneManager.LoadScene("SampleScene");
//     // }
//     
//     public void PlayGame()
//     {
//         Time.timeScale = 1f;
//         isPaused = false;
//         SceneManager.LoadScene("SampleScene");
//     }
// }

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    public ScoreDisplay[] scoreDisplay;

    private GameObject pauseMenu;
    private GameObject gameHUD;
    private GameObject deathScreen;
    private GameObject mainMenu;

    private bool isPaused = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Time.timeScale = 1f;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isPaused = false;
        Time.timeScale = 1f;

        GameObject canvas = GameObject.Find("Canvas");

        if (canvas != null)
        {
            pauseMenu = canvas.transform.Find("PauseMenu")?.gameObject;
            gameHUD = canvas.transform.Find("GameHUD")?.gameObject;
            deathScreen = canvas.transform.Find("DeathScreen")?.gameObject;
            mainMenu = canvas.transform.Find("MainMenu")?.gameObject;

            if (scene.name == "SampleScene")
            {
                if (pauseMenu != null) pauseMenu.SetActive(false);
                if (gameHUD != null) gameHUD.SetActive(true);
                if (deathScreen != null) deathScreen.SetActive(false);
                if (mainMenu != null) mainMenu.SetActive(false);
            }
            else if (scene.name == "MainMenu")
            {
                if (pauseMenu != null) pauseMenu.SetActive(false);
                if (gameHUD != null) gameHUD.SetActive(false);
                if (deathScreen != null) deathScreen.SetActive(false);
                if (mainMenu != null) mainMenu.SetActive(true);
            }
        }
        else
        {
            Debug.LogWarning("Canvas not found in scene: " + scene.name);
            pauseMenu = null;
            gameHUD = null;
            deathScreen = null;
            mainMenu = null;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu != null)
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreVisual(score);
    }

    void UpdateScoreVisual(int currentScore)
    {
        if (scoreDisplay.Length > 0)
        {
            int spriteIndex = Mathf.Clamp(currentScore, 0, scoreDisplay[0].scoreSprites.Length - 1);
            scoreDisplay[0].SetScore(spriteIndex);
        }
    }
    
    public HealthDisplay healthDisplay;  // assign this in inspector

    public void UpdateHealth(int currentHealth)
    {
        healthDisplay.SetHealth(currentHealth);
    }

    public void PauseGame()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            if (gameHUD != null) gameHUD.SetActive(false);
            if (pauseMenu != null) pauseMenu.SetActive(false);
        }
        Time.timeScale = 0f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
