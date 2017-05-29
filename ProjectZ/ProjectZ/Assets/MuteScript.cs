using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteScript : MonoBehaviour {
    bool toggler;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Toggle>().onValueChanged.AddListener(delegate { SetMute(toggler); });
    }

    // Update is called once per frame
    void Update () {
        toggler = gameObject.GetComponent<Toggle>().isOn;
	}

    void SetMute(bool m) {
        if (GameLogicScript.gameLogic != null) {
            GameLogicScript.gameLogic.Muted = m;
        }else {
            Debug.Log("Couldn't find GameLogic");
            return;
        }
        if (GameObject.FindGameObjectWithTag("MainCamera") != null) {
            GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioListener>().enabled = m;
        }
        else {
            Debug.Log("Couldn't find Camera");
            return;
        }

    }
}
