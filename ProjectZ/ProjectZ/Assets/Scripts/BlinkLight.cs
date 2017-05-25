using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlinkLight : MonoBehaviour {
    private Light l;
    private float time1, time2, startT; //tiempo de intermitencias
    private float count; //tiempo acumulado
    private bool started; //empieza el comportamiento normal, de base tienen un delay para iniciarse

	// Use this for initialization
	void Start () {
        l = GetComponent<Light>();
        time1 = Random.Range(0.4f, 0.6f);
        startT = Random.Range(0.3f, 0.7f);
        time2 = Random.Range(2.0f, 2.5f);
        started = false;
        count = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if (!started) {
            if (startT < count) {
                count = 0;
                started = true;
            }
            count += Time.deltaTime;
        }
        else {
            if (l.enabled) {
                if (time1 < count) {
                    count = 0;
                    l.enabled = false;
                }
                count += Time.deltaTime;
            }
            else {
                if (time2 < count) {
                    count = 0;
                    l.enabled = true;
                }
                count += Time.deltaTime;
            }
        }
	}
}
