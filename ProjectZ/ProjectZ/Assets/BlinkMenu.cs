using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkMenu : MonoBehaviour {

    private Image img;
    private float timer;
    private bool active;

	// Use this for initialization
	void Start () {
        active = true;
        timer = 0;
        img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
        if(timer > 0.5f){
            active = !active;
            img.enabled = active;
            timer = 0;
        }
        timer += Time.deltaTime;

        if (Input.anyKey) Destroy(gameObject);
	}
}
