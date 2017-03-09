using UnityEngine;
using System.Collections;

public class CameraTryScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = GetComponent<Camera>().ScreenPointToRay(new Vector3(50, 150, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
        Debug.Log("ShouldDraw");
    }
}
