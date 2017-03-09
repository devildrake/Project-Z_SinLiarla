using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaTutorial : MonoBehaviour {
    public bool steppedOn;
    SpriteRenderer circulo;
	// Use this for initialization
	void Start () {
        circulo = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (steppedOn) {
            circulo.color = Color.green;

        }
	}

    public void DestroyThis() {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zn") {
            steppedOn = true;
        }
    }

}
