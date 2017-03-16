using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event2Script : MonoBehaviour {
    public GameLogicScript gameLogic;
    public bool[] hasHappened;

    // Use this for initialization
    void Start () {
        hasHappened = new bool[3];
        gameLogic = GameLogicScript.gameLogic;
        gameLogic.eventManager.eventList[0].hasHappened = gameLogic.eventManager.eventList[1].hasHappened = gameLogic.eventManager.eventList[2].hasHappened = gameLogic.eventManager.eventList[3].hasHappened = true;
    
    }

    // Update is called once per frame
    void Update () {
        if (gameLogic == null)
        {

            gameLogic = GameLogicScript.gameLogic;
        }
        else if(!gameLogic.eventManager.eventList[4].hasHappened)
        {
            gameLogic.eventManager.activateEvent(4);
        }
    }
}
