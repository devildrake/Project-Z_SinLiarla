using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToMenu : MonoBehaviour {
    bool once;
	// Use this for initialization
	void Start () {
        once = false;
	}
	
    void GoMainMenu()
    {
        Application.LoadLevel(0);
    }
    // Update is called once per frame
    void Update()
    {
        if (!once)
        {
            Invoke("GoMainMenu", 5f);
            once = true;
        }
    }
}
