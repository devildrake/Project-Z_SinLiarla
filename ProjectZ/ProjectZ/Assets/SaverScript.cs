﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaverScript : MonoBehaviour {
    public float savedSFXVolume, savedVolume;
    public int savedLevel;
    public static SaverScript saver;
    public bool hasLoaded = false;
    // Use this for initialization

    void Awake() {
        if (saver == null) {
            DontDestroyOnLoad(gameObject);
            saver = this;
        }
        else if (saver != this) {
            Destroy(gameObject);
        }
    }

    void Start () {
        LoadPrefs();
	}
	
	// Update is called once per frame
	void Update () {

	}


    void OnEnable() {
    //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
    SceneManager.sceneLoaded += OnLevelFinishedLoading;
}

void OnDisable() {
    //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
    SceneManager.sceneLoaded -= OnLevelFinishedLoading;
}

void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
    Debug.Log("Level Loaded");
    Debug.Log(scene.name);
    Debug.Log(mode);

        if (scene.name == "menu") {
            //MusicManager musicManager = FindObjectOfType<MusicManager>();
            LoadPrefs();
            GameObject.FindGameObjectWithTag("SfxSlider").GetComponent<Slider>().value = savedSFXVolume;
            GameObject.FindGameObjectWithTag("VolumeSlider").GetComponent<Slider>().value = savedVolume;

            //musicManager.Volume = savedVolume;
            //musicManager.SFXVolume = savedSFXVolume;


        }


        if (scene.name == "Tutorial") {
            // GameLogicScript gameLogic = FindObjectOfType<GameLogicScript>();
            //MusicManager musicManager = FindObjectOfType<MusicManager>();


            GameObject.FindGameObjectWithTag("SfxSlider").GetComponent<Slider>().value = savedSFXVolume;
            GameObject.FindGameObjectWithTag("VolumeSlider").GetComponent<Slider>().value = savedVolume;
            //musicManager.Volume = savedVolume;
            //musicManager.SFXVolume = savedSFXVolume;

        }


    }



public void SaveCurrentPrefs(float vol, float sfx, int l) {
        Debug.Log("SavedProgress");
        PlayerPrefs.SetFloat("sfxVolume", sfx);
        PlayerPrefs.SetFloat("musicVolume", vol);
        PlayerPrefs.SetInt("CurrentLevel", l);
    }

    public void SaveCurrentPrefs() {
        PlayerPrefs.SetFloat("sfxVolume", savedSFXVolume);
        PlayerPrefs.SetFloat("musicVolume", savedVolume);
        PlayerPrefs.SetInt("CurrentLevel", savedLevel);
    }

    public void LoadPrefs() {
        hasLoaded = true;
        if (!PlayerPrefs.HasKey("sfxVolume")) {
            PlayerPrefs.SetFloat("sfxVolume", 0.7f);
        }

        if (!PlayerPrefs.HasKey("musicVolume"))
            PlayerPrefs.SetFloat("musicVolume", 0.7f);

        if (!PlayerPrefs.HasKey("CurrentLevel"))
            PlayerPrefs.SetFloat("CurrentLevel", 1);

        

        savedSFXVolume = PlayerPrefs.GetFloat("sfxVolume", 0.7f);
        savedVolume =  PlayerPrefs.GetFloat("musicVolume", 0.7f);
        savedLevel = PlayerPrefs.GetInt("CurrentLevel", 1);

        if(savedLevel == 0) {
            savedLevel = 1;
        }

        Debug.Log("LoadingProgress");

    }

    public void SetLevel(int l) {
        savedLevel = l;
    }

    public void GetVolumes(float vol, float sfx) {
        vol = savedVolume;
        sfx = savedSFXVolume;
    }

    public int GetLevel() {
        LoadPrefs();
        if (savedLevel == 0) {
            savedLevel = 1;
        }
        return savedLevel;
    }
}
