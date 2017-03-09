using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ZombieNoBlock : MonoBehaviour {

	private GameObject pj;
	private UnityEngine.AI.NavMeshAgent nav;
	private float distance;
	public float rangoView;
	private GameObject passGameObject;
	
	void Awake()
	{
		if (GameObject.Find("PassGameObject"))
		{
			passGameObject = GameObject.Find("PassGameObject");
		}
		DontDestroyOnLoad(passGameObject);
	}
	// Use this for initialization
	void Start () {
		pj = GameObject.FindWithTag("Player");
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		
		
		distance = Vector3.Distance(pj.transform.position, transform.position);
		RaycastHit hit;
		Ray ray = new Ray(transform.position, transform.forward);
		if (Physics.Raycast(ray, out hit, 100))
		{
			if (hit.collider != null)
			{
				if (hit.collider.gameObject.tag == "Player" || distance <= rangoView)
				{
					transform.LookAt(pj.transform);
					nav.SetDestination(pj.transform.position);
				}
				else if (hit.collider.gameObject.tag == "Player" || distance <= rangoView) 
				{
					transform.LookAt(pj.transform);
					nav.SetDestination(pj.transform.position);
				}
			}
		}
		if (distance <= rangoView) {
			transform.LookAt(pj.transform);
			nav.SetDestination(pj.transform.position);
		} 

		if (distance <= 1){
			//Destroy
		}
	}
}
