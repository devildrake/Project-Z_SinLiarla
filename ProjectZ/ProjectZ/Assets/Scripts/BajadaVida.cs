using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BajadaVida : MonoBehaviour {
    public BarricadaScript laBarricada;

	// Use this for initialization
	void Start () {
        laBarricada = GetComponentInParent<BarricadaScript>();
	}

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Zn") {
            if (!laBarricada._atacantes.Contains(col.gameObject)) {
                laBarricada._atacantes.Add(col.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.tag == "Zn")
        {
            if (laBarricada._atacantes.Contains(col.gameObject))
            {
                laBarricada._atacantes.Remove(col.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
