using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detonate : MonoBehaviour {
    private ParticleSystem exp;
    private AudioSource aud;
    public bool playing;
    [HideInInspector]public bool detonate;
    [HideInInspector]public AudioClip boom, beep;


	// Use this for initialization
	void Start () {
        exp = GetComponent<ParticleSystem>();
        aud = GetComponent<AudioSource>();
        boom = Resources.Load<AudioClip>("sfx/explosion_1");
        beep = Resources.Load<AudioClip>("sfx/beeps");
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    detonate = true;

        if(detonate)
            Activate();

        detonate = false;
        playing = aud.isPlaying;
	}

    void Activate() {
        aud.Play();
        Invoke("Explode", aud.clip.length);
    }

    void Explode(){
        aud.clip = boom;
        exp.Play();
        aud.Play();
        Invoke("StopExplosion", exp.main.duration);
        Invoke("StopBoom", 1.5f);
    }

    void StopBoom(){
        aud.Stop();
        aud.clip = beep;
    }

    void StopExplosion(){
        exp.Stop();
    }
}
