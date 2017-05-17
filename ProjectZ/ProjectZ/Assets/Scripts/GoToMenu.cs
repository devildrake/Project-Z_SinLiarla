using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToMenu : MonoBehaviour {
    bool once;
	// Use this for initialization
	void Start () {
        once = false;
	}
	
    void GoMainMenu()
    {
        SceneManager.LoadScene(0);
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
