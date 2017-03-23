using UnityEngine;
using System.Collections;

public class EdificioCreaSoldiers : MonoBehaviour {
    public bool isSpecial;
    //GameObject villager;
    public bool alert;
    public bool alertColor;
    public float spawnTimer;
    public float spawnTime;
    public GameLogicScript gameLogic;
    public GameObject specialPatrolPoint;
    public Vector3 spawnPoint;
    public int amount;
    public int counter;
    public Vector3[] posiciones;
    public bool isBlocked;
    public GameObject blocker;
    public GameObject spawnPointObject;
    Renderer render;

    // Use this for initialization
    void Start () {
        gameLogic = GameLogicScript.gameLogic;

        gameLogic._bases.Add(gameObject);

        isBlocked = false;
        amount = 4;
        posiciones = new Vector3[amount];
        counter = 0;
        posiciones[3] = gameObject.transform.position + new Vector3(1,0,-2);
        posiciones[2] = gameObject.transform.position + new Vector3(0,0,-1);
        posiciones[1] = gameObject.transform.position + new Vector3(1,0,0);
        posiciones[0] = gameObject.transform.position + new Vector3(2,0,0);

        posiciones[3].y = 0.3859999f;
        posiciones[2].y = 0.3859999f;
        posiciones[1].y = 0.3859999f;
        posiciones[0].y = 0.3859999f;

        //spawnPoint = gameObject.transform.position+ new Vector3(-3.719f, 0.539f, -4.338f);

        //villager = Resources.Load("VillagerObject") as GameObject;
        spawnTimer = 0f;
        spawnTime = 4f;
        alert = alertColor = false;
        render = gameObject.GetComponentInChildren<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        spawnPoint = spawnPointObject.transform.position;

        if (gameLogic == null) {
            gameLogic = GameLogicScript.gameLogic;
            }
        if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            if (!isSpecial)
            {
                /*if (blocker == null)
                 {
                     isBlocked = false;
                 }
                 else {
                     isBlocked = true;
                 }*/

                if (alert && amount > 0 && !isBlocked)
                {
                    if (!alertColor)
                    {
                        alertColor = true;
                        render.material.color += Color.red;

                        // GetComponent<MeshRenderer>().material.color = Color.red;
                    }
                    spawnTimer += Time.deltaTime;
                    if (spawnTime <= spawnTimer)
                    {
                        //spawn(Random.Range(0, 1));
                        gameLogic.SpawnSoldier(spawnPoint+new Vector3(Random.Range(-1,1),0,Random.Range(-1,0)));
                        counter++;
                        amount--;
                        spawnTimer = 0;
                    }
                }
            }
        }
    }
    //void spawn(int tipo) {
    //    switch (tipo) {
    //        case 0: //VILLAGER
    //            GameObject villager2 = Instantiate(villager, gameObject.transform.position+posiciones[counter], Quaternion.identity) as GameObject;
    //            villager2.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.villager;
    //            gameLogic._villagers.Add(villager2);
    //            counter++;
    //            break;
    //        case 1: //SOLDADO
    //            GameObject soldier = Instantiate(villager, gameObject.transform.position + posiciones[counter], Quaternion.identity) as GameObject;
    //            soldier.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
    //            gameLogic._villagers.Add(soldier);
    //            counter++;
    //            break;
    //    }
    //}
}
