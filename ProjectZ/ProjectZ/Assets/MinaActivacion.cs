using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaActivacion : MonoBehaviour {
    private bool active;
    public bool blown;
    public float timeToExplode;
    public float counter;
	// Use this for initialization
	void Start () {
        active = false;
        blown = false;
        counter = 0;
        timeToExplode = 1f;
	}

    // Update is called once per frame
    void Update() {
        if (active) {
            counter += Time.deltaTime;
            if (counter >= timeToExplode) {
                blown = true;
            }
        }
	}
}
