using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BotonTransparente : MonoBehaviour {
    // Use this for initialization
    private Button mySelfButton;
	void Start () {
        //gameObject.GetComponent<Button>().onClick.AddListener(delegate { Continue(); });
        mySelfButton = gameObject.GetComponent<Button>();
    }

    void Continue() {
        if (GameLogicScript.gameLogic != null) {
            GameLogicScript.gameLogic.gameObject.GetComponent<InputHandlerScript>().makeContinueTrue();
        }

    }

    // Update is called once per frame
    void Update () {
        if (Input.GetMouseButtonUp(0)) {
            mySelfButton.onClick.AddListener(() => Continue());
        }
    }
}
