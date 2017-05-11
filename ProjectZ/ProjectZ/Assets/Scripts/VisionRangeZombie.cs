using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VisionRangeZombie : MonoBehaviour
{
    GameLogicScript gameLogic;

    public bool enemyInSight = false;
    public GameObject anEnemyToAdd;
    public List<GameObject> _enemiesInRange;
    public GameObject closestEnemy;
    public bool hasCheckedFirst = false;

    void Start()
    {
        gameLogic = GameLogicScript.gameLogic;
        _enemiesInRange = new List<GameObject>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            anEnemyToAdd = col.gameObject;
            _enemiesInRange.Add(anEnemyToAdd);

            enemyInSight = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            _enemiesInRange.Remove(col.gameObject);
            if (_enemiesInRange.Count == 0)
            {
                enemyInSight = false;
            }
        }
    }

    GameObject GetClosestEnemy(List<GameObject> enemies)
    {
        if (enemies.Count > 0)
        {
            GameObject tMin = null;
            float minDist = Mathf.Infinity;
            Vector3 currentPos = transform.position;
            foreach (GameObject t in enemies)
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
        else
        {
            return null;
        }
    }

    bool IsNotAlive(GameObject z)
    {
        return !z.GetComponent<VillagerScript>().isAlive;
    }

    void CheckEnemyAlive()
    {
        _enemiesInRange.RemoveAll(IsNotAlive);
    }
    void Update()
    {

        foreach(GameObject g in _enemiesInRange)
        {
            if (g == null)
            {
                _enemiesInRange.Remove(g);
            }
        }

        if (gameLogic.eventManager != null)
        {
            if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            if (_enemiesInRange.Count > 0)
            {
                CheckEnemyAlive();
                closestEnemy = GetClosestEnemy(_enemiesInRange);
            }
            else
            {
                gameObject.GetComponentInParent<ZombieScript>().ResetStuff("noEnemies");
                hasCheckedFirst = false;
                closestEnemy = null;
            }
        } }
    }
}

