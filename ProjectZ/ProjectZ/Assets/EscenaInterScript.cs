using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscenaInterScript : MonoBehaviour {
    bool done;
    GameLogicScript gameLogic;
	// Use this for initialization
	void Start () {
        done = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (gameLogic == null)
        {
            gameLogic = GameLogicScript.gameLogic;
        }
        if (gameLogic != null && !done)
        {
            done = true;
            Application.LoadLevel(gameLogic.currentLevel);
        }
	}
}
