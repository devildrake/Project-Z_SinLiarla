using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lataScript : MonoBehaviour {
    GameLogicScript gameLogic;
    private bool noise;
    public int ondaSonora;
    List<GameObject> closeHumans;
	// Use this for initialization
	void Start () {
        gameLogic = GameLogicScript.gameLogic;
        noise = false;
        ondaSonora = 15;
	}

    // Update is called once per frame
    void Update() {
        if(!gameLogic.isPaused && !gameLogic.eventManager.onEvent){
            if (noise) {
                foreach (GameObject v in gameLogic._villagers) {
                    if ((v.transform.position - gameObject.transform.position).magnitude < ondaSonora) {

                        v.GetComponent<VillagerScript>().heardSomething(gameObject.transform.position);

                    }
                }
            }
        } }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Zn")
        {
            Debug.Log("No me des la Lata");
                noise = true;
            Invoke("NoTrue", 2f);
        }
    }
    void NoTrue() {
        noise = false;
    }
}
