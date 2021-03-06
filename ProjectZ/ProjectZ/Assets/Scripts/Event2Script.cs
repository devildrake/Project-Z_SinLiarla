﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event2Script : MonoBehaviour {
    public GameLogicScript gameLogic;
    EventManager eventManager;
    public bool[] hasHappened;
    bool cFind;
    bool waitForAFrame = false;
    // Use this for initialization


    void Start() {
        cFind = false;
        hasHappened = new bool[3];
        gameLogic = GameLogicScript.gameLogic;
        eventManager = EventManager.eventManager;

        gameLogic.ClearLists();
        if (gameLogic.eventManager != null && gameLogic.camara != null) {
            // gameLogic.eventManager.eventList[0].hasHappened = gameLogic.eventManager.eventList[1].hasHappened = gameLogic.eventManager.eventList[2].hasHappened = gameLogic.eventManager.eventList[3].hasHappened = true;
            gameLogic.camara.gameObject.transform.position = new Vector3(0.6264908f, 6.3f, -14.65801f);
            gameLogic.currentLevel = 2;
            gameLogic.camara.SetOrgPos();
        }
        else {
            cFind = true;
        }
    }

    // Update is called once per frame
    void Update() {

        if (gameLogic == null) {
            gameLogic = GameLogicScript.gameLogic;
        }
        if (!waitForAFrame) {
            waitForAFrame = true;

        }else { 
        if (eventManager == null) {
            eventManager = EventManager.eventManager;
        }

        if (gameLogic.eventManager != null) {
            if (cFind) {
                cFind = false;
                gameLogic = GameLogicScript.gameLogic;
                gameLogic.camara.gameObject.transform.position = new Vector3(0.6264908f, 6.3f, -14.65801f);
                gameLogic.currentLevel = 2;
                gameLogic.camara.SetOrgPos();
                gameLogic.loadingScene = false;
            }
            else if (!gameLogic.eventManager.eventList[4].hasHappened && !hasHappened[0]) {
                eventManager.activateEvent(4);
            }

            if (!hasHappened[0] && eventManager.eventList[4].hasHappened)
                hasHappened[0] = true;

            if (eventManager != null) {
                if (eventManager.eventList[4].hasHappened && hasHappened[0]) {
                    //SceneManager.LoadScene(3);
                    bool endLevel = true;
                    foreach (GameObject bas in gameLogic._bases) {
                        if (bas.GetComponent<EdificioCreaSoldiers>().counter != 4) {
                            endLevel = false;
                        }
                    }
                    if (gameLogic._villagers.Count != 0) {
                        endLevel = false;
                    }

                    if (endLevel) {
                        eventManager.activateEvent(5);
                        hasHappened[1] = true;
                    }

                }
                if (eventManager.eventList[5].hasHappened) {
                        gameLogic.currentLevel = 3;
                        SceneManager.LoadScene("EscenaInter");
                    }
                }
        }
    }
}
}
