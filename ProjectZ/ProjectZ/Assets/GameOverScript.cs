using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour {
    float contador = 0;
    bool hasEnded = false;
    bool done = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        contador += Time.deltaTime;

        if (!done) {
            done = true;
            if (EventManager.eventManager != null) {
                EventManager.eventManager.ResetAll();
                Destroy(EventManager.eventManager.gameObject);
            }

            if (GameLogicScript.gameLogic != null) {
                GameLogicScript.gameLogic.currentLevel = 1;
                FindObjectOfType<SaverScript>().SetLevel(1);
                FindObjectOfType<SaverScript>().SaveCurrentPrefs();
                Destroy(GameLogicScript.gameLogic.gameObject);
            }
            if(MusicManager.Instance!=null)
            Destroy(MusicManager.Instance.gameObject);
        }

        if (contador > 6&&!hasEnded) {
            hasEnded = true;
            SceneManager.LoadScene("Menu");
        }
	}
}
