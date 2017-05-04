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
    public AudioClip boom, beep;
    GameLogicScript gameLogic;

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
                aud.clip = boom;
                exp.Play();
                aud.Play();
                Invoke("StopExplosion", exp.main.duration);
                Invoke("StopBoom", 2.0f);
            }
        }
    }

    void Activate(){
        if (!gameLogic.eventManager.onEvent)
        {
            if (!gameLogic.isPaused)
            {
                aud.Play();

                if (!hasExploded&&aud.clip!=null)
                {
                    Invoke("Explode", aud.clip.length);
                    hasExploded = true;
                }
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
                if (!aud.isPlaying)
                    aud.UnPause();

                if (detonate && !hasBeenActive)
                {
                    Activate();
                    hasBeenActive = true;
                }

                detonate = false;
                playing = aud.isPlaying;
            }
            else {
                aud.Pause();
            }
        }
    }
    }





}
