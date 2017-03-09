using UnityEngine;
using System.Collections;

public class RaycasterMouse : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Input.GetMouseButtonDown (0)) {
			if (Physics.Raycast (ray, out hit, 100))
				Debug.DrawLine (ray.origin, hit.point);
		}*/

		if (Input.GetMouseButtonDown(0))
		{
			

			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

			if (hit) 
			{
				Debug.Log("Hit " + hitInfo.transform.gameObject.name);
				if (hitInfo.transform.gameObject.tag == "Zn") {
					Debug.Log ("Ouch");
					Static.MoveTo = true;
				} else {
					Static.MoveTo = false;
				}
			} 

		} 
	}
}

	