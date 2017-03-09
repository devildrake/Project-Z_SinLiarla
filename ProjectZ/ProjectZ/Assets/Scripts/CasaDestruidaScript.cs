using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasaDestruidaScript : MonoBehaviour {

    public bool[] sitiosBool = new bool[12];
    public Vector3[] sitiosVec = new Vector3[12];
	// Use this for initialization
	void Start () {
      
        sitiosVec[0] = gameObject.transform.position + new Vector3(1f, 0.4f, 0.3f);
        sitiosVec[1] = gameObject.transform.position + new Vector3(1f, 0.4f, 0.7f);
        sitiosVec[2] = gameObject.transform.position + new Vector3(1f, 0.4f, -0.1f);
        sitiosVec[3] = gameObject.transform.position + new Vector3(1, 0.4f, -0.4f);
        sitiosVec[4] = gameObject.transform.position + new Vector3(0.6f, 0.4f, 0.3f);
        sitiosVec[5] = gameObject.transform.position + new Vector3(0.6f, 0.4f, 0.7f);
        sitiosVec[6] = gameObject.transform.position + new Vector3(0.6f, 0.4f, -0.1f);
        sitiosVec[7] = gameObject.transform.position + new Vector3(0.6f, 0.4f, -0.4f);
        sitiosVec[8] = gameObject.transform.position + new Vector3(-0.7f, 0.4f, 0.3f);
        sitiosVec[9] = gameObject.transform.position + new Vector3(-0.7f, 0.4f, 0.7f);
        sitiosVec[10] = gameObject.transform.position + new Vector3(-0.7f, 0.4f, -0.4f);
        sitiosVec[11] = gameObject.transform.position + new Vector3(-0.7f, 0.4f, -0.8f);

        sitiosBool[0] = sitiosBool[1] = sitiosBool[2] = sitiosBool[3] = sitiosBool[4] = sitiosBool[5] = sitiosBool[6] = sitiosBool[7] = sitiosBool[8] = sitiosBool[9] = sitiosBool[10] = sitiosBool[11] = false;


    }

    //Función que devuelve el numero de trues dentro del array de booleanos
    public int CheckTrues()
    {
        int i = 0;
        foreach (bool a in sitiosBool) {
            if (a) {
                i++;
            }
        }
        return i;
    }

    public Vector3 AssignarSitio() {
        Vector3 a = new Vector3(0, 0, 0);
        int i = 0;
        foreach (bool b in sitiosBool) {
            if (!sitiosBool[i])
            {
                a = sitiosVec[i];
                sitiosBool[i] = true;
            }
            else {
                i++;
            }
        }
        return a;
    }

    public void DesasignarSitio(int cual) {
        sitiosBool[cual] = false;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
