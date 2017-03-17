using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class EventManager : MonoBehaviour
    {
        //Esta clase maneja todos los eventos del juego, tiene un array con los eventos que hay (eventList).
        //Y un booleano que hace las veces de pause pero justo por debajo de este.

        //BOOLEANO TEMPORAL A BORRAR, SOLO ES PARA PRUEBAS DE FUNCIONALIDAD O PARA UN PRIMER EVENTO DEL JUEGO


        //Referencia al InputHandler
        private InputHandlerScript _input;

        //Booleano que genera la pseudoPausa en todos los demás scripts salvo el del pausado
        public bool onEvent;

        //Listado de eventos
        public Evento[] eventList;

        //Número de eventos totales
        int numEvents = 10;

        //Evento actual
        public Evento currentEvent;

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
        public void SetEvents(bool[] eventitos, int numEvents)
        {
            for (int i = 0; i < numEvents; i++)
            {
                eventList[i].hasHappened = eventitos[i];
            }
        }

        private Evento CrearEvento(int i)
        {
            Evento anEvent = new Evento();
            anEvent.numInteracts = i;
            anEvent.messages = new string[i];
            anEvent.currInteract = 0;
            anEvent.isHappening = anEvent.hasHappened = false;
            return anEvent;
        }

        //Método que activa el evento en la posicion which del array de evento
        public void activateEvent(int which)
        {
            if (!eventList[which].hasHappened && !eventList[which].isHappening && !onEvent)
            {
                onEvent = true;
                currentEvent = eventList[which];
                currentEvent.isHappening = true;
            }
        }

        //Método que termina el evento actual de forma interna y externa
        public void endCurrentEvent()
        {
            if (!currentEvent.hasHappened && onEvent)
            {
                _input._continue = false;
                onEvent = false;
                currentEvent.currInteract = 0;
                currentEvent.isHappening = false;
                currentEvent.hasHappened = true;

            }
        }

        //Metodo que enciende/apaga el canvas en función del parametro bool 
        void setCanvas(bool tellme)
        {
            canvasChild.gameObject.SetActive(tellme);
        }


        // Use this for initialization
        void Start()
        {
            _input = FindObjectOfType<InputHandlerScript>();
            canvasChild = gameObject.GetComponentInChildren<Canvas>();
            blancoTrans = gameObject.GetComponentInChildren<RawImage>();
            currentText = gameObject.GetComponentInChildren<Text>();
            eventList = new Evento[numEvents];
            eventList[0] = CrearEvento(5);
            eventList[1] = CrearEvento(5);
            eventList[2] = CrearEvento(2);
            eventList[3] = CrearEvento(2);
            eventList[4] = CrearEvento(1);
            eventList[5] = CrearEvento(5);
            eventList[6] = CrearEvento(2);
            eventList[7] = CrearEvento(2);
            eventList[8] = CrearEvento(5);
            eventList[9] = CrearEvento(5);

            if (language == 0)
            {
                eventList[0].messages[0] = "Bueeno.. veamos si esto funciona...";
                eventList[0].messages[1] = "Sistemas funcionales con el paciente 0";
                eventList[0].messages[2] = "Hmm, es una lástima que solo esta variante del virus sea la que puedo controlar";
                eventList[0].messages[3] = "Nota mental, para proximos experimentos diseñar sistema de control con antelación";
                eventList[0].messages[4] = "Debería montarme alguna IA para no hablar solo..";

                eventList[1].messages[0] = "Vaaale, parece que el movimiento funciona correctamente";
                eventList[1].messages[1] = "Veamos como de eficiente pueden ser unos zombies controlados contra una persona normal..";
                eventList[1].messages[2] = "Y de paso probamos el mutank..";
                eventList[1].messages[3] = "Como se nota que ese de la izquierda es el mutank.. estúpidamente grandes.. heh heh heh..";
                eventList[1].messages[4] = "Y con ese enorme brazo desproporcionado..";



                eventList[2].messages[0] = "Heh heh heh.. no tenía ninguna posibilidad.. y ahora tengo dos..";
                eventList[2].messages[1] = "Ahora toca probar contra un soldadito";

                eventList[3].messages[0] = "Bueno.. parece que todo esta en orden..";
                eventList[3].messages[1] = "Hora de salir de aquí..";

                eventList[4].messages[0] = "Hmm.. los runners deberian poder pasar ese campo de minas sin problemas";

                eventList[5].messages[0] = "Torretas eeh.. y estas ya tienen a sus operarios.. bueno ahora los mutanks podran lucirse..";
            }
            else
            {

            }

            currentText.gameObject.SetActive(true);
            blancoTrans.gameObject.SetActive(true);
            setCanvas(false);
            onEvent = false;
        }


        void Update()
        {

            if (onEvent && currentEvent.isHappening)
            {
                currentText.text = currentEvent.messages[currentEvent.currInteract];
                setCanvas(true);

                if (_input._continue)
                {
                    currentEvent.currInteract++;
                    if (currentEvent.currInteract < currentEvent.numInteracts)
                    {
                        _input._continue = false;
                    }
                    else
                    {
                        endCurrentEvent();
                    }
                }

            }
            else
            {
                setCanvas(false);
            }
        }
    }
}