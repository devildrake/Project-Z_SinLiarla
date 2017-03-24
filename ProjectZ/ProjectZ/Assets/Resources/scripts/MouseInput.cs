using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MouseInput : MonoBehaviour {

    public CanvasScript canvasScript;

    /*
     * Clicked-> detecta cuando se ha clickado el raton en una posicion determinada y lo utilizo para mantener
     *          el bucle de la animacion de click en los botones. 
     */
    private bool contFinished, clicked, finished;
    private Sprite continuar, nuevaPartida, opciones, salir; //Sprites idle de los botones
 
    

	// Use this for initialization
	void Start ()
    {
        canvasScript = FindObjectOfType<CanvasScript>();
        canvasScript.timer = 0;
        contFinished = false;
        clicked = false;
        finished = false; //Controla cuando acaba la animacion de click
        continuar = Resources.Load<Sprite>("Botones/Continuar/idle");
        nuevaPartida = Resources.Load<Sprite>("Botones/Nueva_Partida/idle");
        opciones = Resources.Load<Sprite>("Botones/Opciones/idle");
        salir = Resources.Load<Sprite>("Botones/Salir/idle");

    }
	
	// Update is called once per frame
	void Update ()
    {


        //Debug.Log(Input.mousePosition);
        //Control de logica en el sceneState = 1
        if (canvasScript.sceneState == 1)
        {
            //Raton sobre boton continuar
            //DESHABILITADO
            /*
            if ((Input.mousePosition.x > 1064 && Input.mousePosition.y < 856) && (Input.mousePosition.x < 1619 && Input.mousePosition.y > 720))
            {
                //click en el boton
                if (Input.GetMouseButtonDown(0))
                {
                    clicked = true;
                }

                if(clicked && !finished)
                {
                    finished = canvasScript.ContinuarClickAnim();
                    canvasScript.timer += Time.deltaTime;
                }

                else if (!contFinished)
                {
                    //animacion
                    contFinished = canvasScript.ContinuarSelectedAnim();
                    canvasScript.timer += Time.deltaTime;

                    //los demás botones tienen que estar en idle
                    canvasScript.nuevaPartida.sprite = nuevaPartida;
                    //canvasScript.opciones.sprite = opciones;
                    canvasScript.salir.sprite = salir;
                }
                
            }
            */
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Application.LoadLevel(1); 
            }
            //Raton sobre boton nueva partida
            if ((Input.mousePosition.x > 1048 && Input.mousePosition.y < 660) && (Input.mousePosition.x < 1640 && Input.mousePosition.y > 540))
            {
                if (Input.GetMouseButton(0))
                {
                    clicked = true;
                }
                if(clicked && !finished)
                {
                    finished = canvasScript.NuevaClickAnim();
                    canvasScript.timer += Time.deltaTime;
                }
                if (!contFinished)
                {
                    //animacion
                    contFinished = canvasScript.NuevaPartidaSelectedAnim(); //Llamar a la funcion del selected del boton nueva partida
                    canvasScript.timer += Time.deltaTime;

                    //los demás botones tienen que estar en idle
                    //canvasScript.continuar.sprite = continuar;
                    //canvasScript.opciones.sprite = opciones;
                    canvasScript.salir.sprite = salir;
                }
            }

            //Raton sobre boton opciones
            //BOTON OPCIONES DESHABILIDADO
            /*
            else if ((Input.mousePosition.x > 1115 && Input.mousePosition.y < 480) && (Input.mousePosition.x < 1584 && Input.mousePosition.y > 350))
            {
                if (Input.GetMouseButton(0)) clicked = true;
                if(clicked && !finished)
                {
                    finished = canvasScript.OpcionesClickAnim();
                    canvasScript.timer += Time.deltaTime;
                }
                if (!contFinished)
                {
                    //animacion
                    contFinished = canvasScript.OpcionesSelectedAnim(); //Llamar a la funcion del selected del boton opciones.
                    canvasScript.timer += Time.deltaTime;

                    //los demás botones tienen que estar en idle
                    canvasScript.continuar.sprite = continuar;
                    canvasScript.nuevaPartida.sprite = nuevaPartida;
                    canvasScript.salir.sprite = salir;
                }
            }
            */

            //Raton sobre boton salir
            else if ((Input.mousePosition.x > 1214 && Input.mousePosition.y < 290) && (Input.mousePosition.x < 1505 && Input.mousePosition.y > 180))
            {
                //click sobre el boton
                if (Input.GetMouseButtonDown(0)) clicked = true;
                if (clicked && !finished)
                {
                    finished = canvasScript.SalirClickAnim();
                    canvasScript.timer += Time.deltaTime;
                }
                if (finished) Application.Quit();
                if (!contFinished)
                {
                    //animacion
                    contFinished = canvasScript.SalirSelectedAnim(); //Llamar a la funcion del selected del boton nueva partida
                    canvasScript.timer += Time.deltaTime;

                    //los demás botones tienen que estar en idle
                    //canvasScript.continuar.sprite = continuar;
                    canvasScript.nuevaPartida.sprite = nuevaPartida;
                   // canvasScript.opciones.sprite = opciones;
                }
            }

            else
            {
                canvasScript.timer = 0;
                contFinished = false;
                finished = false;
                clicked = false;

                //canvasScript.continuar.sprite = continuar;
                canvasScript.nuevaPartida.sprite = nuevaPartida;
                //canvasScript.opciones.sprite = opciones;
                canvasScript.salir.sprite = salir;
            }
        }//Final sceneState = 1

        //Control de logica en sceneState = 2 (seleccion de slot de partida)
        else if(canvasScript.sceneState == 2)
        {
            //cursor sobre carpeta 1 cambia el sprite de este a selected
            if((Input.mousePosition.x>75 && Input.mousePosition.x<670) && (Input.mousePosition.y>80 && Input.mousePosition.y < 870))
            {
                canvasScript.partidas[0].GetComponent<Image>().sprite = canvasScript.selected_Partida1;
                canvasScript.partidas[1].GetComponent<Image>().sprite = canvasScript.idle_Partida2;
                canvasScript.partidas[2].GetComponent<Image>().sprite = canvasScript.idle_Partida3;
                //Click sobre el boton
                if (Input.GetMouseButtonDown(0))
                {
                    //TODO comportamiento del boton al pulsar sobre el.
                }
            }
            //cursor sobre carpeta 2 cambia el sprite de este a selected
            else if ((Input.mousePosition.x>720 && Input.mousePosition.x<1195) && (Input.mousePosition.y>340 && Input.mousePosition.y<995))
            {
                canvasScript.partidas[1].GetComponent<Image>().sprite = canvasScript.selected_Partida2;
                canvasScript.partidas[0].GetComponent<Image>().sprite = canvasScript.idle_Partida1;
                canvasScript.partidas[2].GetComponent<Image>().sprite = canvasScript.idle_Partida3;
                //click sobre el boton
                if (Input.GetMouseButtonDown(0))
                {
                    //TODO comportamiento del boton al pulsar sobre el
                }
            }
            //cursor sobre carpeta 3 cambia el sprite de este a selected
            else if ((Input.mousePosition.x>1250 && Input.mousePosition.x<1880) && (Input.mousePosition.y>50 && Input.mousePosition.y < 800))
            {
                canvasScript.partidas[2].GetComponent<Image>().sprite = canvasScript.selected_Partida3;
                canvasScript.partidas[0].GetComponent<Image>().sprite = canvasScript.idle_Partida1;
                canvasScript.partidas[1].GetComponent<Image>().sprite = canvasScript.idle_Partida2;
                //click sobre el boton
                if (Input.GetMouseButtonDown(0))
                {
                    //TODO comportamiento del boton al pulsar sobre el
                }

            }
            else
            {
                canvasScript.partidas[0].GetComponent<Image>().sprite = canvasScript.idle_Partida1;
                canvasScript.partidas[1].GetComponent<Image>().sprite = canvasScript.idle_Partida2;
                canvasScript.partidas[2].GetComponent<Image>().sprite = canvasScript.idle_Partida3;
            }
        }


	}
}
