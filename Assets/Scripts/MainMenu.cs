using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenuUI;

    public static float musicVolume {get; private set;}
    public static float soundVolume {get; private set;}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused) //if gameIsPaused is true, then resume playing
            {
                Resume();
            }
            else //if gameIsPaused is false, then pause gameplay
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
        Debug.Log("Start has been selected.");
    }

        //change volume for music on slider value change
    public void onMusicSliderVolumeChange(float value)
    {
        musicVolume = value;
        //AudioManager.Instance.UpdateMixerVolume();
    }

    //change volume for sfx on slider value change
    public void onSoundSliderVolumeChange(float value)
    {
        soundVolume = value;
    }
}