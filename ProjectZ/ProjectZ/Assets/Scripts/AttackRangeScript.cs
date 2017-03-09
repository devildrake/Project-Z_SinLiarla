using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRangeScript : MonoBehaviour {
    public bool enemyInRange = false;
    public float attackRange;
    public VisionRangeScript laVision;
    GameLogicScript gameLogic;

    void Start()
    {
        gameLogic = GameLogicScript.gameLogic;
        laVision = gameObject.GetComponentInParent<VisionRangeScript>();
    }
    // Update is called once per frame

    bool CheckAttack() {
        bool a = false;
        foreach (GameObject zombie in laVision._zombiesInRange)
        {
            if (zombie != null)
            {
                if ((laVision.closestZombie.transform.position - gameObject.transform.position).magnitude <= attackRange && zombie != null && laVision.closestZombie.GetComponent<ZombieScript>().isAlive && laVision.closestZombie != null)
                {
                    a = true;
                }
            }
        }
        return a;
    }
    void Update()
    {
        if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            attackRange = gameObject.GetComponentInParent<VillagerScript>().theAttackRange;
            enemyInRange = CheckAttack();
        }
    }
}
  
    

 
        

