using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorretaScript : MonoBehaviour {
    public List<GameObject> _zombiesInArea;
    public List<GameObject> _targetingZombies;
    public GameLogicScript gameLogic;
    public GameObject closestZombie;
    public GameObject closestMutank;
    public GameObject Cañon;
    public GameObject ObjetoPosicionRoto;
    float contadorTiempoAtaque;
    public bool alive = true;
    public bool hasChanged = false;
    public float health;
    public const float maxHealth = 100;
    public float attackSpeed = 0.2f;
    public float attackDamage = 1;
    private bool attacking;
    public ParticleSystem shoot;

    // Use this for initialization
    void Start() {
        gameLogic = GameLogicScript.gameLogic;
        gameLogic._barricadas.Add(gameObject);
        health = maxHealth;
        attacking = false;
        shoot.Stop();
    }

    bool IsNotAlive(GameObject z) {
        if (z != null) {
            if (z.GetComponent<ZombieScript>() != null) {
                return !z.GetComponent<ZombieScript>().isAlive;
            }
            else {
                return !z.GetComponent<VillagerScript>().isAlive;
            }
        }
        else {
            return true;
        }
    }

    GameObject GetClosestZombie(List<GameObject> zombies) {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in zombies) {
            if (t != null) {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist) {
                    tMin = t;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }

    GameObject CheckForMutanks() {
        if (_zombiesInArea.Count > 0) {
            foreach (GameObject z in _zombiesInArea) {
                if (z.GetComponent<ZombieScript>().tipo == ZombieScript.zombieClass.mutank) {
                    return z;
                }
                else {
                    return null;
                }
            }
        }
        else {
            return null;
        }
        return null;
    }


    // Update is called once per frame
    void Update() {
        if (gameLogic == null) {
            gameLogic = GameLogicScript.gameLogic;
        }

        

        //Si gamelogic no es null, se comprueba si hay un evento en marcha o esta el juego pausado para realizar el comportamiento normal
        else if(alive){
            if (!gameLogic._torretas.Contains(gameObject)) {
                gameLogic._torretas.Add(gameObject);
            }

            if (health <= 0)
                alive = false;

            _zombiesInArea.RemoveAll(IsNotAlive);
            if (!gameLogic.eventManager.onEvent) {
                if (!gameLogic.isPaused) {
                    if(CheckForMutanks()==null)
                    closestZombie = GetClosestZombie(_zombiesInArea);
                    else {
                        closestZombie = CheckForMutanks();
                    }
                }
                if (closestZombie != null) {
                    contadorTiempoAtaque += Time.deltaTime;
                    if (contadorTiempoAtaque > attackSpeed) {
                        //aqui empieza a atacar
                        attacking = true;
                        contadorTiempoAtaque = 0;
                        closestZombie.GetComponent<ZombieScript>().health -= attackDamage;
                    }
                }else {
                    contadorTiempoAtaque = 0;
                    attacking = false;
                }
            }
        }else {
            if (!hasChanged) {
                hasChanged = true;
                Cañon.transform.rotation = ObjetoPosicionRoto.transform.rotation;
                Cañon.transform.position = ObjetoPosicionRoto.transform.position;
            }

        }
        //gestion del sistema de particulas
        if (attacking) {
            shoot.Play();
        }
        else {
            StopParticles();
        }
    }

    private void StopParticles() {
        if(shoot.particleCount == 0) {
            shoot.Stop();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Zn") {
            if (_zombiesInArea.Contains(other.gameObject)) {
                _zombiesInArea.Remove(other.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Zn") {
            Debug.Log("A");
            if (!_zombiesInArea.Contains(other.gameObject)) {
                Debug.Log("B");
                _zombiesInArea.Add(other.gameObject);
            }
        }   
    }
    private void OnDestroy() {
        if (gameLogic._barricadas.Contains(gameObject))
            gameLogic._barricadas.Remove(gameObject);
        _targetingZombies.RemoveAll(IsNotAlive);
        foreach (GameObject z in _targetingZombies) {
            z.GetComponent<ZombieScript>().turretToAttack = null;
        }

    }

}
