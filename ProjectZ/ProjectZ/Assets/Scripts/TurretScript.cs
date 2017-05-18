using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour {
	GameLogicScript gameLogic;
	public GameObject Canon;

	public enum humanClass { Turret }
	public VisionRangeScript laVision;
	AttackRangeScript elAtaque;
	public bool moving = false;
	public Vector3 targetPosition;
	public float movementLinearSpeed;
	public int health;
	public int attack;
	public int defense;
	public float movSpeed;
	public float attackSpeed;
	public float theAttackRange;
	public float distanciaAlerta;

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

	List<GameObject> _nearbyPartners;
    List<GameObject> _targetigZombies;
	public GameObject patrolPointObject;
	VillagerMovement villagerMovement;
	VillagerAttack villagerAttack;
	private float attackTime;

	private GameObject Latas;
	// Use this for initialization

	void Start()
	{
		distanciaAlerta = 20;
		gameLogic = GameLogicScript.gameLogic;
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

		Renderer render = this.gameObject.GetComponentInChildren<Renderer>();


		switch (tipo)
		{
		case humanClass.Turret:
			theAttackRange = 6;
			health = 100;
			attack = 15;
			defense = 20;
			attackSpeed = 2.5f;
			movSpeed = 0;
			render.material.color += Color.red;
			break;
		}
		if (patrolPointObject == null)
		{
			switch (patrolType)
			{
			case 0:
				patrolPoint = originalPos + new Vector3(3, 0, 0);
				break;
			case 1:
				patrolPoint = originalPos + new Vector3(0, 3, 0);
				break;
			}

		}
		else {
			patrolPoint = patrolPointObject.gameObject.transform.position;
		}
	}
	bool CheckAlive() {
		if (isAlive)
		{
			if (health <= 0)
			{
				isAlive = false;
			}
		}
		return isAlive;
	}
	void heightCheck()
	{
		if (gameObject.transform.position.y > originalPos.y)
		{

			gameObject.transform.position = groundPos;

		}


	}
	void Patrol() {

		if (!goingToPat && !goingBack) {
			goingToPat = true;
		}

		if (goingToPat)
		{
			villagerMovement.MoveTo(patrolPoint);

			if ((patrolPoint-gameObject.transform.position).magnitude < 0.3f) {
				goingToPat = false;
				goingBack = true;
			}


		}
		else if (goingBack) {
			villagerMovement.MoveTo(originalPos);
			if ((originalPos - gameObject.transform.position).magnitude < 0.3f)
			{
				goingToPat = true;
				goingBack = false;
			}

		}


	}
	void Update()
	{

		if (!gameLogic.isPaused&&!gameLogic.eventManager.onEvent)
		{
			groundPos.x = transform.position.x;
			groundPos.z = transform.position.z;
			heightCheck();
			confirmAlive = CheckAlive();
			if (confirmAlive)
			{
				if (patrolPointObject != null && patrolPoint != patrolPointObject.transform.position)
					patrolPoint = patrolPointObject.transform.position;


				if (laVision.enemyInSight)
				{
					alerted = true;
					freeRoam = false;

					if (canMove && laVision.closestZombie != null)
					{
						villagerMovement.MoveTo(laVision.closestZombie.transform.position);
					}
				}
				if (elAtaque.enemyInRange)
				{
					canMove = false;
					villagerAttack.Attack(laVision.closestZombie);
					villagerMovement.moving = false;
					// AttackEnemy();
				}
				else if (!laVision.enemyInSight && !goingToCheck)
				{
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
					if (gameLogic.CalcularDistancia(gameObject, t) < distanciaAlerta && alerted&&!hasAlerted)
					{
						t.GetComponent<VillagerScript>().heardSomething(gameObject.transform.position);
						hasAlerted = true;
					}
				}
			}
			else
			{
				gameObject.SetActive(false);
				Destroy(gameObject, 0.3f);
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
