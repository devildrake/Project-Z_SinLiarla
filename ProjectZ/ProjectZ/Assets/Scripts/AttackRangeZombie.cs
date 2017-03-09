using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttackRangeZombie : MonoBehaviour
{
    public bool enemyInRange = false;
    private float attackRange;
    private VisionRangeZombie laVision;
    GameLogicScript gameLogic;

    void Start()
    {
        gameLogic = GameLogicScript.gameLogic;

        laVision = gameObject.GetComponentInParent<VisionRangeZombie>();
    }
    // Update is called once per frame

    bool CheckAttack()
    {
        
        bool a = false;
        if (laVision.closestEnemy != null)
        {
            if ((laVision.closestEnemy.transform.position - gameObject.transform.position).magnitude <= attackRange && laVision.closestEnemy.GetComponent<VillagerScript>().isAlive)
            {

                a = true;
            }
        }
        return a;
    }
    void Update()
    {
        if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            attackRange = gameObject.GetComponentInParent<ZombieScript>().theAttackRange;
            enemyInRange = CheckAttack();
            if (enemyInRange && gameObject.GetComponentInParent<ZombieScript>().canAttack&&gameObject.GetComponentInParent<ZombieScript>().attackToggle)
            {
                gameObject.GetComponentInParent<ZombieAttack>().Attack(laVision.closestEnemy);
            }
        }
    }
}