using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaEffect : MonoBehaviour {
    public List<GameObject> zombiesEnRango;
    private MinaActivacion activacion;
    public float mineDmg;
	// Use this for initialization
	void Start () {
        activacion = gameObject.GetComponentInChildren<MinaActivacion>();
	}

    void Dissappear() {
        Destroy(gameObject);
    }

	// Update is called once per frame
	void Update () {
        if (activacion.blown) {
            foreach (GameObject z in zombiesEnRango) {
                if (z.GetComponent<ZombieScript>().tipo != ZombieScript.zombieClass.runner)
                {
                    z.GetComponent<ZombieScript>().health -= mineDmg;
                }
                else {
                    Debug.Log("Dat Runner Got hurt Gurl");
                }
            }
            Invoke("Dissappear",0.5f);
        }
	}

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Zn") {
            zombiesEnRango.Add(other.gameObject);
        }
    } 
}
