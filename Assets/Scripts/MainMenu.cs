using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject Player;
    public GameObject DeathScreen;
    public GameObject HealthBar;
    public GameObject Progress;

    public static float musicVolume {get; private set;}
    public static float soundVolume {get; private set;}

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Player.GetComponent<PlayerController>().enabled = false;
        Time.timeScale = 0;
        Cursor.visible = true;
        HealthBar.SetActive(false);
        Progress.SetActive(false); 
    }



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

        if (Player.GetComponent<PlayerHealth>().CurrentHea == 0)
        {
            Death();
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Cursor.visible = false;
        Player.GetComponent<PlayerController>().enabled = true;
        HealthBar.SetActive(true);
        Progress.SetActive(true);
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Player.GetComponent<PlayerController>().enabled = false;
        Cursor.visible = true;
        HealthBar.SetActive(false);
        Progress.SetActive(false);
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
        Time.timeScale = 1f;
        Cursor.visible = false;
        Player.GetComponent<PlayerController>().enabled = true;
        HealthBar.SetActive(true);
        Progress.SetActive(true);
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

    public void Death()
    {
        DeathScreen.SetActive(true);
        Time.timeScale = 0f;
        Player.GetComponent<PlayerController>().enabled = false;
        Cursor.visible = true;
        HealthBar.SetActive(false);
        Progress.SetActive(false);
    }

    public void Reset()
    {
        SceneManager.LoadScene("TestingGrounds");
        print("DeathScreen");
    }
}