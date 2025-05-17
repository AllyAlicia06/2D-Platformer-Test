using UnityEngine;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public int score = 0;  // Current score
    public ScoreDisplay[] scoreDisplay;  //basically different images with arrays and scripts for score or health
    //this is where i dragged the score image from the canvas so it got the actual sprites from the score
    
    //paused menu basically
    public GameObject pauseMenu;
    private bool isPaused = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;  // Add the score
        UpdateScoreVisual(score);   // Update the UI (ScoreDisplay)
    }

    // Update the visuals of the score (UI images)
    void UpdateScoreVisual(int currentScore)
    {
        if (scoreDisplay.Length > 0)
        {
            //the clamp thingy prevents my score from going over the array bounds
            int spriteIndex = Mathf.Clamp(currentScore, 0, scoreDisplay[0].scoreSprites.Length - 1);
            scoreDisplay[0].SetScore(spriteIndex);
        }
    }

    //pause menu basically
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) PauseGame();
            else ResumeGame();
        }
    }

    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
    }

    public void MainMenu()
    {
        //cand o sa bag main menu aici vine codu
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}