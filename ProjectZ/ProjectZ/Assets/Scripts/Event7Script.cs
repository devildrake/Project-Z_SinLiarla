using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event7Script : MonoBehaviour {
    public bool[] hasHappened;
    GameLogicScript gameLogic;
    // Use this for initialization
    void Start() {
        hasHappened = new bool[1];
        gameLogic = GameLogicScript.gameLogic;

        if (gameLogic.camara == null) {
            gameLogic.camara = FindObjectOfType<CameraScript>();
        }
        gameLogic.camara.SetPos(new Vector3(-11.4f, 6.3f, 8.7f));
        gameLogic.camara.SetOrgPos();
        gameLogic.ClearLists();
        gameLogic.camara.TOPLIMIT = 20;
        //gameLogic.camara.BOTLIMIT = -40;
        //gameLogic.camara.RIGHTLIMIT = -10;
        gameLogic.camara.LEFTLIMIT = -20;

    }

    // Update is called once per frame
    void Update() {
        if (!hasHappened[0]) {
            gameLogic.eventManager.activateEvent(10);
            hasHappened[0] = true;
        }
        else {
            if (gameLogic._villagers.Count == 0 && hasHappened[0]) {
                gameLogic.currentLevel = 8;
                SceneManager.LoadScene("EscenaGameOver");
            }
        }
    }
}
