using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int score = 0;
    //public ScoreDisplay[] scoreDisplay;

    private GameObject pauseMenu;
    private GameObject gameHUD;
    private GameObject deathScreen;
    private GameObject mainMenu;
    private ScoreDisplay scoreDisplay;

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
        
        scoreDisplay = FindObjectOfType<ScoreDisplay>();
    }

    //not sure about these two functions yet
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

        if (scene.name == "SampleScene" || scene.name == "SampleScene 1")
        {
            ResetGame(); //resets the score and health when a new scene is loaded (works for both "next level" and restart the current level)
        }
        
        GameObject canvas = GameObject.Find("Canvas"); //everything canvas related, the ui, main menu, all of them here

        if (canvas != null)
        {
            pauseMenu = canvas.transform.Find("PauseMenu")?.gameObject;
            gameHUD = canvas.transform.Find("GameHUD")?.gameObject;
            deathScreen = canvas.transform.Find("DeathScreen")?.gameObject;
            mainMenu = canvas.transform.Find("MainMenu")?.gameObject;

            if (scene.name == "SampleScene" || scene.name == "SampleScene 1")
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
    }

    void Update()
    {
        //pause menu by pressing ESC
        if (Input.GetKeyDown(KeyCode.Escape) && pauseMenu != null)
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        if (scoreDisplay == null)
            scoreDisplay = FindObjectOfType<ScoreDisplay>();

        if (scoreDisplay != null)
        {
            scoreDisplay.SetScore(score); //update score UI
        }
    }

    public void PauseGame() //pause button
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(true);
            isPaused = true;
            Time.timeScale = 0f;
        }
    }

    public void ResumeGame() //resume button from pause menu
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            isPaused = false;
            Time.timeScale = 1f;
        }
    }

    public void RestartGame() //restart button from pause menu
    {
        ResetGame();
        Time.timeScale = 1f;
        isPaused = false;
        score = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetGame() //this gets called everytime there s a restart game or new scene loaded, so the score and health resets
    {
        score = 0;

        if (scoreDisplay == null)
            scoreDisplay = FindObjectOfType<ScoreDisplay>();

        if (scoreDisplay != null)
        {
            scoreDisplay.SetScore(score); //update UI score to zero
        }
    }
    
    public void MainMenu() //main menu button from pause menu
    {
        Time.timeScale = 1f;
        isPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayGame() //play button from pause menu
    {
        Time.timeScale = 1f;
        isPaused = false;
        GameManager.instance.RestartGame();
        SceneManager.LoadScene("SampleScene");
    }
    
    //not sure i need this function anymore, but i'll keep it for now
    public void ShowDeathScreen()
    {
        if (deathScreen != null)
        {
            deathScreen.SetActive(true);
            if (gameHUD != null) gameHUD.SetActive(false);
            if (pauseMenu != null) pauseMenu.SetActive(false);
        }
        ResetGame();
    
        Time.timeScale = 0f;
    }

    public void QuitGame() //quit game button from both main menu and pause menu
    {
        Application.Quit();
    }
}
