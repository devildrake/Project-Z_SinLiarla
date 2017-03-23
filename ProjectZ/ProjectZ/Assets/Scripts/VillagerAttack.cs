using UnityEngine;
using System.Collections;

public class VillagerAttack : MonoBehaviour
{

    public bool attacking = false;
    public ZombieScript zombieToAttack;
    public VillagerScript theVillager;
    public AttackRangeScript elRango;
    public float attackTimer = 0;
    public GameLogicScript gameLogic;
    Animator elAnimator;
    private GameObject Latas;
    float initSpeedAn;
    // Use this for initialization
    void Start()
    {
        gameLogic = GameLogicScript.gameLogic;
        elAnimator = gameObject.GetComponent<Animator>();
        zombieToAttack = null;
        theVillager = GetComponent<VillagerScript>();
        elRango = gameObject.GetComponentInChildren<AttackRangeScript>();
        initSpeedAn = elAnimator.speed;
    }

    public void Attack(GameObject aZombie)
    {
        if (aZombie != null)
        {
            if (aZombie.GetComponent<ZombieScript>().isAlive && elRango.enemyInRange)
            {
                attacking = true;
                zombieToAttack = aZombie.GetComponent<ZombieScript>();
            }
            else
            {
                attacking = false;
            }
        }
    }
    // Update is called once per frame
    void takeDamageColor()
    {
        elAnimator.SetBool("isHit", true);
        Component[] renders = zombieToAttack.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer render in renders)
        {
            render.material.color += Color.red;
        }
    }

    void noRed()
    {
        Component[] renders = zombieToAttack.GetComponentsInChildren(typeof(Renderer));
        foreach (Renderer render in renders)
        {
            render.material.color -= Color.red;
            Debug.Log("NoMoreRed?");
        }
    }

    void Update()
    {
        if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            if (elAnimator.speed == 0)
            {
                elAnimator.speed = initSpeedAn;
            }
            if (attacking)
            {
                if (zombieToAttack != null)
                {
                    if (attackTimer < theVillager.attackSpeed)
                    {
                        attackTimer += Time.deltaTime;
                    }
                    else
                    {
                        zombieToAttack.health -= theVillager.attack;
                        attackTimer = 0;

                    }
                }
            }

            if (!elRango.enemyInRange)
            {
                attacking = false;
            }
        }else
        {
            elAnimator.speed = 0;
        }
    }
}
