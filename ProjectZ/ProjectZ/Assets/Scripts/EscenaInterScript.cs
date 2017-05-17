using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscenaInterScript : MonoBehaviour {
    bool done;
    GameLogicScript gameLogic;
	// Use this for initialization
	void Start () {
        gameLogic = GameLogicScript.gameLogic;
        gameLogic.loadingScene = false;
        done = false;
    }
	
    void LoadCurrentLevel()
    {
        gameLogic.loadingScene = false;
        gameLogic.hasGamedOver = false;
        gameLogic.waitAFrame = false;

        SceneManager.LoadScene(gameLogic.currentLevel);
    }
    // Update is called once per frame
    void Update () {
        if (gameLogic == null)
        {
            gameLogic = GameLogicScript.gameLogic;
            gameLogic.loadingScene = false;
        }
        if (gameLogic != null && !done)
        {
            done = true;
            Invoke("LoadCurrentLevel",2.0f);

        }
	}
}
