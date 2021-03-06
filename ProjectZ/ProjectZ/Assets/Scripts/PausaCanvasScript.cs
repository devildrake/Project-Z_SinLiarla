﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausaCanvasScript : MonoBehaviour {
    public GameObject menuPausa;
    public GameLogicScript gameLogic;
    public GameObject confirmacionA;
    public GameObject confirmacionB;
    public GameObject colorGris;
    // Use this for initialization
    void Start() {
        gameLogic = GameLogicScript.gameLogic;
        gameLogic.elPausaScript = this;
        menuPausa = GameObject.FindGameObjectWithTag("Pause");
        menuPausa.SetActive(false);
        colorGris.SetActive(false);

    }

    public void UnPause() {
        gameLogic.isPaused = false;
    }

	// Update is called once per frame
	void Update () {
        if (gameLogic.isPaused)
        {
            menuPausa.SetActive(true);
            colorGris.SetActive(true);
        }else
        {
            confirmacionA.SetActive(false);
            confirmacionB.SetActive(false);
            menuPausa.SetActive(false);
            colorGris.SetActive(false);

        }
    }
}
