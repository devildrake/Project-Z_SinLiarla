﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VillagerScript : MonoBehaviour
{
    GameLogicScript gameLogic;
    Animator elAnimator;
    public enum humanClass {villager, soldier}
    public VisionRangeScript laVision;
    AttackRangeScript elAtaque;
    public Vector3 targetPosition;
    public float movementLinearSpeed;
    public int health;
    public int maxHealth;
    public int attack;
    public int defense;
    public float movSpeed;
    public float attackSpeed;
    public float theAttackRange;
    public float distanciaAlerta;
    public bool runAway;
    public bool isAlive;
    public bool hasTransformed;
    public bool canMove;
    public bool goingToPat;
    public bool goingBack;
    public bool goingToCheck;

    //array que guarda todos los materiales para los quads de feedback
    //estos se cargan desde el inspector.
    //Orden--> {escapar, alerta soldado, hostil soldado, torreta activa, torreta inutilizada}
    public Material[] feedbackMaterials = new Material[5];

    bool confirmAlive;
    public humanClass tipo;

    public Vector3 groundPos;
    public Vector3 runAWayPoint;
    public GameObject runAwayObject;
    public Vector3 originalPos;
    public Vector3 patrolPoint;
    public bool freeRoam;
    public int patrolType;
    public bool hasAlerted;
    public bool alerted;
    float initSpeedAn;
    float prevsfxv;

    public AudioClip[] audioClip; //array de soniditos

    public GameObject patrolPointObject;
    VillagerMovement villagerMovement;
    VillagerAttack villagerAttack;
    private float attackTime;

    public GameObject quadFeedback;

    // Use this for initialization
    void Start(){

  
        if(GameObject.FindGameObjectWithTag("RunAwayPoint")!=null)
        {
            runAWayPoint = GameObject.FindGameObjectWithTag("RunAwayPoint").transform.position;
        }
        else if (runAwayObject == null) {
            runAWayPoint = new Vector3(0, 0, 0);
        }else {
            runAWayPoint = runAwayObject.transform.position;
        }

        distanciaAlerta = 20;
        gameLogic = GameLogicScript.gameLogic;
        runAway = false;
        elAnimator = gameObject.GetComponent<Animator>();
        elAnimator.SetBool("moviendose", false);
        hasAlerted = alerted = false;
        freeRoam = true;
        goingToCheck = false;
        originalPos = transform.position;
        groundPos.y = originalPos.y;
        confirmAlive = isAlive = true;
        laVision = GetComponentInChildren<VisionRangeScript>();
        elAtaque = GetComponentInChildren<AttackRangeScript>();
        villagerMovement = GetComponent<VillagerMovement>();
        villagerAttack = GetComponent<VillagerAttack>();
        hasTransformed = false;
        initSpeedAn = elAnimator.speed;
        GetComponent<AudioSource>().volume = 0.2f;

        canMove = true;
        switch (tipo){
            case humanClass.villager:
                theAttackRange = 1;
                health = 100;
                attack = 0;
                defense = 10;
                attackSpeed = 0.5f;
                movSpeed = Random.Range(0.8f,1.2f);
                //render.material.color += Color.yellow;
                break;
            case humanClass.soldier:
                theAttackRange = 3;
                health = 100;
                attack = 15;
                defense = 10;
                attackSpeed = 1.5f;
                movSpeed = 2;
                //render.material.color += Color.green;
                break;
        }

        maxHealth = health;

        if (patrolPointObject == null){
            switch (patrolType){
                case 0:
                    patrolPoint = originalPos + new Vector3(3, 0, 0);
                    break;
                case 1:
                    patrolPoint = originalPos + new Vector3(0, 3, 0);
                    break;
            }
        }
        else{
            patrolPoint = patrolPointObject.gameObject.transform.position;
        }
    }

    //public void UpdateVolume(float sfxv) {
    //    GetComponent<AudioSource>().volume = sfxv;
    //}

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


    void heightCheck(){
        if (gameObject.transform.position.y > originalPos.y){
            gameObject.transform.position = groundPos;
        }
    }

    void PlaySound(int clip)//funcion para hacer sonar el ruido
    {
        GetComponent<AudioSource>().clip = audioClip[clip];
        GetComponent<AudioSource>().Play();
    }

    public void gunNoises()
    {
        if (!GetComponent<AudioSource>().isPlaying)
            PlaySound(1);//play groan 1

    }

    void Patrol()
    {

        if (!goingToPat && !goingBack)
        {
            goingToPat = true;
        }

        if (goingToPat)
        {
            villagerMovement.MoveTo(patrolPoint);

            if (villagerMovement.hasArrived)
            {
                villagerMovement.hasArrived = false;
                villagerMovement.puntoActual = 0;
                goingToPat = false;
                goingBack = true;
            }

            if ((patrolPoint - gameObject.transform.position).magnitude < 0.3f)
            {
                goingToPat = false;
                goingBack = true;
            }


        }
        else if (goingBack)
        {
            villagerMovement.MoveTo(originalPos);

            if (villagerMovement.hasArrived)
            {
                villagerMovement.puntoActual = 0;
                villagerMovement.hasArrived = false;
                goingToPat = true;
                goingBack = false;
            }
            if ((originalPos - gameObject.transform.position).magnitude < 0.3f)
            {
                goingToPat = true;
                goingBack = false;
            }
        }
    }
    bool isCloseEnough(GameObject a, float lindar) {
        if (Mathf.Abs((a.transform.position - gameObject.transform.position).magnitude) < lindar) {
            return true;
        }
        else return false;
    }


    public void LookTowards(Vector3 where)
    {
        gameObject.transform.LookAt(where);
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
    }

    void Update() {
        //controla que solo aparezca el sprite de feedback cuando esta escapando el villager
        if (tipo == humanClass.villager) {
            quadFeedback.GetComponent<Renderer>().material = feedbackMaterials[0];
            if (!runAway) {
                quadFeedback.SetActive(false);
            } else {
                quadFeedback.SetActive(true);
            }

            if (health != maxHealth) {
                canMove = false;
                elAnimator.SetBool("isHit", true);

            }


        }
        // controla los sprites de feedback del soldier
        else if (tipo == humanClass.soldier)
        {
            if (quadFeedback != null) {
                if (gameObject.GetComponent<VillagerAttack>().attacking) {
                    GetComponentInParent<VillagerMovement>().LookTowards(gameObject.GetComponent<VillagerAttack>().zombieToAttack.transform.position);
                    quadFeedback.SetActive(true);
                    quadFeedback.GetComponent<Renderer>().material = feedbackMaterials[2];
                }
                else if (goingToCheck) {
                    quadFeedback.SetActive(true);
                    quadFeedback.GetComponent<Renderer>().material = feedbackMaterials[1];
                }

                else {
                    quadFeedback.SetActive(false);
                }
            }
        }

        elAnimator.SetBool("moviendose", villagerMovement.moving);
        if (!villagerMovement.moving)
        {
            elAnimator.SetBool("correr", villagerMovement.moving);
        }
        if (!gameLogic._villagers.Contains(gameObject) && confirmAlive)
        {
            gameLogic._villagers.Add(gameObject);
        }
        if (gameLogic.eventManager != null)
        {
            if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
            {

            if (elAnimator.speed == 0)
            {
                elAnimator.speed = initSpeedAn;
            }


            groundPos.x = transform.position.x;
            groundPos.z = transform.position.z;
            heightCheck();
            confirmAlive = CheckAlive();

            if (confirmAlive)
            {

                    if (runAway&&gameLogic._bases[0]!=null&&runAWayPoint!= new Vector3(0,0,0))
                {

                        if (!isCloseEnough (gameLogic._bases[0].GetComponent<EdificioCreaSoldiers>().spawnPointObject,0.2f))
                    {
                        movSpeed = 1.2f;
                        goingToPat = false;
                        villagerMovement.MoveTo(runAWayPoint);
                        elAnimator.SetBool("correr", true);
                        freeRoam = false;


                        if (tipo == humanClass.villager)
                        {
                            if (health != maxHealth)
                            {
                                canMove = false;
                                elAnimator.SetBool("isHit", true);
                                    movSpeed = 0;
                                elAnimator.SetBool("moviendose", false);
                                elAnimator.SetBool("correr", false);
                            }
                        }
                    }
                }
                else {
                        if (patrolPointObject != null) {
                            if(patrolPoint != patrolPointObject.transform.position)
                                patrolPoint = patrolPointObject.transform.position;
                        }

                        if (laVision != null) {
                            if (laVision.enemyInSight) {
                                if (tipo == humanClass.soldier) {
                                    alerted = true;
                                    freeRoam = false;
                                    elAnimator.SetBool("correr", true);
                                    if (canMove && laVision.closestZombie != null) {
                                        villagerMovement.MoveTo(laVision.closestZombie.transform.position);
                                    }
                                }
                                else if (tipo == humanClass.villager) {
                                    runAway = true;
                                }
                            }
                        }
                    if (tipo == humanClass.villager)
                    {
                        if (health != maxHealth)
                        {
                            canMove = false;
                            elAnimator.SetBool("isHit", true);
                            elAnimator.SetBool("moviendose", false);
                            elAnimator.SetBool("correr", false);

                        }
                    }
                        if (elAtaque != null) {
                            if (elAtaque.enemyInRange && tipo == humanClass.soldier) {
                                canMove = false;
                                elAnimator.SetBool("correr", false);
                                elAnimator.SetBool("atacando", true);
                                //PlaySound(1);
                                villagerAttack.Attack(laVision.closestZombie);
                                villagerMovement.moving = false;
                                // AttackEnemy();
                            }
                            else if (!laVision.enemyInSight && !goingToCheck && tipo == humanClass.soldier) {
                                elAnimator.SetBool("correr", false);
                                elAnimator.SetBool("moviendose", true);
                                freeRoam = true;
                                canMove = true;
                            }
                        }

                    if (canMove && freeRoam && !goingToCheck)
                    {
                        Patrol();

                        //   villagerMovement.MoveTo(patrolPoint);
                    }

                    foreach (GameObject t in gameLogic._villagers)
                    {
                        if (gameLogic.CalcularDistancia(gameObject, t) < distanciaAlerta && alerted && !hasAlerted)
                        {
                            t.GetComponent<VillagerScript>().heardSomething(gameObject.transform.position);
                            hasAlerted = true;
                        }
                    }
                }
            }
            else
            {
                gameObject.SetActive(false);
                PlaySound(0);
                Destroy(gameObject, 3.0f);
            }
        }
        }
        if (gameLogic.eventManager != null)
        {
            if (gameLogic.isPaused || gameLogic.eventManager.onEvent)
            {
                elAnimator.speed = 0;
            }
        }
    }

    public void heardSomething(Vector3 somewhere)
    {
        freeRoam = false;
        goingToPat = false;
        goingBack = false;
        goingToCheck = true;
        villagerMovement.MoveTo(somewhere);
    }
}
