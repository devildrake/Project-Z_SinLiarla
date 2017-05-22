using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLight : MonoBehaviour {
    private Light l;
    private float time; //tiempo de intermitencias
    private float count; //tiempo acumulado

	// Use this for initialization
	void Start () {
        l = GetComponent<Light>();
        time = Random.Range(0.9f,1.0f);
        count = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (count > time) {
            l.enabled = !l.enabled;
            count = 0;
            time = Random.Range(0.9f, 1.0f);
        }
        count += Time.deltaTime;
	}
}
