using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Este script hace que los quads con los sprites de feedback
//de todos los personajes siempre apunten hacia la camara.

public class Orientacion : MonoBehaviour {

    private Camera cam;
	// Use this for initialization
	void Start () {
        cam = FindObjectOfType<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(cam.transform);
	}
}
