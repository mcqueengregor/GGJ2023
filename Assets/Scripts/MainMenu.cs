using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused) //if gameIsPaused is true, then resume playing
            {
                Resume();
            }
            else //if gameIsPaused it false, then pause gameplay
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void ExitGame()
    {    
        //Exits the game application
        Application.Quit();
        Debug.Log("The game has been closed.");
    }

    public void StartGame()
    {
        //Chanages from the Main menu to start the game
        //SceneManager.LoadScene("Main Prototype");
        Debug.Log("Start has been selected.");
    }

    
}
