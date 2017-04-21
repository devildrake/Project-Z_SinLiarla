﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VillagerScript : MonoBehaviour
{
    GameLogicScript gameLogic;
    Animator elAnimator;
    public enum humanClass {villager, soldier, turret}
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

    bool confirmAlive;
    public humanClass tipo;

    public Vector3 groundPos;

    public Vector3 originalPos;
    public Vector3 patrolPoint;
    public bool freeRoam;
    public int patrolType;
    public bool hasAlerted;
    public bool alerted;
    float initSpeedAn;


    List<GameObject> _nearbyPartners;
    public GameObject patrolPointObject;
    VillagerMovement villagerMovement;
    VillagerAttack villagerAttack;
    private float attackTime;

    public GameObject quadFeedback;

    // Use this for initialization
    void Start(){
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
        Renderer render = this.gameObject.GetComponentInChildren<Renderer>();
        
        switch (tipo){
            case humanClass.villager:
                theAttackRange = 1;
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 0.5f;
                movSpeed = Random.Range(0.8f,1.2f);
                //render.material.color += Color.yellow;
                break;
            case humanClass.soldier:
                theAttackRange = 3;
                health = 100;
                attack = 10;
                defense = 10;
                attackSpeed = 1.5f;
                movSpeed = 2;
                //render.material.color += Color.green;
                break;
            case humanClass.turret:
                theAttackRange = 6;
                health = 100;
                attack = 20;
                defense = 20;
                attackSpeed = 2.5f;
                movSpeed = 0;
                render.material.color = Color.red;
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
    void Update(){
        if(tipo == humanClass.villager){
            if (!runAway){
                quadFeedback.SetActive(false);
            }else{
                quadFeedback.SetActive(true);
            }
        }

        elAnimator.SetBool("moviendose", villagerMovement.moving);
        if (!villagerMovement.moving)
        {
            elAnimator.SetBool("correr", villagerMovement.moving);
        }



        if (!gameLogic._villagers.Contains(gameObject)&&confirmAlive)
        {
            gameLogic._villagers.Add(gameObject);
        }

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
                if (runAway)
                {
                    if (gameObject.transform.position != gameLogic._bases[0].GetComponent<EdificioCreaSoldiers>().spawnPointObject.transform.position)
                    {

                        movSpeed = 1.2f;
                        goingToPat = false;
                        villagerMovement.MoveTo(gameLogic._bases[0].GetComponent<EdificioCreaSoldiers>().spawnPointObject.transform.position);
                        elAnimator.SetBool("correr", true);
                        freeRoam = false;
                        Debug.Log("Huyendo");


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
                    }
                }
                else { 
                    if (patrolPointObject != null && patrolPoint != patrolPointObject.transform.position)
                        patrolPoint = patrolPointObject.transform.position;



                    if (laVision.enemyInSight)
                    {
                        if (tipo == humanClass.soldier)
                        {
                            alerted = true;
                            freeRoam = false;
                            elAnimator.SetBool("correr", true);

                            if (canMove && laVision.closestZombie != null)
                            {
                                villagerMovement.MoveTo(laVision.closestZombie.transform.position);
                            }
                        }
                        else if (tipo == humanClass.villager)
                        {
                            runAway = true;
                        }
                    }

                    if (tipo == humanClass.villager)
                    {
                        if (health != maxHealth)
                        {
                            canMove = false;
                            elAnimator.SetBool("isHit", true);
                            elAnimator.SetBool("moviendose",false);
                            elAnimator.SetBool("correr", false);

                        }
                    }

                    if (elAtaque.enemyInRange)
                    {
                        canMove = false;
                        elAnimator.SetBool("correr", false);
                        elAnimator.SetBool("atacando", true);

                        villagerAttack.Attack(laVision.closestZombie);
                        villagerMovement.moving = false;
                        // AttackEnemy();
                    }
                    else if (!laVision.enemyInSight && !goingToCheck)
                    {
                        elAnimator.SetBool("correr", false);
                        elAnimator.SetBool("moviendose", true);
                        freeRoam = true;
                        canMove = true;
                    }
                    else
                    {
                        canMove = true;
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
    
            Destroy(gameObject, 3.0f);
        }
        }else{
            elAnimator.speed = 0;
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
