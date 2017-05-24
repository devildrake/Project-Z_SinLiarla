using System.Collections;
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

    public void SetEvents(bool[] eventitos, int numEvents) {
        for (int i = 0; i < numEvents; i++) {
            eventList[i].hasHappened = eventitos[i];
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
            Debug.Log("Activando evento normal");
        }
        else if (!eventList[which].hasHappened && !eventList[which].isHappening && !onEvent && eventList[which].tipo == Assets.Scripts.Evento.tipoEvento.ESPECIAL) {
            Debug.Log("Activando evento especial");
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
            Debug.Log("Terminando evento normal");

        }
        else if (!currentEvent.hasHappened && onSpecialEvent) {
            _input._continue = false;
            onSpecialEvent = false;
            currentEvent.currInteract = 0;
            currentEvent.isHappening = false;
            currentEvent.hasHappened = true;
            Debug.Log("Terminando evento especial");

        }
    }

    //Metodo que enciende/apaga el canvas en función del parametro bool 
    void setCanvas(bool tellme) {
        canvasChild.gameObject.SetActive(tellme);
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

        }
        else {

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
                    Debug.Log("EventoNormal");
                currentText.text = currentEvent.messages[currentEvent.currInteract];
                setCanvas(true);

                if (_input._continue) {
                        Debug.Log("_continueNormal");
                    currentEvent.currInteract++;
                        Debug.Log(currentEvent.currInteract);
                    if (currentEvent.currInteract < currentEvent.numInteracts) {
                        _input._continue = false;
                            Debug.Log("ProsigueElTextoNormal");
                        }
                        else {
                        endCurrentEvent();
                        _input._continue = false;
                    }
                }
            } else if (onSpecialEvent && currentEvent.isHappening) {
                    Debug.Log("EventoEspecial");

                    currentText.text = currentEvent.messages[currentEvent.currInteract];
                setCanvas(true);

                if (_input._continue) {
                        Debug.Log("_continueEspecial");

                        currentEvent.currInteract++;
                    if (currentEvent.currInteract < currentEvent.numInteracts) {
                        _input._continue = false;
                            Debug.Log("ProsigueElTextoEspecial");

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
