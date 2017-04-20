using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event2Script : MonoBehaviour {
    public GameLogicScript gameLogic;
    public bool[] hasHappened;
    bool cFind;
    // Use this for initialization


    void Start () {
        cFind = false;
        hasHappened = new bool[3];
        gameLogic = GameLogicScript.gameLogic;


        gameLogic.ClearLists();
        if (gameLogic.eventManager != null&&gameLogic.camara!=null)
        {
            gameLogic.eventManager.eventList[0].hasHappened = gameLogic.eventManager.eventList[1].hasHappened = gameLogic.eventManager.eventList[2].hasHappened = gameLogic.eventManager.eventList[3].hasHappened = true;
            gameLogic.camara.gameObject.transform.position = new Vector3(0.6264908f, 6.3f, -14.65801f);
            gameLogic.currentLevel = 2;
            gameLogic.camara.SetOrgPos();
        }
        else
        {
            cFind = true;
        }
    }

    // Update is called once per frame
    void Update () {
        if (cFind)
        {
            cFind = false;
            gameLogic = GameLogicScript.gameLogic;
            gameLogic.eventManager.eventList[0].hasHappened = gameLogic.eventManager.eventList[1].hasHappened = gameLogic.eventManager.eventList[2].hasHappened = gameLogic.eventManager.eventList[3].hasHappened = true;
            gameLogic.camara.gameObject.transform.position = new Vector3(0.6264908f, 6.3f, -14.65801f);
            gameLogic.currentLevel = 2;
            gameLogic.camara.SetOrgPos();
        }

        if (gameLogic == null)
        {
            gameLogic = GameLogicScript.gameLogic;
        }
        else if(!gameLogic.eventManager.eventList[4].hasHappened&&!hasHappened[0])
        {
            gameLogic.eventManager.activateEvent(4);
            hasHappened[0] = true;
        }

        if (gameLogic.eventManager != null)
        {
            if (gameLogic.eventManager.eventList[4].hasHappened&&hasHappened[1])
            {
                Application.LoadLevel(3);
                bool endLevel = true;
                foreach (GameObject bas in gameLogic._bases)
                {
                    if (bas.GetComponent<EdificioCreaSoldiers>().counter != 4)
                    {
                        endLevel = false;
                    }
                }
                if (gameLogic._villagers.Count != 0)
                {
                    endLevel = false;
                }

                if (endLevel)
                {
                    gameLogic.eventManager.activateEvent(5);
                    hasHappened[1] = true;
                }

            }
            else if (gameLogic.eventManager.eventList[5].hasHappened)
            {
                Application.LoadLevel(3);
            }
        }
        }
}
