using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieScript : MonoBehaviour
{
    //ENUM QUE GESTIONA EL TIPO DE ZOMBIE DEL QUE SE TRATA
    public enum zombieClass { walker, runner, mutank }

    //INSTANCIA DEL ANIMADOR
    Animator elAnimator;

    //VARIABLES RELACIONADAS CON EL CIRCULO DE SELECCION
    private SpriteRenderer elSprite;
    public GameObject elCirculo;

    //INSTANCIA DEL ENUM DE TIPO DE ZOMBIE
    public zombieClass tipo;

    //BOOLEANO QUE GESTIONA SI EL ZOMBIE ESTA VIVO O NO
    public bool isAlive;

    //BOOLEANO QUE GESTIONA SI EL ZOMBIE ESTA EN LA LISTA DE SELECCIONADOS O NO
    public bool isSelected;

    //BOOLEANO QUE GESTIONA SI EL ZOMBIE SE PUEDE MOVER DE FORMA AUTOMATICA
    public bool canMove;

    //BOOLEANO QUE GESTIONA SI EL ZOMBIE PUEDE ATACAR 
    public bool canAttack;

    public bool attackToggle;    //BOOLEANO DEL TOGGLE DE ATAQUE

    public bool goBarricade;    //BOOLEANO QUE GESTIONA SI EL ZOMBIE ESTA YENDO HACIA UNA BARRICADA
    public bool inBuilding;     //BOOLEANO QUE GESTIONA SI EL ZOMBIE SE ENCUENTRA EN UN EDIFICIO
    public bool irCasa;         //BOOLEANO QUE GESTIONA SI EL ZOMBIE ESTA YENDO HACIA UNA CASA
    public bool movingToEnemy; //BOOLEANO QUE GESTIONA SI EL ZOMBIE ESTA YENDO HACIA UN ENEMIGO
    public bool defenseMode;    //BOOLEANO SOLO UTILIZADO POR LOS MUTANKS PARA SABER SI DEBE RECIBIR MENOS DAÑO Y SI DEBE PONERSE EN POSICION DEFENSA
    public bool hasArrived;     //BOOLEANO QUE GESTIONA SI EL ZOMBIE HA LLEGADO A LA POSICION DESEADA
    public int barricadaSpot;   //INTEGER QUE GESTIONA LA POSICION A LA QUE SE DEBE DIRIGIR EL ZOMBIE EN EL ARRAY DE POSICIONES DE LA BARRICADA
    private GameLogicScript gameLogic;  //SINGLETON DEL GAMELOGIC
    public BarricadaScript barricada;   //"PUNTERO" HACIA LA BARRICADA A LA QUE ATACAR
    bool confirmAlive;  //BOOLEANO DE CONFIRMACION DE VIDA
    public float health;//FLOAT DE GESTION DE VIDA
    float prevHealth;   //FLOAT DE GESTIÓN DE VIDA PREVIA
    float timeOutOfCombat; //CONTADOR DEL TIEMPO QUE LLEVA EL MUTNAK SIN RECIBIR DAÑO
    float defenseTime;  //TIEMPO QUE DEBE ESPERAR EL MUTANK ANTES DE DEJAR EL ESTADO DEFENSA
    float contadorAtk;  //CONTADOR DE TIEMPO ENTRE ATAQUES
    public float maxHealth; //VIDA MAXIMA
    public int attack;  //DAÑO 
    public int defense; //DEFENSA
    public float attackSpeed; //VELOCIDAD DE ATAQUE
    public float movSpeed; //VELOCIDAD DE MOVIMIENTO
    public float theAttackRange; //RANGO DE ATAQUE
    float initSpeedAn; //VELOCIDAD INICIAL DE LAS ANIMACIONES
    public Vector3 puntoCasa;   //A_BORRAR
    public Vector3 targetPosition; //POSICION A LA QUE SE QUIERE MOVER EL ZOMBIE
    public Vector3 prevTargetPos;  //POSICION PREVIA A LA QUE SE QUERIA MOVER EL ZOMBIE
    VisionRangeZombie laVision;     //INSTANCIA DEL HIJO QUE GESTIONA EL RANGO DE VISION
    AttackRangeZombie elAtaqueRange;    //INSTANCIA DEL HIJO QUE GESTIONA EL RANGO DE ATAQUE
    ZombieMovement elMovimiento;    //INSTANCIA DEL SCRIPT HERMANO QUE GESTIONA EL MOVIMIENTO
    Vector3 originalPos; //POSICION ORIGINAL DEL ZOMBIE
    Vector3 groundPos;  //VARAIBLE QUE CAMBIA LA POSICION SOBRE LA QUE SE CLICA PARA QUE EL ZOMBIE NO SE META EN EL SUELO NI FLOTE
    Vector3 barricadePlace; //PUNTO DE LA BARRICADA AL QUE IR AL ATACARLA
    public GameObject villagerToAttackOnClick;

    //METODO QUE COMPRUEBA SI EL ZOMBIE SIGUE VIVO
    bool CheckAlive()
    {
        if (gameObject != null)
        {
            if (isAlive)
            {
                if (health <= 0)
                {
                    isAlive = false;
                }
            }
            return isAlive;

        }else
        {
            return false;
        }
    }
    //METODO QUE REINCIA TODOS LOS BOOLEANOS PERTINENTES PARA QUE EL ZOMBIE ATAQUE UNA BARRICADA
    public void attackBarricade(GameObject laBarricada)
    {
        if (laBarricada != null)
        {
            ResetStuff("command");
            goBarricade = true;
            barricada = laBarricada.GetComponentInParent<BarricadaScript>();
        }
    }

    //METODO QUE INICIALIZA LAS VARIABLES, Y MODIFICA LAS STATS DEL ZOMBIE EN FUNCION DE SU TIPO
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
                movSpeed = 1.5f;
                theAttackRange = 1f;
                break;
        }
        maxHealth = health;
    }

    //METODO DE PREVENCION DE ERRORES PARA QUE LOS ZOMBIES NO FLOTEN NI SE HUNDAN
    void heightCheck()
    {
        if (gameObject.transform.position.y > originalPos.y)
        {
            gameObject.transform.position = groundPos;
        }
    }


    //METODO QUE REINCIA LOS BOOLEANOS EN FUNCION DE LA ORDEN RECIBIDA
    public void ResetStuff(string orden)
    {
        //SI EL ZOMBIE RECIBE UNA ORDEN
        if (orden == "command")
        {
            GetComponent<ZombieAttack>().attacking = goBarricade = hasArrived = movingToEnemy = elMovimiento.countedOnce = gameObject.GetComponent<ZombieAttack>().atBarricade = gameObject.GetComponent<ZombieAttack>().atHuman = canAttack = false;
            villagerToAttackOnClick = null;
            elAnimator.SetBool("atacando", false);

        }
        //SI NO QUEAN ENEMIGO
        else if (orden == "NoEnemies")
        {

            GetComponent<ZombieAttack>().attacking = movingToEnemy = elMovimiento.countedOnce = elMovimiento.moving = gameObject.GetComponent<ZombieAttack>().atBarricade = gameObject.GetComponent<ZombieAttack>().atHuman = canAttack = false;
            hasArrived = true;
            elAnimator.SetBool("atacando", false);

            Debug.Log("NoEnemies");
            villagerToAttackOnClick = null;
        }

        //SI DEBEN PARAR DE MOVERSE
        else if (orden == "StopMoving")
        {
            elAnimator.SetBool("atacando", false);

            villagerToAttackOnClick = null;
            //TO DO
        }
    }

    //COMPRUEBA SI HAY REFERENCIAS NULAS, EN CASO DE QUE EL ZOMBIE NO ESTE EN LAS LISTAS, SE INTRODUCE
    void Update()
    {
        if (gameLogic == null) {
            gameLogic = GameLogicScript.gameLogic;
        }

        if (!gameLogic._zombies.Contains(gameObject) && isAlive)
        {
            gameLogic._zombies.Add(gameObject);

        }
        if (gameLogic.eventManager != null)
        {
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

            //ESTA REGION DE AQUI GESTIONA EL COMPORTAMIENTO ESPECIAL DEL MUTANK
            #region comportamiento Mutank
            if (tipo == zombieClass.mutank)
            {

                    Debug.Log(defenseMode);

                //Comportamiento especifico de mutank

                if (prevHealth != health)
                {
                    defenseMode = true;
                       timeOutOfCombat = 0;
                        Debug.Log("Defences up");
                }
                else
                {
                    timeOutOfCombat += Time.deltaTime;
                    if (timeOutOfCombat > defenseTime)
                    {
                            Debug.Log("Defences down");

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
                    timeOutOfCombat = 0;
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
                                elAnimator.SetBool("atacando",true);
                            {
                                contadorAtk += Time.deltaTime;
                            }

                            if (contadorAtk > attackSpeed)
                            {
                                contadorAtk = 0;
                                barricada.loseHp();
                            }
                        }
                        }else
                        {
                            elAnimator.SetBool("atacando", false);
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

            //EN CASO DE QUE LOS ZOMBIES NO TENGAN UNA ORDEN PENDIENTE PUEDEN MOVERSE LIBREMENTE, Y SI HAY ENEMIGOS CERCA Y PUEDEN ATACARLES
            //VAN HACIA ELLOS 
            if (!elMovimiento.wasCommanded)
            {
                canMove = true;
                if (laVision.enemyInSight) {
                    if (!elAtaqueRange.enemyInRange) {
                        if (canAttack) {
                            if (canMove && attackToggle) {
                                if (laVision.closestEnemy != null) {
                                    if (laVision.closestEnemy.transform.position != prevTargetPos) {
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

            //ESTO MODIFICA EL COLOR DEL CIRCULO EN FUNCION DEL PORCENTAJE DE VIDA
            if (health / maxHealth * 100 <= 20)
            {
                elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            else if (health / maxHealth * 100 <= 50)
            {

                elCirculo.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
            }
        } }

        //EN CASO DE ESTAR PAUSADO PONE LA VELOCIDAD DE ANIMACION A 0
        if (gameLogic.eventManager != null)
        {
            if (gameLogic.isPaused || gameLogic.eventManager.onEvent)
            {
                elAnimator.speed = 0;
            }
        }
    }

}
