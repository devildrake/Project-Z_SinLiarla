using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinaEffect : MonoBehaviour {
    public List<GameObject> zombiesEnRango;
    public float mineDmg;
    public bool active;
    public bool blown;
    public float timeToExplode;
    public float counter;
    public float rangoActivacion;
    GameLogicScript gameLogic;
    private Detonate particleScript;
    
    // Use this for initialization
    void Start () {
        gameLogic = GameLogicScript.gameLogic;
        particleScript = gameObject.GetComponentInChildren<Detonate>();
        active = false;
        blown = false;
        counter = 0;
        timeToExplode = particleScript.beep.length + 0.2f; //Despues de los pitidos de la detonacion explota con un pequeño margen de error para hacer coincidir las particulas
        rangoActivacion = 0.35f;
        mineDmg = 50;
    }

    void Dissappear() {
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update(){
        if (!gameLogic.eventManager.onEvent){
            if (!gameLogic.isPaused){
                foreach (GameObject z in zombiesEnRango){
                    if (!z.GetComponent<ZombieScript>().isAlive || z.GetComponent<ZombieScript>() == null){
                        zombiesEnRango.Remove(z);
                    }
                    else if(!active){
                        if (gameLogic.CalcularDistancia(gameObject, z) <= rangoActivacion){
                            active = true;
                        }
                    }
                }

                if (blown){
                    particleScript.detonate = true; //Esto activa las particulas y sonidos del otro script.
                    foreach (GameObject z in zombiesEnRango){
                        if (z.GetComponent<ZombieScript>().tipo != ZombieScript.zombieClass.runner){
                            z.GetComponent<ZombieScript>().health -= mineDmg;
                        }
                        else{
                            Debug.Log("Dat Runner Got hurt Gurl");
                        }
                    }
                    Dissappear();
                }else if (active){
                    Debug.Log("Time's ticking");

                    counter += Time.deltaTime;
                    if (counter >= timeToExplode){
                        blown = true;
                        Debug.Log("Allahuakbar");
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other){
        if (other.tag == "Zn") {
            zombiesEnRango.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other){
        if (other.tag == "Zn") {
            zombiesEnRango.Remove(other.gameObject);
        }
    }
}
