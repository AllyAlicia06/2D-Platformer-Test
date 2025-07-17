using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public GameObject lockedDoorText;
    public GameObject unlockedDoorText;

    public Scene nextScene;
    private bool isPlayerNear = false;
    
    public int nextSceneBuildIndex;
    
    private void Start()
    {
        lockedDoorText.SetActive(false);
        unlockedDoorText.SetActive(false);
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            Debug.Log("Player near. Score: " + GameManager.instance.score);

            if (GameManager.instance.score >= 5 && Input.GetKeyDown(KeyCode.E))
            {
                LoadNextScene();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        isPlayerNear = true;
        
        if (GameManager.instance.score >= 5) //level finished!
        {
            if(unlockedDoorText != null)
                unlockedDoorText.SetActive(true);
        }
        else //locked door
        {
            if(lockedDoorText != null)
                lockedDoorText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        
        isPlayerNear = false;
        
        if(lockedDoorText != null)
            lockedDoorText.SetActive(false);
        
        if(unlockedDoorText != null)
            unlockedDoorText.SetActive(false);
    }

    void LoadNextScene()
    {
        Debug.Log("Attempting to load scene with index: " + nextSceneBuildIndex);

        if (nextSceneBuildIndex >= 0 && nextSceneBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneBuildIndex);
        }
        else
        {
            Debug.LogError("Invalid build index! Scene not found in Build Settings.");
        }
    }

}
