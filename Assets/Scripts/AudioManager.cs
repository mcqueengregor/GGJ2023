using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager: MonoBehaviour
{
    //set audio manager instance to null
    public static AudioManager instance = null;

    //Clip arrays to call specific music and sounds from
    public AudioClip[] musicList;
    public AudioClip[] sfxList;

    //change to the volumes of music and sound
    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundMixerGroup;

    //audio sources
    public AudioSource musicSource;
    public AudioSource soundSource;

    // Initialize the singleton instance.
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		//If instance exists, destroy this object is for enforce singleton. 
		else if (instance != this)
		{
			Destroy(gameObject);
            return;
		}
		//Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

    // Play a single clip through the sound effects source.
	public void PlaySound(AudioSource sound)
	{

        if(sound == null) //does sound not exist or incorrect call
        {
            Debug.Log("SoundFX Source cannot be found.");
        }
        else
        {
		    soundSource.Play();
        }
	}
	// Play a single clip through the music source.
	public void PlayMusic(AudioSource music)
	{

        if(music == null) //does music not exist or incorrect call
        {
            Debug.Log("Music Source cannot be found.");
        }
        else
        {
            //musicSource.clip = music;
		    musicSource.Play();
        }
	}

    private void Start()
    {
       //play music on start
    }

    public void UpdateMixerVolume()
    {
        //change the music and sound volumes
        musicMixerGroup.audioMixer.SetFloat("MusicMixerVolume", Mathf.Log10(MainMenu.musicVolume) * 20);
        soundMixerGroup.audioMixer.SetFloat("SoundMixerVolume", Mathf.Log10(MainMenu.soundVolume) * 20);
    }
    
}
