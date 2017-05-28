using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderScript : MonoBehaviour {
    bool hasBeenSet = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (gameObject.tag == "VolumeSlider" && !hasBeenSet) {
            if (FindObjectOfType<SaverScript>().hasLoaded) {
                hasBeenSet = true;
                gameObject.GetComponent<Slider>().value = FindObjectOfType<SaverScript>().savedVolume;
            }
        }else if(gameObject.tag == "SfxSlider" && !hasBeenSet) {
            if (FindObjectOfType<SaverScript>().hasLoaded) {
                hasBeenSet = true;
                gameObject.GetComponent<Slider>().value = FindObjectOfType<SaverScript>().savedSFXVolume;
            }
        }	
	}
}
