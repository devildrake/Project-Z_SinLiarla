using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieScript : MonoBehaviour
{

    public enum zombieClass { walker, runner, mutank }
    Animator elAnimator;
    private SpriteRenderer elSprite;
    public GameObject elCirculo;
    public zombieClass tipo;
    public bool isAlive;
    public bool isSelected;
    public bool canMove;
    public bool canAttack;
    public bool attackToggle;
    public bool goBarricade;
    public bool inBuilding;
    public bool irCasa;
    public bool movingToEnemy;
    public bool defenseMode;
    public bool hasArrived;
    public int barricadaSpot;
    private GameLogicScript gameLogic;
    public BarricadaScript barricada;
    bool confirmAlive;
    public float health;
    float prevHealth;
    float healthCounter;
    float defenseTime;
    float contadorAtk;
    public float maxHealth;
    public int attack;
    public int defense;
    public float attackSpeed;
    public float movSpeed;
    public float theAttackRange;
    float initSpeedAn;
    public Vector3 puntoCasa;
    public Vector3 targetPosition;
    public Vector3 prevTargetPos;
    public float movementLinearSpeed;
    VisionRangeZombie laVision;
    AttackRangeZombie elAtaqueRange;
    ZombieMovement elMovimiento;
    Vector3 originalPos;
    Vector3 groundPos;
    Vector3 barricadePlace;

    // Use this for initialization
    bool CheckAlive()
    {
        if (isAlive)
        {
            if (health <= 0)
            {
                isAlive = false;
            }
        }
        return isAlive;
    }

    public void attackBarricade(GameObject laBarricada)
    {
        ResetStuff("command");
        goBarricade = true;
        barricada = laBarricada.GetComponentInParent<BarricadaScript>();
    }

    void Start()
    {
        gameLogic = GameLogicScript.gameLogic;
        if (!gameLogic._zombies.Contains(gameObject))
        {
            gameLogic._zombies.Add(gameObject);
        }
        attackToggle = true;
        defenseMode = goBarricade = hasArrived = inBuilding = false;
        defenseTime = 1.5f;
        elAnimator = gameObject.GetComponent<Animator>();
        elAnimator.SetBool("moviendose", false);

        originalPos = gameObject.transform.position;
        groundPos.y = originalPos.y;

        elMovimiento = gameObject.GetComponent<ZombieMovement>();
        elSprite = GetComponentInChildren<SpriteRenderer>();
        elCirculo = elSprite.gameObject;
        elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        canMove = canAttack = true;
        laVision = gameObject.GetComponentInChildren<VisionRangeZombie>();
        elAtaqueRange = gameObject.GetComponentInChildren<AttackRangeZombie>();
        confirmAlive = isAlive = true;
        initSpeedAn = elAnimator.speed;

        switch (tipo)
        {
            case zombieClass.walker:
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 1f;
                movSpeed = 2f;
                theAttackRange = 0.8f;
                break;
            case zombieClass.runner:
                health = 50;
                attack = 5;
                defense = 10;
                attackSpeed = 1.5f;
                movSpeed = 3f;
                theAttackRange = 0.8f;
                break;
            case zombieClass.mutank:
                elAnimator.SetBool("ModoDefensa", false);
                health = 300;
                attack = 20;
                defense = 10;
                attackSpeed = 0.5f;
                movSpeed = 1f;
                theAttackRange = 1f;
                break;
        }
        maxHealth = health;
    }

    /*public void MoveTo(Vector3 newTargetPosition)
    {
        targetPosition = newTargetPosition;
        moving = true;
    }*/
    // Update is called once per frame
    void heightCheck()
    {
        if (gameObject.transform.position.y > originalPos.y)
        {

            gameObject.transform.position = groundPos;
        }
    }

    public void ResetStuff(string orden)
    {
        if (orden == "command")
        {
            GetComponent<ZombieAttack>().attacking = goBarricade = hasArrived = movingToEnemy = elMovimiento.countedOnce = gameObject.GetComponent<ZombieAttack>().atBarricade = gameObject.GetComponent<ZombieAttack>().atHuman = canAttack = false;
        }
        else if (orden == "NoEnemies")
        {
            GetComponent<ZombieAttack>().attacking = movingToEnemy = elMovimiento.countedOnce = elMovimiento.moving = gameObject.GetComponent<ZombieAttack>().atBarricade = gameObject.GetComponent<ZombieAttack>().atHuman = canAttack = false;
            hasArrived = true;
            Debug.Log("NoEnemies");
        }
        else if (orden == "StopMoving")
        {
            //TODO
        }
    }

    void Update()
    {

        if (!gameLogic._zombies.Contains(gameObject) && isAlive)
        {
            gameLogic._zombies.Add(gameObject);

        }

        if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {

            if (elMovimiento.wasCommanded)
            {
                ResetStuff("command");
            }

            if (elAnimator.speed == 0)
            {
                elAnimator.speed = initSpeedAn;
            }

            #region comportamiento Mutank
            if (tipo == zombieClass.mutank)
            {
                //Comportamiento especifico de mutank

                if (prevHealth != health)
                {
                    defenseMode = false;
                }
                else
                {
                    healthCounter += Time.deltaTime;
                    if (healthCounter > defenseTime)
                    {
                        defenseMode = false;
                    }
                }
                if (defenseMode)
                {
                    elAnimator.SetBool("ModoDefensa", true);
                    defense = 30;
                }
                else
                {
                    elAnimator.SetBool("ModoDefensa", false);
                    defense = 15;
                    healthCounter = 0;
                }
                prevHealth = health;

            }
            #endregion


            if (elMovimiento.moving) //Codigo que pone true el booleano del animador "moviendose" cuando moving es true
            {
                elAnimator.SetBool("spawn", false);
                elAnimator.SetBool("moviendose", true);
            }
            else
            {
                elAnimator.SetBool("moviendose", false);
            }

            groundPos.x = gameObject.transform.position.x;
            groundPos.z = gameObject.transform.position.z;
            heightCheck();
            confirmAlive = CheckAlive();
            if (confirmAlive)
            {
                if (irCasa)
                {
                    elMovimiento.MoveTo(puntoCasa);
                    if (hasArrived)
                    {
                        irCasa = false;
                    }
                }
                else if (goBarricade)
                {
                    if (barricada != null)
                    {
                        if (gameLogic.CalcularDistancia(barricada.gameObject, gameObject) > theAttackRange)
                        {
                            if (!barricada._atacantes.Contains(gameObject))
                            {
                                barricadePlace = barricada.AsignarSitio(gameObject);
                                barricadaSpot = barricada.aPlaceToAssign;
                                barricada._atacantes.Add(gameObject);
                            }
                            elMovimiento.MoveTo(barricadePlace);
                        }
                        else
                        {
                            elMovimiento.moving = false;
                            {
                                contadorAtk += Time.deltaTime;
                            }

                            if (contadorAtk > attackSpeed)
                            {
                                contadorAtk = 0;
                                barricada.loseHp();
                            }
                        }
                    }
                }
                //Código de que hace el zombie normalmente
                if (isSelected)
                {
                    /*Renderer theRenderer = gameObject.GetComponentInChildren<Renderer>();
                    theRenderer.material.color = Color.yellow;*/
                    elCirculo.SetActive(true);
                }
                else
                {
                    elCirculo.SetActive(false);
                }
            }
            else
            {
                elAnimator.SetBool("isAlive", false);
                Destroy(gameObject, 4.0f);
            }

            if (!elMovimiento.wasCommanded)
            {
                canMove = true;
                if (laVision.enemyInSight)
                {
                    if (!elAtaqueRange.enemyInRange)
                    {
                        if (canAttack)
                        {
                            if (canMove && attackToggle)
                            {
                                if (laVision.closestEnemy != null)
                                {

                                    if (laVision.closestEnemy.transform.position != prevTargetPos)
                                    {
                                        movingToEnemy = false;
                                    }
                                    prevTargetPos = laVision.closestEnemy.transform.position;
                                    if (!movingToEnemy)
                                    {

                                        movingToEnemy = true;
                                        elMovimiento.MoveTo(laVision.closestEnemy.transform.position);

                                    }
                                }
                                else
                                {
                                    movingToEnemy = false;
                                    elAnimator.SetBool("atacando", false);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                canMove = false;
                movingToEnemy = false;
                elAnimator.SetBool("atacando", false);
            }
            //color vida
            if (health / maxHealth * 100 <= 20)
            {
                elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (health / maxHealth * 100 <= 50)
            {

                elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        }
        else
        {
            elAnimator.speed = 0;
        }
    }

}
