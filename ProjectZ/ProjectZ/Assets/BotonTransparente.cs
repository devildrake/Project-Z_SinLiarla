using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonTransparente : MonoBehaviour {
	// Use this for initialization
	void Start () {
            gameObject.GetComponent<Button>().onClick.AddListener(delegate { Continue(); });

    }

    void Continue() {
        if (GameLogicScript.gameLogic != null) {
            GameLogicScript.gameLogic.gameObject.GetComponent<InputHandlerScript>().makeContinueTrue();
        }

    }

    // Update is called once per frame
    void Update () {
		
	}
}
