using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionTorreVigilancia : MonoBehaviour {
    TorreVigilanciaScript torreVigilancia;
    float contadorQuitarAlarm = 5;

    // Use this for initialization
    void Start() {
        torreVigilancia = GetComponentInParent<TorreVigilanciaScript>();
    }

    // Update is called once per frame
    void Update() {
        if (GameLogicScript.gameLogic != null) {
            if (EventManager.eventManager != null) {
                if (!EventManager.eventManager.onEvent) {
                    if (!GameLogicScript.gameLogic.isPaused) {

                        if (torreVigilancia._nearbyZombies.Count != 0) {
                            torreVigilancia.alerted = true;
                            contadorQuitarAlarm = 0;
                        }
                        else if (contadorQuitarAlarm < 5) {
                            contadorQuitarAlarm += Time.deltaTime;
                        }

                        if (contadorQuitarAlarm > 5) {
                            torreVigilancia.alerted = false;
                        }

                        if (torreVigilancia.alerted) {
                            if (torreVigilancia._nearbyZombies.Count != 0) {
                                gameObject.transform.position = torreVigilancia._nearbyZombies[0].transform.position;
                            }
                        }else {
                            Patrol();
                        }
                    }
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Zn") {
            if (!torreVigilancia._nearbyZombies.Contains(other.gameObject)) {
                torreVigilancia._nearbyZombies.Add(other.gameObject);
            }
        }
    }

    void Patrol() {

    }

    private void OnTriggerStay(Collider other) {
        if (other.tag == "Zn") {
            if (!torreVigilancia._nearbyZombies.Contains(other.gameObject)) {
                torreVigilancia._nearbyZombies.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Zn") {
            if (torreVigilancia._nearbyZombies.Contains(other.gameObject)) {
                torreVigilancia._nearbyZombies.Remove(other.gameObject);
            }
        }
    }
}
