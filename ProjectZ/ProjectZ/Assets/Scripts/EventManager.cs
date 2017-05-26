﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {
    //Esta clase maneja todos los eventos del juego, tiene un array con los eventos que hay (eventList).
    //Y un booleano que hace las veces de pause pero justo por debajo de este.

    //BOOLEANO TEMPORAL A BORRAR, SOLO ES PARA PRUEBAS DE FUNCIONALIDAD O PARA UN PRIMER EVENTO DEL JUEGO
    public static EventManager eventManager;

    //Referencia al InputHandler
    private InputHandlerScript _input;

    //Booleano que genera la pseudoPausa en todos los demás scripts salvo el del pausado
    public bool onEvent;
    public bool onSpecialEvent;
    //Listado de eventos
    public Assets.Scripts.Evento[] eventList;

    //Número de eventos totales
    int numEvents = 10;

    //Evento actual
    public Assets.Scripts.Evento currentEvent;

    //Texto que se muestra en pantalla
    public Text currentText;

    //Canvas que se activa/desactiva
    public Canvas canvasChild;


    public int language;

    //Imagen un poco transparente blanca
    public RawImage blancoTrans;

    //Struct Evento que mantiene track de si esta ocurriendo, si ha ocurrido, cuantas interacciones tiene, en que interaccion se 
    //Encuentra y que mensajes tiene
    //Esta funcion crea un evento con el numero de mensajes que se pasa como parametro

    void Awake() {
        if (eventManager == null) {
            DontDestroyOnLoad(gameObject);
            eventManager = this;
        }
        else if (eventManager != this) {
            Destroy(gameObject);
        }
    }

    private Assets.Scripts.Evento CrearEvento(int i, Assets.Scripts.Evento.tipoEvento tipo) {
        Assets.Scripts.Evento anEvent = new Assets.Scripts.Evento();
        anEvent.numInteracts = i;
        anEvent.messages = new string[i];
        anEvent.currInteract = 0;
        anEvent.isHappening = anEvent.hasHappened = false;
        anEvent.tipo = tipo;
        return anEvent;
    }

    //Método que activa el evento en la posicion which del array de evento
    public void activateEvent(int which) {
        if (!eventList[which].hasHappened && !eventList[which].isHappening && !onEvent && eventList[which].tipo == Assets.Scripts.Evento.tipoEvento.NORMAL) {
            onEvent = true;
        }
        else if (!eventList[which].hasHappened && !eventList[which].isHappening && !onEvent && eventList[which].tipo == Assets.Scripts.Evento.tipoEvento.ESPECIAL) {
            onSpecialEvent = true;
        }
        currentEvent = eventList[which];
        currentEvent.isHappening = true;
        //currentEvent.currInteract = 0;
    }

    //Método que termina el evento actual de forma interna y externa
    public void endCurrentEvent() {
        if (!currentEvent.hasHappened && onEvent) {
            _input._continue = false;
            onEvent = false;
            currentEvent.currInteract = 0;
            currentEvent.isHappening = false;
            currentEvent.hasHappened = true;


        }
        else if (!currentEvent.hasHappened && onSpecialEvent) {
            _input._continue = false;
            onSpecialEvent = false;
            currentEvent.currInteract = 0;
            currentEvent.isHappening = false;
            currentEvent.hasHappened = true;

        }
    }

    //Metodo que enciende/apaga el canvas en función del parametro bool 
    void setCanvas(bool tellme) {
        canvasChild.gameObject.SetActive(tellme);
    }

    void ResetEvents(int nivel) {
        switch (nivel) {
            case 1:
                eventList[0].hasHappened = false;
                eventList[1].hasHappened = false;
                eventList[2].hasHappened = false;
                eventList[3].hasHappened = false;

                break;
            case 2:
                eventList[4].hasHappened = false;

                break;
            case 3:
                eventList[5].hasHappened = false;

                break;
            case 4:
                eventList[6].hasHappened = false;

                break;
            case 5:
                eventList[7].hasHappened = false;
                break;

        }
    }

    // Use this for initialization
    void Start() {
        _input = FindObjectOfType<InputHandlerScript>();
        canvasChild = gameObject.GetComponentInChildren<Canvas>();
        blancoTrans = gameObject.GetComponentInChildren<RawImage>();
        currentText = gameObject.GetComponentInChildren<Text>();
        eventList = new Assets.Scripts.Evento[numEvents];
        eventList[0] = CrearEvento(5, Assets.Scripts.Evento.tipoEvento.ESPECIAL);
        eventList[1] = CrearEvento(5, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[2] = CrearEvento(2, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[3] = CrearEvento(4, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[4] = CrearEvento(1, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[5] = CrearEvento(1, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[6] = CrearEvento(1, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[7] = CrearEvento(1, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[8] = CrearEvento(1, Assets.Scripts.Evento.tipoEvento.NORMAL);
        eventList[9] = CrearEvento(1, Assets.Scripts.Evento.tipoEvento.NORMAL);

        if (language == 0) {
            eventList[0].messages[0] = "Bueeno.. veamos si esto funciona...";
            eventList[0].messages[1] = "Clica y manten el botón izquierdo del ratón y arrastra para seleccionar los \nzombies en el cuadro de seleccion";
            eventList[0].messages[2] = "Las unidades seleccionadas se indican con el circulo de selección, que tambien indica la vida";
            eventList[0].messages[3] = "Haz click derecho en un lugar para mover a los zombies seleccionados";
            eventList[0].messages[4] = "Mueve al zombie a las tres zonas para continuar..";

            eventList[1].messages[0] = "Los zombies deberían atacar a todo lo que encuentren, pero son estupidos.. por si acaso acercalos";
            eventList[1].messages[1] = "Veamos cuanto tardan mis queridos podridos en acabar con un superviviente..";
            eventList[1].messages[2] = "Y de paso probamos el mutank..";
            eventList[1].messages[3] = "Los mutanks son los zombies con el brazo gigante, son más lentos, pero tambien más resistentes";
            eventList[1].messages[4] = "Y con ese enorme brazo desproporcionado.. no hay quien los confunda";



            eventList[2].messages[0] = "Como puedes ver los humanos se transforman al morir, pero estos solo huyen";
            eventList[2].messages[1] = "Ahora toca probar contra un soldadito, que este se defiende..";

            eventList[3].messages[0] = "Bueno.. parece que todo esta en orden..";
            eventList[3].messages[1] = "Hora de salir de aquí.. rompamos esas barricadas..";
            eventList[3].messages[2] = "Haz click derecho sobre una barricada para que los zombies seleccionados la ataquen";
            eventList[3].messages[3] = "Puedes comprobar la vida que le queda a la barricada haciendo click izquierdo sobre ella";



            eventList[4].messages[0] = "Hmm.. los runners deberian poder pasar ese campo de minas sin problemas";

            eventList[5].messages[0] = "Parece que los runners estan en forma.. heheheh";

            eventList[6].messages[0] = "Los supervivientes le han cogido demasiado cariño a las minas..";

            eventList[7].messages[0] = "Nivel 5";

        }
        else if (language == 1){

            eventList[0].messages[0] = "Soo.. let's try this out";
            eventList[0].messages[1] = "Click, hold and drag the left mouse button in order to select the units \nwithin the green rectangle, you may also left click on a zombie to select it";
            eventList[0].messages[2] = "Selected units are highlighted by a selection circle, which changes its \ncolor depending on the zombie's current health";
            eventList[0].messages[3] = "Right click somewhere to move the selected zombies there";
            eventList[0].messages[4] = "Make that zombie go inside those three highlited areas to continue..";

            eventList[1].messages[0] = "Zombies will attack whatever it is that they have nearby, but just in \ncase, make them go near humans you want to kill, they aren't geniuses..";
            eventList[1].messages[1] = "Let's see how our beloved rottened take to kill a mere survivor";
            eventList[1].messages[2] = "And while we're at it, let's try out mutanks..";
            eventList[1].messages[3] = "Mutanks are those zombies with the gigantic arm, they're pretty slower,\n but also more resistant";
            eventList[1].messages[4] = "And they can use their arm to defend demselves pretty efficiently";



            eventList[2].messages[0] = "As you can see, killed humans turn into more zombies, although survivors\n just run away";
            eventList[2].messages[1] = "Let's try that again with a soldier that can actually defend himself";

            eventList[3].messages[0] = "Well.. seems we're al set up";
            eventList[3].messages[1] = "Let's break down those barricades..";
            eventList[3].messages[2] = "Right click on them to make selected zombies attack them";
            eventList[3].messages[3] = "You can see how much health the barricade still has by left clicking on it";



            eventList[4].messages[0] = "Hmm.. runners should be able to ignite the mines without getting hurt";

            eventList[5].messages[0] = "It looks like runners are in good shape";

            eventList[6].messages[0] = "Survivors really have taken a liking on mines..";

            eventList[7].messages[0] = "Level 5";
        }

        currentText.gameObject.SetActive(true);
        blancoTrans.gameObject.SetActive(true);
        setCanvas(false);
        onEvent = false;
        onSpecialEvent = false;
    }

    void Update() {
        if (GameLogicScript.gameLogic != null) {
            if (!GameLogicScript.gameLogic.isPaused) { 

            if (onEvent && currentEvent.isHappening) {
                currentText.text = currentEvent.messages[currentEvent.currInteract];
                setCanvas(true);

                if (_input._continue) {
                    currentEvent.currInteract++;
                    if (currentEvent.currInteract < currentEvent.numInteracts) {
                        _input._continue = false;
                        }
                        else {
                        endCurrentEvent();
                        _input._continue = false;
                    }
                }
            } else if (onSpecialEvent && currentEvent.isHappening) {


                    currentText.text = currentEvent.messages[currentEvent.currInteract];
                setCanvas(true);

                if (_input._continue) {

                        currentEvent.currInteract++;
                    if (currentEvent.currInteract < currentEvent.numInteracts) {
                        _input._continue = false;

                        }
                        else {
                        endCurrentEvent();
                        _input._continue = false;
                    }
                }
            }




            else {
                setCanvas(false);
            }
            _input._continue = false;
        }
    }
}
}
