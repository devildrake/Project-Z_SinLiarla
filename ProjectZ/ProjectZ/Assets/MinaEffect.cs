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
    public bool stop;
    GameLogicScript gameLogic;
    public Detonate particleScript;
    
    // Use this for initialization
    void Start () {
        gameLogic = GameLogicScript.gameLogic;
        //particleScript = gameObject.GetComponentInChildren<Detonate>();
        active = false;
        blown = false;
        counter = 0;
        timeToExplode = particleScript.beep.length + 0.2f;
        //Despues de los pitidos de la detonacion 
        //explota con un pequeño margen de error
        //para hacer coincidir las particulas
        rangoActivacion = 0.4f;
        mineDmg = 50;
    }

    public void Dissappear() {
        Destroy(gameObject);
    }

    bool IsNotAlive(GameObject z)
    {
        if (z.GetComponent<ZombieScript>() != null)
        {
            return !z.GetComponent<ZombieScript>().isAlive;
        }
        else
        {
            return !z.GetComponent<VillagerScript>().isAlive;
        }
    }

    // Update is called once per frame
    void Update()
{
        zombiesEnRango.RemoveAll(IsNotAlive);
            if (!gameLogic.eventManager.onEvent){
            if (!stop)
            {
                if (!gameLogic.isPaused)
                {
                    foreach (GameObject z in zombiesEnRango)
                    {
                        if (!active)
                        {
                            if (gameLogic.CalcularDistancia(gameObject, z) <= rangoActivacion)
                            {
                                active = true;
                            }
                        }
                    }
                }
                    if (blown){


                        foreach (GameObject z in zombiesEnRango){
                            if (z.GetComponent<ZombieScript>().tipo != ZombieScript.zombieClass.runner)
                            {
                                z.GetComponent<ZombieScript>().health -= mineDmg;
                            }
                            else
                            {
                                Debug.Log("Dat Runner Got hurt Gurl");
                            }
                        }
                        GetComponent<MeshFilter>().mesh = null;
                        stop = true;
                    }
                    else if (active){
                        particleScript.detonate = true; //Esto activa las particulas y sonidos del otro script.

                        counter += Time.deltaTime;
                        if (counter >= timeToExplode){
                            blown = true;
                        Debug.Log(gameLogic._bases.Count);
                        foreach (GameObject b in gameLogic._bases)
                        {
                            b.GetComponent<EdificioCreaSoldiers>().alert = true;
                        }
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
