using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event4Script : MonoBehaviour {
    GameLogicScript gameLogic;
    CameraScript camara;
    public bool[] hasHappened;
    // Use this for initialization

    private void Awake()
    {
        gameLogic = GameLogicScript.gameLogic;
        gameLogic.ClearLists();
    }

    void Start () {
        hasHappened = new bool[1];
        gameLogic = GameLogicScript.gameLogic;
        gameLogic.currentLevel = 4;

        if (gameLogic.camara == null) {
            gameLogic.camara = FindObjectOfType<CameraScript>();
        }        //camara.TOPLIMIT = 10;
        //camara.BOTLIMIT = -20;
        //camara.RIGHTLIMIT = 36;
        //camara.LEFTLIMIT = -36;
        camara.TOPLIMIT = 25;
        camara.BOTLIMIT = -40;
        camara.RIGHTLIMIT = -10;
        camara.LEFTLIMIT = 10;
        camara.mode = 1;
        gameLogic.currentLevel = 4;

        camara.SetPos(new Vector3(-35.0f,9.9f,2.93594f));
        camara.SetOrgPos();
        gameLogic.ClearLists();

    }




    // Update is called once per frame
    void Update () {
        if (!hasHappened[0]) {
            gameLogic.eventManager.activateEvent(6);
            hasHappened[0] = true;
        }
        else {
            if (gameLogic._villagers.Count == 0 && hasHappened[0]) {
                gameLogic.currentLevel = 5;
                SceneManager.LoadScene(5);
            }
        }
    }   
}

