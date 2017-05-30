using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event6Script : MonoBehaviour {
    public bool[] hasHappened;
    GameLogicScript gameLogic;
    // Use this for initialization
    void Start()
    {
        hasHappened = new bool[1];
        gameLogic = GameLogicScript.gameLogic;

        if (gameLogic.camara == null)
        {
            gameLogic.camara = FindObjectOfType<CameraScript>();
        }
        gameLogic.camara.SetOrgPos();
        gameLogic.ClearLists();
        gameLogic.camara.TOPLIMIT = 30;
        //gameLogic.camara.BOTLIMIT = -40;
        //gameLogic.camara.RIGHTLIMIT = -10;
        gameLogic.camara.LEFTLIMIT = -10;

    }

    // Update is called once per frame
    void Update()
    {
        if (!hasHappened[0])
        {
            gameLogic.eventManager.activateEvent(9);
            hasHappened[0] = true;
        }
        else {
            if (gameLogic._villagers.Count == 0 && hasHappened[0]&&gameLogic.eventManager.eventList[9].hasHappened)
            {
                gameLogic.currentLevel = 7;
                SceneManager.LoadScene("EscenaInter");
            }
        }
    }
}
