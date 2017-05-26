using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VisionTorreVigilancia : MonoBehaviour {
    TorreVigilanciaScript torreVigilancia;
    float contadorQuitarAlarm = 5;
    float movSpeed = 0.5f;
    bool goPointA = true;
    public GameObject pointA;
    public GameObject pointB;
    public float distanciaMax = 6.5f;
    public float limiteDistanciaSoldados = 6.5F;
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

                        UpdateNearbySoldiers(limiteDistanciaSoldados);

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
                                FollowZombie();
                            }
                        }else {
                            Patrol();
                        }
                    }
                }
            }
        }
    }

    void UpdateNearbySoldiers(float l) {
        torreVigilancia._nearbySoldiers.RemoveAll(GameLogicScript.gameLogic.IsNotAlive);
        foreach (GameObject v in GameLogicScript.gameLogic._villagers) {
            if (v.GetComponent<VillagerScript>().tipo == VillagerScript.humanClass.soldier) {
                if ((v.transform.position - torreVigilancia.transform.position).magnitude < l) {
                    if (!torreVigilancia._nearbySoldiers.Contains(v)) {
                        torreVigilancia._nearbySoldiers.Add(v);
                    }
                }else {
                    if (torreVigilancia._nearbySoldiers.Contains(v)) {
                        torreVigilancia._nearbySoldiers.Remove(v);
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

    bool isCloseEnough(GameObject a, float lindar) {
        if (Mathf.Abs((a.transform.position - gameObject.transform.position).magnitude) < lindar) {
            return true;
        }
        else return false;           
    }

    void Patrol() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        if (goPointA) {
            Vector3 direction = pointA.transform.position - gameObject.transform.position;
            gameObject.transform.position += direction.normalized * movSpeed * Time.deltaTime;
            if (isCloseEnough(pointA, 0.5f)) {
                goPointA = false;
            }
        }else {
            Vector3 direction = pointB.transform.position - gameObject.transform.position;
            gameObject.transform.position += direction.normalized * movSpeed * Time.deltaTime;
            if (isCloseEnough(pointB, 0.5f)) {
                goPointA = true;
            }
        }
    }

    bool IsTooFar(float lindar) {
        if ((gameObject.transform.position - torreVigilancia.gameObject.transform.position).magnitude > lindar) {
            return true;
        } else return false;

    }

    void FollowZombie() {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        if (!IsTooFar(distanciaMax)) {
            Vector3 zombieAtOurHeight = torreVigilancia._nearbyZombies[0].transform.position;
            zombieAtOurHeight.y = gameObject.transform.position.y;


            if (!isCloseEnough(torreVigilancia._nearbyZombies[0], 0.1f)) {
                Vector3 direction = zombieAtOurHeight - gameObject.transform.position;
                gameObject.transform.position += direction.normalized * movSpeed * 2 * Time.deltaTime;
            }
        }
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
