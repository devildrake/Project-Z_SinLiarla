using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLookat : MonoBehaviour {

	VisionRangeScript laVision;
	private GameObject canon;
	private GameObject zombies;

	// Use this for initialization
	void Start () {

		canon = GameObject.FindGameObjectWithTag ("Canon");
		zombies = GameObject.FindGameObjectWithTag ("Zn");

	}
	
	// Update is called once per frame
	void Update () {

		Vector3 targetPosition = zombies.transform.position;

		targetPosition.y= transform.position.y;
		//if(laVision.enemyInSight)
			transform.LookAt(targetPosition);


		
	}
}
