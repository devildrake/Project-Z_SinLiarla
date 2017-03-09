using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Event1Script : MonoBehaviour {
    public  GameLogicScript gameLogic;
    public GameObject spawner;
    public bool[] hasHappened;
    public GameObject[] objetosZona = new GameObject[3];
    // Use this for initialization
    void Start() {
        gameLogic = GameLogicScript.gameLogic;
        spawner = GameObject.FindGameObjectWithTag("SpecialSpawner");
        hasHappened = new bool[10];
    }

    // Update is called once per frame
    void Update()
    {
        if (gameLogic == null)
        {
            gameLogic = GameLogicScript.gameLogic;
        }
        if (objetosZona[0] != null && objetosZona[1] != null && objetosZona[2] != null) { 
            if (objetosZona[0].GetComponent<ZonaTutorial>().steppedOn && objetosZona[1].GetComponent<ZonaTutorial>().steppedOn && objetosZona[2].GetComponent<ZonaTutorial>().steppedOn)
            {
                gameLogic.eventManager.activateEvent(1);

                if (gameLogic.eventos[1] && !hasHappened[1] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
                {
                    if (gameLogic != null)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            objetosZona[i].GetComponent<ZonaTutorial>().DestroyThis();
                        }

                        gameLogic.SpawnVillager(spawner.GetComponent<EdificioCreaSoldiers>().spawnPoint+spawner.gameObject.transform.position);
                        gameLogic.SpawnWalker(new Vector3(2.5f, 0.0249f, -9.5f));
                        gameLogic.SpawnWalker(new Vector3(6f, 0.0249f, -9.5f));
                        gameLogic.SpawnMutank(new Vector3(-0.5f, 0.0249f, -9.5f));
                        hasHappened[1] = true;
                    }
                    else
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            objetosZona[i].GetComponent<ZonaTutorial>().DestroyThis();
                        }
                        gameLogic = GameLogicScript.gameLogic;
                        gameLogic.SpawnVillager(spawner.GetComponent<EdificioCreaSoldiers>().spawnPoint);
                        gameLogic._villagers[0].GetComponent<VillagerScript>().patrolPointObject = spawner.GetComponent<EdificioCreaSoldiers>().specialPatrolPoint;
                        gameLogic.SpawnWalker(new Vector3(2.5f, 0.0249f, -9.5f));
                        gameLogic.SpawnWalker(new Vector3(6f, 0.0249f, -9.5f));
                        hasHappened[1] = true;
                    }
                }
            }
    }
        if (gameLogic.eventos[1] && !gameLogic.eventos[2] && gameLogic._villagers.Count == 0)
        {
            gameLogic.eventManager.activateEvent(2);
        }

            if (gameLogic.eventManager.eventList[2].hasHappened&& !hasHappened[2] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
            {
                gameLogic.SpawnSoldier(spawner.GetComponent<EdificioCreaSoldiers>().spawnPoint);
                hasHappened[2] = true;
            }

        if (gameLogic.eventos[2] && !gameLogic.eventos[3] && gameLogic._villagers.Count == 0)
        {
            gameLogic.eventManager.activateEvent(3);
        }

        if (gameLogic.eventManager.eventList[3].hasHappened && !hasHappened[3] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            gameLogic.SpawnVillager(new Vector3(16.94f,0.39f,17.63f),new Vector3(-6.7f,0.39f,17.63f));
            gameLogic.SpawnVillager(new Vector3(16.94f, 0.39f,16), new Vector3(-6.7f, 0.39f,15.97f));
            gameLogic.SpawnVillager(new Vector3(16.94f, 0.39f,-8.88f), new Vector3(-6.7f, 0.39f,-8.88f));
            gameLogic.SpawnVillager(new Vector3(16.94f, 0.39f,-10.54f), new Vector3(-6.7f, 0.39f,-10.54f));
            gameLogic.SpawnVillager(new Vector3(-18.18f, 0.39f,4.03f), new Vector3(-7.91f,0.39f,4.03f));
            gameLogic.SpawnVillager(new Vector3(-18.18f, 0.39f,2.37f), new Vector3(-7.91f,0.39f,2.37f));
            hasHappened[3] = true;
        }

        if (gameLogic.eventos[3] && gameLogic._villagers.Count == 0) {
            Application.LoadLevel(2);
        }
        for (int i = 0; i < 10; i++)
        {
            hasHappened[i] = gameLogic.eventos[i];
        }
    }
}
