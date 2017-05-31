using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour {
    bool hasBeenSet = false;
	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Slider>().onValueChanged.AddListener(delegate { Setvolume(gameObject.GetComponent<Slider>().value); });
    }
    void Setvolume(float val) {
        if (MusicManager.Instance != null) {
            if (gameObject.tag == "VolumeSlider")
                //MusicManager.Instance.Volume = val;
                if (FindObjectOfType<CameraScript>() != null)
                    AudioListener.volume = val;
                else if (gameObject.tag == "SfxSlider") {
                    AudioListener.volume = val;
                    //MusicManager.Instance.SFXVolume = val;
                }
        }
    }
    // Update is called once per frame
    void Update () {
        if (gameObject.tag == "VolumeSlider" && !hasBeenSet) {
            if (FindObjectOfType<SaverScript>() != null) {
                if (FindObjectOfType<SaverScript>().hasLoaded) {
                    hasBeenSet = true;
                    gameObject.GetComponent<Slider>().value = FindObjectOfType<SaverScript>().savedVolume;
                }
            }
        }
        else if (gameObject.tag == "SfxSlider" && !hasBeenSet) {
            if (FindObjectOfType<SaverScript>() != null) { 
            if (FindObjectOfType<SaverScript>().hasLoaded) {
                hasBeenSet = true;
                gameObject.GetComponent<Slider>().value = FindObjectOfType<SaverScript>().savedSFXVolume;
            }
        }
        }	
	}
}
