using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorreVigilanciaScript : MonoBehaviour {
    public List<GameObject> _nearbySoldiers;
    public List<GameObject> _nearbyZombies;
    GameLogicScript gameLogic;
    Transform lastSeen;
    public bool alerted;
	// Use this for initialization
	void Start () {
        gameLogic = GameLogicScript.gameLogic;
    }

    // Update is called once per frame
    void Update() {
        if (gameLogic != null) {
            if (EventManager.eventManager != null) {
                if (!EventManager.eventManager.onEvent) {
                    if (!gameLogic.isPaused) {
                        _nearbyZombies.RemoveAll(gameLogic.IsNotAlive);
                        _nearbySoldiers.RemoveAll(gameLogic.IsNotAlive);

                        if (alerted) {
                            if (_nearbyZombies.Count != 0) {
                                lastSeen = _nearbyZombies[0].transform;
                            }
                            foreach(GameObject v in _nearbySoldiers) {
                                v.GetComponent<VillagerScript>().alerted = true;
                                v.GetComponent<VillagerScript>().heardSomething(lastSeen.position);
                            }
                        }

                    }

                }
            }
        }
    }
}
