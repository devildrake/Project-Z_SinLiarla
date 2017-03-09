using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausaCanvasScript : MonoBehaviour {
    public GameObject menuPausa;
    public GameLogicScript gameLogic;
    public GameObject confirmacionA;
    public GameObject confirmacionB;
    // Use this for initialization
    void Start() {
        gameLogic = GameLogicScript.gameLogic;
        menuPausa = GameObject.FindGameObjectWithTag("Pause");
        menuPausa.SetActive(false);

    }

    public void UnPause() {
        gameLogic.isPaused = false;
    }

	// Update is called once per frame
	void Update () {
        if (gameLogic.isPaused)
        {
            menuPausa.SetActive(true);
        }else
        {
            menuPausa.SetActive(false);
                
        }
	}
}
