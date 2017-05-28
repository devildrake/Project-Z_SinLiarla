using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScriptConfirmarSalir : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	// Update is called once per frame
	void Update () {
		
	}

    public void Save() {
        if (FindObjectOfType<SaverScript>() != null) {
            int level = 0;
            float sfxVol = 0;
            float vol = 0;


            if (GameLogicScript.gameLogic!=null) {
                level = GameLogicScript.gameLogic.currentLevel;
            }
            else{
                Debug.Log("Couldn't find gameLogic");
                return;
            }
            if (FindObjectOfType<MusicManager>() != null){
                sfxVol= FindObjectOfType<MusicManager>().SFXVolume;
                vol = FindObjectOfType<MusicManager>().Volume;
            }
            else{
                Debug.Log("Couldn't find MusicManager");
                return;
            }

            FindObjectOfType<SaverScript>().SaveCurrentPrefs(vol, sfxVol, level);
        }else {
            Debug.Log("Couldn't find a Saver");
        }
    }

}
