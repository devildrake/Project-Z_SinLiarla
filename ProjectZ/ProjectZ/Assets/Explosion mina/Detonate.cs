using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : MonoBehaviour {
    private ParticleSystem exp;
    public AudioSource aud;
    public bool playing;
    public bool hasBeenActive;
    public bool hasExploded;
    public bool detonate;
    public bool hasFinished = false;
    public bool hasStartedPlaying;
    public AudioClip boom, beep;
    GameLogicScript gameLogic;
    public float timer = 0;

	// Use this for initialization
	void Start () {
        hasBeenActive = hasExploded = false;
        exp = GetComponentInChildren<ParticleSystem>();
        aud = GetComponent<AudioSource>();
        aud.clip = beep;
        gameLogic = GameLogicScript.gameLogic;
        //boom = Resources.Load<AudioClip>(".sfx/explosion_1");
        //beep = Resources.Load<AudioClip>(".sfx/beeps");
	}

    void StopBoom(){
        aud.Stop();
        gameObject.GetComponentInParent<MinaEffect>().Dissappear();
    }

    void StopExplosion(){
        exp.Stop();
    }

    void Explode(){

        if (!GameLogicScript.gameLogic.eventManager.onEvent)
        {
            if (!GameLogicScript.gameLogic.isPaused)
            {
                if (!hasStartedPlaying) {
                    aud.clip = boom;
                    exp.Play();
                    aud.Play();
                    hasStartedPlaying = true;
                }

                if (timer >= exp.main.duration) {
                    StopExplosion();
                }

                if (timer >= 2.0f) {
                    StopBoom();
                    hasFinished = true;
                }


            }
        }
    }

    void Activate(){
        if (!gameLogic.eventManager.onEvent)
        {
            if (!gameLogic.isPaused)
            {
                if (!hasStartedPlaying&&!hasExploded) {
                    aud.Play();
                    hasStartedPlaying = true;
                }

                if (!hasExploded&&aud.clip!=null&&timer>=aud.clip.length)
                {
                    hasStartedPlaying = false;
                    Explode();
                    hasExploded = true;
                    timer = 0;
                }
            }
            else {
                aud.Pause();
            }
        }
    }


    // Update is called once per frame
    void Update() {
        if (gameLogic.eventManager != null)
        {
            if (!gameLogic.eventManager.onEvent)
        {
            if (!gameLogic.isPaused)
            {
                if (!aud.isPlaying&&playing)
                    aud.UnPause();

                if (detonate)
                {
                            timer += Time.deltaTime;
                            Activate();
                }

                //detonate = false;
                playing = aud.isPlaying;
            }
            else {
                aud.Pause();
            }
        }
    }
    }





}
