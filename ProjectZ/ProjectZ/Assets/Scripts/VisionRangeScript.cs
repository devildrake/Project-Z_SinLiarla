using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionRangeScript : MonoBehaviour {

    public bool enemyInSight = false;
    public GameObject aZombieToAdd;
    public GameObject aZombieToRemove;
    public List<GameObject> _zombiesInRange;
    public GameObject closestZombie;
    public GameLogicScript gameLogic;

    void Start () {
        _zombiesInRange = new List<GameObject>();
        gameLogic = GameLogicScript.gameLogic;


    }

    void OnTriggerEnter(Collider col) {

        if (col.tag == "Zn") {
            aZombieToAdd = col.gameObject;
            _zombiesInRange.Add(aZombieToAdd);

            enemyInSight = true;
        }
    }

    void OnTriggerStay(Collider col) {
        if (col.tag == "Zn")
        {
            if (!_zombiesInRange.Contains(col.gameObject))
            {
                _zombiesInRange.Add(col.gameObject);
            }
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.tag == "Zn")
        {
            aZombieToRemove = col.gameObject;
            _zombiesInRange.Remove(aZombieToRemove);
            if (_zombiesInRange.Count == 0) {
                enemyInSight = false;
            }
        }
    }

    GameObject GetClosestZombie(List<GameObject> zombies)
    {
        GameObject tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;
        foreach (GameObject t in zombies)
        {
            if (t != null)
            {
                float dist = Vector3.Distance(t.transform.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    minDist = dist;
                }
            }
        }
        return tMin;
    }

    bool IsNotAlive(GameObject z)
    {
        if (z != null)
        {
            return !z.GetComponent<ZombieScript>().isAlive;
        }
        else return true;
    }

    void CheckZombieAlive() {
        _zombiesInRange.RemoveAll(IsNotAlive);
        foreach (GameObject zombie in _zombiesInRange)
        {
            if (!zombie.GetComponent<ZombieScript>().isAlive && zombie != null)
            {
                _zombiesInRange.Remove(zombie);
            }
        }
    }
    void Update()
    {
        if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            if (enemyInSight)
            {
                foreach (GameObject unaBase in gameLogic._bases)
                {
                    if ((unaBase.transform.position - gameObject.transform.position).magnitude < gameObject.GetComponentInParent<VillagerScript>().distanciaAlerta)
                        unaBase.GetComponent<EdificioCreaSoldiers>().alert = true;
                }
            }

            if (_zombiesInRange.Count > 0)
            {
                CheckZombieAlive();
                closestZombie = GetClosestZombie(_zombiesInRange);
            }
            else
            {
                closestZombie = null;
            }
        }
    }
}
