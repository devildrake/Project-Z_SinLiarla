﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event5Script : MonoBehaviour {
    public bool[] hasHappened;
    GameLogicScript gameLogic;
    // Use this for initialization
    void Start() {
        hasHappened = new bool[1];
        gameLogic = GameLogicScript.gameLogic;
        if (gameLogic.camara == null) {
            gameLogic.camara = FindObjectOfType<CameraScript>();
        }

        //gameLogic.camara.transform.position = new Vector3(-14.3277f, 6.3f, -17.712f);
        gameLogic.camara.transform.position = new Vector3(-24.0f, 6.3f, -26.9f);
        gameLogic.camara.SetOrgPos();
        gameLogic.ClearLists();
    }

    // Update is called once per frame
    void Update() {
        if (!hasHappened[0]) {
            gameLogic.eventManager.activateEvent(8);
            hasHappened[0] = true;
        }
        else {
            if (gameLogic._villagers.Count == 0 && hasHappened[0]&&gameLogic.eventManager.eventList[8].hasHappened) {
                gameLogic.currentLevel = 6;
                SceneManager.LoadScene("EscenaInter");
            }
        }
    }
}
