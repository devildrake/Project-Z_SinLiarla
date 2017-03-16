using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : MonoBehaviour {
    private ParticleSystem exp;
    private AudioSource aud;
    public bool playing;
    public bool hasBeenActive;
    public bool hasExploded;
    [HideInInspector]public bool detonate;
    [HideInInspector]public AudioClip boom, beep;


	// Use this for initialization
	void Start () {
        hasBeenActive = hasExploded = false;
        exp = GetComponentInChildren<ParticleSystem>();
        aud = GetComponent<AudioSource>();
        boom = Resources.Load<AudioClip>("sfx/explosion_1");
        beep = Resources.Load<AudioClip>("sfx/beeps");
	}

    void StopBoom(){
        Debug.Log("StopSounding");
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
                Debug.Log("Boom");
                aud.clip = boom;
                exp.Play();
                aud.Play();
                Invoke("StopExplosion", exp.main.duration);
                Invoke("StopBoom", 2.0f);
            }
        }
    }

    void Activate(){
        if (!GameLogicScript.gameLogic.eventManager.onEvent)
        {
            if (!GameLogicScript.gameLogic.isPaused)
            {
                aud.Play();
                if (!hasExploded)
                {
                    Invoke("Explode", aud.clip.length);
                    hasExploded = true;
                }
            }
        }
    }


    // Update is called once per frame
    void Update(){

        if (!GameLogicScript.gameLogic.eventManager.onEvent)
        {
            if (!GameLogicScript.gameLogic.isPaused)
            {
                if (!aud.isPlaying)
                    aud.UnPause();
                //if (Input.GetKeyDown(KeyCode.Space))
                //    detonate = true;

                if (detonate && !hasBeenActive)
                {
                    Activate();
                    hasBeenActive = true;
                }

                detonate = false;
                playing = aud.isPlaying;
            }
            else{
                aud.Pause();
            }
        }
    }





}
