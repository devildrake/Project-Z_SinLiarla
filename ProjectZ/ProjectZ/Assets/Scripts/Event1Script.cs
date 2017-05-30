using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Event1Script : MonoBehaviour {
    public GameLogicScript gameLogic;
    public GameObject animacionSeleccion;
    public GameObject objetoVida;
    public GameObject spawner;
    public bool[] hasHappened;
    bool once = false;
    public GameObject[] objetosZona = new GameObject[3];
    public GameObject runAwayPoint;
    public GameObject[] puntosDePatrulla = new GameObject[6];
    // Use this for initialization
    void Start() {
        animacionSeleccion.SetActive(false);
        objetoVida.SetActive(false);
        gameLogic = GameLogicScript.gameLogic;
        gameLogic.ClearLists();

        puntosDePatrulla[0].transform.position = new Vector3(-6.7f, 0.39f, 17.63f);
        puntosDePatrulla[1].transform.position = new Vector3(-6.7f, 0.39f, 15.97f);
        puntosDePatrulla[2].transform.position = new Vector3(-6.7f, 0.39f, -8.88f);
        puntosDePatrulla[3].transform.position = new Vector3(-6.7f, 0.39f, -10.54f);
        puntosDePatrulla[4].transform.position = new Vector3(-7.91f, 0.39f, 4.03f);
        puntosDePatrulla[5].transform.position = new Vector3(-7.91f, 0.39f, 2.37f);

        spawner = GameObject.FindGameObjectWithTag("SpecialSpawner");
        hasHappened = new bool[4];
        if (gameLogic.eventManager != null && gameLogic.camara != null)
        {
            gameLogic.currentLevel = 1;
            gameLogic.camara.gameObject.transform.position = new Vector3(4.330669f, 6.3f, -12.29557f);
            gameLogic.camara.SetOrgPos();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (gameLogic._keptSelectedZombies.Count != 0&&!hasHappened[0]) {
            animacionSeleccion.SetActive(false);
        }


        if (gameLogic == null)
        {
            gameLogic = GameLogicScript.gameLogic;
        }

        if (gameLogic.eventManager != null)
        {

        if (!once)
        {
            once = true;
            gameLogic.eventManager.activateEvent(0);
                animacionSeleccion.SetActive(true);
                objetoVida.SetActive(true);
                gameLogic.camara.DisableMovement();
        }
        if (objetosZona[0] != null && objetosZona[1] != null && objetosZona[2] != null && gameLogic.eventManager.eventList[0].hasHappened) {
            if (objetosZona[0].GetComponent<ZonaTutorial>().steppedOn && objetosZona[1].GetComponent<ZonaTutorial>().steppedOn && objetosZona[2].GetComponent<ZonaTutorial>().steppedOn)
            {
                gameLogic.eventManager.activateEvent(1);
                    animacionSeleccion.SetActive(false);
                    objetoVida.SetActive(false);
                    gameLogic.camara.EnableMovement();

                    if (gameLogic.eventManager.eventList[1].hasHappened && !hasHappened[1] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
                {
                        if (gameLogic != null)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            objetosZona[i].GetComponent<ZonaTutorial>().DestroyThis();
                        }
                            animacionSeleccion.SetActive(false);
                            objetoVida.SetActive(false);
                            gameLogic.SpawnVillager(spawner.GetComponent<EdificioCreaSoldiers>().spawnPoint);
                        gameLogic._villagers[0].GetComponent<VillagerScript>().patrolPointObject = spawner.GetComponent<EdificioCreaSoldiers>().specialPatrolPoint;
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
                            gameLogic.SpawnMutank(new Vector3(-0.5f, 0.0249f, -9.5f));
                            hasHappened[1] = true;
                    }
                }
            }
        }
        if (hasHappened[1] && !gameLogic.eventManager.eventList[2].hasHappened && gameLogic._villagers.Count == 0)
        {
            gameLogic.eventManager.activateEvent(2);
        }

        if (gameLogic.eventManager.eventList[2].hasHappened && !hasHappened[2] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            gameLogic.SpawnSoldier(spawner.GetComponent<EdificioCreaSoldiers>().specialPatrolPoint.transform.position, spawner.GetComponent<EdificioCreaSoldiers>().spawnPoint);
            hasHappened[2] = true;
        }

        if (hasHappened[2] && !gameLogic.eventManager.eventList[3].hasHappened && gameLogic._villagers.Count == 0)
        {
            gameLogic.eventManager.activateEvent(3);
        }

        if (gameLogic.eventManager.eventList[3].hasHappened && !hasHappened[3] && !gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            gameLogic.SpawnSoldier(new Vector3(-16.94f, 0.02499896f, 17.63f), puntosDePatrulla[0]);
            gameLogic.SpawnSoldier(new Vector3(-16.94f, 0.02499896f, 16), puntosDePatrulla[1]);
            gameLogic.SpawnSoldier(new Vector3(-16.94f, 0.02499896f, -8.88f), puntosDePatrulla[2]);
            gameLogic.SpawnSoldier(new Vector3(-16.94f, 0.02499896f, -10.54f), puntosDePatrulla[3]);
            gameLogic.SpawnSoldier(new Vector3(-18.18f, 0.02499896f, 4.03f), puntosDePatrulla[4]);
            gameLogic.SpawnSoldier(new Vector3(-18.18f, 0.02499896f, 2.37f), puntosDePatrulla[5]);
            hasHappened[3] = true;
        }

        if (gameLogic.eventManager.eventList[3].hasHappened && gameLogic._villagers.Count == 0) {
                gameLogic.currentLevel = 2;
                SceneManager.LoadScene(2);
                FindObjectOfType<SaverScript>().SetLevel(gameLogic.currentLevel);
                FindObjectOfType<SaverScript>().SaveCurrentPrefs();

        }
    } }
}
