using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour {
    public GameObject fondo, taza, papeles, carpeta, carpeta_abierta, b1, b2, b3, opcionesBase;
    private enum state {PREV_MENU, MAIN_MENU, OPCIONES };
    private state menuState;


    void Start(){
        carpeta_abierta.SetActive(false);
        b1.SetActive(false);
        b2.SetActive(false);
        b3.SetActive(false);
        opcionesBase.SetActive(false);
        menuState = state.PREV_MENU;
    }

    void Update(){
        //comportamiento cuando esta en la pantalla de "pulsa cualquier tecla"
        if (menuState == state.PREV_MENU) {
            if (Input.anyKey) {
                taza.SetActive(false);
                papeles.SetActive(false);
                carpeta.SetActive(false);
                carpeta_abierta.SetActive(true);
                b1.SetActive(true);
                b2.SetActive(true);
                b3.SetActive(true);
               
                menuState = state.MAIN_MENU;
            }
        }

        //comportamiento dentro del boton opciones
        if(menuState == state.OPCIONES) {
            if (Input.GetKeyDown(KeyCode.Escape)) {
                opcionesBase.SetActive(false);
                menuState = state.MAIN_MENU;
                //cuando se vuelve al menu principal se deben reactivar los botones
                b1.GetComponent<Button>().interactable = true;
                b2.GetComponent<Button>().interactable = true;
                b3.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void Continuar(){
        SceneManager.LoadScene(1);
    }

    public void Opciones() {
        opcionesBase.SetActive(true);
        b1.GetComponent<Button>().interactable = false;
        b2.GetComponent<Button>().interactable = false;
        b3.GetComponent<Button>().interactable = false;
        menuState = state.OPCIONES;
    }


    public void ResetGame() {
        //TODO elimina todo el progreso guardado y reinicia el juego.
    }

    public void SalirApp() {
        Application.Quit();
    }

    public void Back() {
        opcionesBase.SetActive(false);
        menuState = state.MAIN_MENU;
        //cuando se vuelve al menu principal se deben reactivar los botones
        b1.GetComponent<Button>().interactable = true;
        b2.GetComponent<Button>().interactable = true;
        b3.GetComponent<Button>().interactable = true;
    }




    //   public GameObject imageGO; //Game object Image child of canvas
    //   public Sprite firstSprite; //First sprite to show
    //   public Sprite secondSprite; //Second sprite to show
    //   [HideInInspector] public Image continuar, nuevaPartida, opciones, salir;
    //   [HideInInspector] public GameObject[] buts;
    //   [HideInInspector] public float timer;


    //   private Image sceneBg; //Component image of the Game Object image.

    //   /*
    //    * sceneState = 0 --> pulsa para continuar. Primera imagen del menu
    //    * sceneState = 1 --> menú principal. Aparecen los botones continuar, nueva partida, opciones, salir.
    //    * sceneState = 3 --> al pulsar nueva partida aparece la seleccion del slot para guardarla. 
    //    */
    //   [HideInInspector] public int sceneState;
    //   [HideInInspector] public GameObject[] partidas; //Game objects que contienen las imagenes de los slots de partida
    //   [HideInInspector] public Sprite idle_Partida1, idle_Partida2, idle_Partida3,
    //      selected_Partida1, selected_Partida2, selected_Partida3, fondo; //Sprites idle que saldran en el sceneState = 2


    //   public GameObject mouseInputScript;
    //   private Sprite[] continuarSelected; //Sprites de la animacion selected del boton continuar
    //   private Sprite[] nuevaSelected; //Sprites de la animacion selected del boton nueva partida
    //   private Sprite[] opcionesSelected; //Sprites de la animacion selected del boton opciones
    //   private Sprite[] salirSelected; //Sprites de la animacion selected del boton salir
    //   private Sprite[] continuarClick, nuevaClick, opcionesClick, salirClick;



    //   // Use this for initialization
    //   void Start()
    //   {
    //       sceneState = 0;

    //       //Inicializar arrays
    //       buts = new GameObject[4];
    //       continuarSelected = new Sprite[9];
    //       nuevaSelected = new Sprite[10];
    //       opcionesSelected = new Sprite[8];
    //       salirSelected = new Sprite[9];
    //       partidas = new GameObject[3];
    //       continuarClick = new Sprite[18];
    //       nuevaClick = new Sprite[25];
    //       opcionesClick = new Sprite[23];
    //       salirClick = new Sprite[14];

    //       //Cargar imagenes
    //       LoadTextures();

    //       //Se carga la imagen del primer fondo en el objeto correspondiente
    //       imageGO = transform.Find("Image").gameObject;
    //       sceneBg = imageGO.GetComponent<Image>();
    //       sceneBg.sprite = firstSprite;

    //       //Se referencian los 4 botones y sus componentes Image. Se desactivan.
    //       buts[0] = transform.Find("Continue_Button").gameObject;
    //       buts[1] = transform.Find("New_Game_Button").gameObject;
    //       buts[2] = transform.Find("Options_Button").gameObject;
    //       buts[3] = transform.Find("Exit_Button").gameObject;
    //       continuar = buts[0].GetComponent<Image>();
    //       nuevaPartida = buts[1].GetComponent<Image>();
    //       opciones = buts[2].GetComponent<Image>();
    //       salir = buts[3].GetComponent<Image>();
    //       for (int i = 0; i < buts.Length; i++)
    //       {
    //           buts[i].SetActive(false);
    //       }

    //       //Se referencian los tres botones de seleccion de partida y se desactivan.
    //       partidas[0] = GameObject.Find("Partida1");
    //       partidas[1] = GameObject.Find("Partida2");
    //       partidas[2] = GameObject.Find("Partida3");
    //       for (int i = 0; i < partidas.Length; i++)
    //       {
    //           partidas[i].SetActive(false);
    //       }

    //       //Busca el game object con el script que se utiliza para las inputs de raton.
    //       //Se desactiva porque en sceneState = 0 no se necesita.
    //       mouseInputScript = GameObject.Find("Mouse_Input");
    //       mouseInputScript.SetActive(false);
    //   }


    //   public bool ContinuarClickAnim()
    //   {
    //       if (timer <= 0.01f)
    //       {
    //           continuar.sprite = continuarClick[0];
    //           return false;
    //       }
    //       else if (timer <= 0.02f)
    //       {
    //           continuar.sprite = continuarClick[1];
    //           return false;
    //       }
    //       else if (timer <= 0.03f)
    //       {
    //           continuar.sprite = continuarClick[2];
    //           return false;
    //       }
    //       else if (timer <= 0.04f)
    //       {
    //           continuar.sprite = continuarClick[3];
    //           return false;
    //       }
    //       else if (timer <= 0.05f)
    //       {
    //           continuar.sprite = continuarClick[4];
    //           return false;
    //       }
    //       else if (timer <= 0.06f)
    //       {
    //           continuar.sprite = continuarClick[5];
    //           return false;
    //       }
    //       else if (timer <= 0.07f)
    //       {
    //           continuar.sprite = continuarClick[6];
    //           return false;
    //       }
    //       else if (timer <= 0.08f)
    //       {
    //           continuar.sprite = continuarClick[7];
    //           return false;
    //       }
    //       else if (timer <= 0.09f)
    //       {
    //           continuar.sprite = continuarClick[8];
    //           return false;
    //       }
    //       else if (timer <= 0.1f)
    //       {
    //           continuar.sprite = continuarClick[9];
    //           return false;
    //       }
    //       else if (timer <= 0.11f)
    //       {
    //           continuar.sprite = continuarClick[10];
    //           return false;
    //       }
    //       else if (timer <= 0.12f)
    //       {
    //           continuar.sprite = continuarClick[11];
    //           return false;
    //       }
    //       else if (timer <= 0.13f)
    //       {
    //           continuar.sprite = continuarClick[12];
    //           return false;
    //       }
    //       else if (timer <= 0.14f)
    //       {
    //           continuar.sprite = continuarClick[13];
    //           return false;
    //       }
    //       else if (timer <= 0.15f)
    //       {
    //           continuar.sprite = continuarClick[14];
    //           return false;
    //       }
    //       else if (timer <= 0.16f)
    //       {
    //           continuar.sprite = continuarClick[15];
    //           return false;
    //       }
    //       else if (timer <= 0.17)
    //       {
    //           continuar.sprite = continuarClick[16];
    //           return false;
    //       }
    //       else
    //       {
    //           continuar.sprite = continuarClick[17];
    //           return true;
    //       }        
    //   }

    //   public bool NuevaClickAnim()
    //   {
    //       if (timer <= 0.01f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[0];
    //           return false;
    //       }
    //       else if (timer <= 0.02f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[1];
    //           return false;
    //       }
    //       else if (timer <= 0.03f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[2];
    //           return false;
    //       }
    //       else if (timer <= 0.04f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[3];
    //           return false;
    //       }
    //       else if (timer <= 0.05f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[4];
    //           return false;
    //       }
    //       else if (timer <= 0.06f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[5];
    //           return false;
    //       }
    //       else if (timer <= 0.07f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[6];
    //           return false;
    //       }
    //       else if (timer <= 0.08f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[7];
    //           return false;
    //       }
    //       else if (timer <= 0.09f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[8];
    //           return false;
    //       }
    //       else if (timer <= 0.1f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[9];
    //           return false;
    //       }
    //       else if (timer <= 0.11f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[10];
    //           return false;
    //       }
    //       else if (timer <= 0.12f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[11];
    //           return false;
    //       }
    //       else if (timer <= 0.13f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[12];
    //           return false;
    //       }
    //       else if (timer <= 0.14f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[13];
    //           return false;
    //       }
    //       else if (timer <= 0.15f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[14];
    //           return false;
    //       }
    //       else if (timer <= 0.16f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[15];
    //           return false;
    //       }
    //       else if (timer <= 0.17f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[16];
    //           return false;
    //       }
    //       else if(timer <= 0.18f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[17];
    //           return false;
    //       }
    //       else if(timer <= 0.19f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[18];
    //           return false;
    //       }
    //       else if(timer <= 0.2f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[19];
    //           return false;
    //       }
    //       else if(timer <= 0.21f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[20];
    //           return false;
    //       }
    //       else if(timer <= 0.22f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[21];
    //           return false;
    //       }
    //       else if(timer <= 0.23f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[22];
    //           return false;
    //       }
    //       else if(timer <= 0.24f)
    //       {
    //           nuevaPartida.sprite = nuevaClick[23];
    //           return false;
    //       }
    //       else
    //       {
    //           nuevaPartida.sprite = nuevaClick[24];
    //           //SceneState2();
    //           Application.LoadLevel("Tutorial");
    //           return true;
    //       }
    //   }

    //   public bool OpcionesClickAnim()
    //   {
    //       if (timer <= 0.01f)
    //       {
    //           opciones.sprite = opcionesClick[0];
    //           return false;
    //       }
    //       else if (timer <= 0.02f)
    //       {
    //           opciones.sprite = opcionesClick[1];
    //           return false;
    //       }
    //       else if (timer <= 0.03f)
    //       {
    //           opciones.sprite = opcionesClick[2];
    //           return false;
    //       }
    //       else if (timer <= 0.04f)
    //       {
    //           opciones.sprite = opcionesClick[3];
    //           return false;
    //       }
    //       else if (timer <= 0.05f)
    //       {
    //           opciones.sprite = opcionesClick[4];
    //           return false;
    //       }
    //       else if (timer <= 0.06f)
    //       {
    //           opciones.sprite = opcionesClick[5];
    //           return false;
    //       }
    //       else if (timer <= 0.07f)
    //       {
    //           opciones.sprite = opcionesClick[6];
    //           return false;
    //       }
    //       else if (timer <= 0.08f)
    //       {
    //           opciones.sprite = opcionesClick[7];
    //           return false;
    //       }
    //       else if (timer <= 0.09f)
    //       {
    //           opciones.sprite = opcionesClick[8];
    //           return false;
    //       }
    //       else if (timer <= 0.1f)
    //       {
    //           opciones.sprite = opcionesClick[9];
    //           return false;
    //       }
    //       else if (timer <= 0.11f)
    //       {
    //           opciones.sprite = opcionesClick[10];
    //           return false;
    //       }
    //       else if (timer <= 0.12f)
    //       {
    //           opciones.sprite = opcionesClick[11];
    //           return false;
    //       }
    //       else if (timer <= 0.13f)
    //       {
    //           opciones.sprite = opcionesClick[12];
    //           return false;
    //       }
    //       else if (timer <= 0.14f)
    //       {
    //           opciones.sprite = opcionesClick[13];
    //           return false;
    //       }
    //       else if (timer <= 0.15f)
    //       {
    //           opciones.sprite = opcionesClick[14];
    //           return false;
    //       }
    //       else if (timer <= 0.16f)
    //       {
    //           opciones.sprite = opcionesClick[15];
    //           return false;
    //       }
    //       else if (timer <= 0.17f)
    //       {
    //           opciones.sprite = opcionesClick[16];
    //           return false;
    //       }
    //       else if (timer <= 0.18f)
    //       {
    //           opciones.sprite = opcionesClick[17];
    //           return false;
    //       }
    //       else if (timer <= 0.19f)
    //       {
    //           opciones.sprite = opcionesClick[18];
    //           return false;
    //       }
    //       else if (timer <= 0.2f)
    //       {
    //           opciones.sprite = opcionesClick[19];
    //           return false;
    //       }
    //       else if (timer <= 0.21f)
    //       {
    //           opciones.sprite = opcionesClick[20];
    //           return false;
    //       }
    //       else if (timer <= 0.22f)
    //       {
    //           opciones.sprite = opcionesClick[21];
    //           return false;
    //       }
    //       else
    //       {
    //           opciones.sprite = opcionesClick[22];
    //           return true;
    //       }
    //   }

    //   public bool SalirClickAnim()
    //   {
    //       if (timer <= 0.01f)
    //       {
    //           salir.sprite = salirClick[0];
    //           return false;
    //       }
    //       else if (timer <= 0.02f)
    //       {
    //           salir.sprite = salirClick[1];
    //           return false;
    //       }
    //       else if (timer <= 0.03f)
    //       {
    //           salir.sprite = salirClick[2];
    //           return false;
    //       }
    //       else if (timer <= 0.04f)
    //       {
    //           salir.sprite = salirClick[3];
    //           return false;
    //       }
    //       else if (timer <= 0.05f)
    //       {
    //           salir.sprite = salirClick[4];
    //           return false;
    //       }
    //       else if (timer <= 0.06f)
    //       {
    //           salir.sprite = salirClick[5];
    //           return false;
    //       }
    //       else if (timer <= 0.07f)
    //       {
    //           salir.sprite = salirClick[6];
    //           return false;
    //       }
    //       else if (timer <= 0.08f)
    //       {
    //           salir.sprite = salirClick[7];
    //           return false;
    //       }
    //       else if (timer <= 0.09f)
    //       {
    //           salir.sprite = salirClick[8];
    //           return false;
    //       }
    //       else if (timer <= 0.1f)
    //       {
    //           salir.sprite = salirClick[9];
    //           return false;
    //       }
    //       else if (timer <= 0.11f)
    //       {
    //           salir.sprite = salirClick[10];
    //           return false;
    //       }
    //       else if (timer <= 0.12f)
    //       {
    //           salir.sprite = salirClick[11];
    //           return false;
    //       }
    //       else if (timer <= 0.13f)
    //       {
    //           salir.sprite = salirClick[12];
    //           return false;
    //       }
    //       else
    //       {
    //           salir.sprite = salirClick[13];
    //           Application.Quit();
    //           return true;
    //       }
    //   }

    //   public void SceneState2()
    //   {
    //       //Se cambia el estado de la escena
    //       sceneState = 2;
    //       //Se cambia el fondo de la escena
    //       sceneBg.sprite = fondo;
    //       //Se desactivan los botones del menu principal
    //       for (int i = 0; i<buts.Length; i++)
    //       {
    //           buts[i].SetActive(false);
    //       }
    //       //Se añaden los sprites correspondientes a los botones de sceneState = 2
    //       partidas[0].GetComponent<Image>().sprite = idle_Partida1;
    //       partidas[1].GetComponent<Image>().sprite = idle_Partida2;
    //       partidas[2].GetComponent<Image>().sprite = idle_Partida3;
    //       for (int i = 0; i < partidas.Length; i++)
    //       {
    //           partidas[i].GetComponent<Image>().SetNativeSize();
    //       }

    //       //Se activan los botones del menu de seleccion de partida
    //       for (int i = 0; i<partidas.Length; i++)
    //       {
    //           partidas[i].SetActive(true);
    //       }

    //   }

    //   //Animacion del boton continuar cuando se coloca el boton sobre este
    //   public bool OpcionesSelectedAnim()
    //   {
    //       if (timer <= 0.02f)
    //       {
    //           opciones.sprite = opcionesSelected[0];
    //           return false;
    //       }
    //       else if(timer <= 0.04f)
    //       {
    //           opciones.sprite = opcionesSelected[1];
    //           return false;
    //       }
    //       else if(timer <= 0.06f)
    //       {
    //           opciones.sprite = opcionesSelected[2];
    //           return false;
    //       }
    //       else if (timer <= 0.08f)
    //       {
    //           opciones.sprite = opcionesSelected[3];
    //           return false;
    //       }
    //       else if (timer <= 0.1f)
    //       {
    //           opciones.sprite = opcionesSelected[4];
    //           return false;
    //       }
    //       else if (timer <= 0.12f)
    //       {
    //           opciones.sprite = opcionesSelected[5];
    //           return false;
    //       }
    //       else if (timer <= 0.14f)
    //       {
    //           opciones.sprite = opcionesSelected[6];
    //           return false;
    //       }
    //       else
    //       {
    //           opciones.sprite = opcionesSelected[7];
    //           timer = 0;
    //           return true;
    //       }
    //   }

    //   public bool SalirSelectedAnim()
    //   {
    //       if(timer <= 0.02f)
    //       {
    //           salir.sprite = salirSelected[0];
    //           return false;
    //       }
    //       else if(timer <= 0.04f)
    //       {
    //           salir.sprite = salirSelected[1];
    //           return false;
    //       }
    //       else if(timer <= 0.06f)
    //       {
    //           salir.sprite = salirSelected[2];
    //           return false;
    //       }
    //       else if(timer <= 0.08f)
    //       {
    //           salir.sprite = salirSelected[3];
    //           return false;
    //       }
    //       else if (timer <= 0.1f)
    //       {
    //           salir.sprite = salirSelected[4];
    //           return false;
    //       }
    //       else if (timer <= 0.12f)
    //       {
    //           salir.sprite = salirSelected[5];
    //           return false;
    //       }
    //       else if (timer <= 0.14f)
    //       {
    //           salir.sprite = salirSelected[6];
    //           return false;
    //       }
    //       else if (timer <= 0.16f)
    //       {
    //           salir.sprite = salirSelected[7];
    //           return false;
    //       }
    //       else
    //       {
    //           salir.sprite = salirSelected[8];
    //           timer = 0;
    //           return true;
    //       }
    //   }

    //   //Animacion del boton continuar cuando se coloca el cursor sobre el boton
    //   public bool ContinuarSelectedAnim()
    //   {
    //       if (timer <= 0.02f)
    //       {
    //           continuar.sprite = continuarSelected[0];
    //           return false;
    //       }
    //       else if (timer <= 0.04f)
    //       {
    //           continuar.sprite = continuarSelected[1];
    //           return false;
    //       }
    //       else if (timer <= 0.06f)
    //       {
    //           continuar.sprite = continuarSelected[2];
    //           return false;
    //       }
    //       else if(timer <= 0.08f)
    //       {
    //           continuar.sprite = continuarSelected[3];
    //           return false;
    //       }
    //       else if(timer <= 0.1f)
    //       {
    //           continuar.sprite = continuarSelected[4];
    //           return false;
    //       }
    //       else if(timer <= 0.12f)
    //       {
    //           continuar.sprite = continuarSelected[5];
    //           return false;
    //       }
    //       else if (timer <= 0.14f)
    //       {
    //           continuar.sprite = continuarSelected[6];
    //           return false;
    //       }
    //       else if(timer <= 0.16f)
    //       {
    //           continuar.sprite = continuarSelected[7];
    //           return false;
    //       }
    //       else
    //       {
    //           continuar.sprite = continuarSelected[8];
    //           timer = 0;
    //           return true;
    //       }
    //   }

    //   public bool NuevaPartidaSelectedAnim()
    //   {
    //       if (timer <= 0.02f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[0];

    //           return false;
    //       }
    //       else if (timer <= 0.04f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[1];
    //           return false;
    //       }
    //       else if (timer <= 0.06f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[2];
    //           return false;
    //       }
    //       else if (timer <= 0.08f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[3];
    //           return false;
    //       }
    //       else if (timer <= 0.1f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[4];
    //           return false;
    //       }
    //       else if (timer <= 0.12f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[5];
    //           return false;
    //       }
    //       else if(timer <= 0.14f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[6];
    //           return false;
    //       }
    //       else if(timer <= 0.16f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[7];
    //           return false;
    //       }
    //       else if(timer <= 0.18f)
    //       {
    //           nuevaPartida.sprite = nuevaSelected[8];
    //           return false;
    //       }
    //       else
    //       {
    //           nuevaPartida.sprite = nuevaSelected[9];
    //           timer = 0;
    //           return true;
    //       }
    //   }



    //// Update is called once per frame
    //void Update () {
    //       //Al pulsar cualquier tecla pasa al menu principal con los botones para interactuar
    //       if (sceneState == 0){
    //           if (Input.anyKey){
    //               SceneState1();
    //           }
    //       }

    //      /* if (sceneState == 2)
    //       {
    //           if (Input.GetKey(KeyCode.Escape)) {
    //               ExitSceneState2();
    //               SceneState1();
    //           }
    //       }*/
    //   }

    //   void SceneState1()
    //   {
    //       sceneState = 1;
    //       sceneBg.sprite = secondSprite;
    //       for(int i = 0; i<buts.Length; i++)
    //       {
    //           buts[i].SetActive(true);
    //       }
    //       mouseInputScript.SetActive(true);
    //   }

    //   void ExitSceneState2()
    //   {
    //       for(int i = 0; i<partidas.Length; i++)
    //       {
    //           partidas[i].SetActive(false);
    //       }
    //   }

    //   void LoadTextures()
    //   {
    //       //Dos imagenes de fondo del menu
    //       firstSprite = Resources.Load<Sprite>("pre_menu");
    //       secondSprite = Resources.Load<Sprite>("menu_sin_botones");

    //       //Cargar animacion selected del boton continuar
    //       for (int i = 0; i < continuarSelected.Length; i++)
    //       {
    //           continuarSelected[i] = Resources.Load<Sprite>("Botones/Continuar/selected_" + i);
    //       }

    //       //Cargar animacion selected del boton nueva partida
    //       for(int i = 0; i<nuevaSelected.Length; i++)
    //       {
    //           nuevaSelected[i] = Resources.Load<Sprite>("Botones/Nueva_Partida/selected_" + i);
    //       }

    //       //Cargar animacion selected del boton opciones
    //       for (int i = 0; i<opcionesSelected.Length; i++)
    //       {
    //           opcionesSelected[i] = Resources.Load<Sprite>("Botones/Opciones/selected_" + i);
    //       }

    //       //Cargar animacion selected del boton salir
    //       for(int i = 0; i<salirSelected.Length; i++)
    //       {
    //           salirSelected[i] = Resources.Load<Sprite>("Botones/Salir/selected_" + i);
    //       }

    //       //Cargar animacion clicked del boton continuar
    //       for(int i = 0; i<continuarClick.Length; i++)
    //       {
    //           continuarClick[i] = Resources.Load<Sprite>("Botones/Continuar/click_"+i);
    //       }

    //       //Animacion click nueva partida
    //       for (int i = 0; i<nuevaClick.Length; i++)
    //       {
    //           nuevaClick[i] = Resources.Load<Sprite>("Botones/Nueva_Partida/click_" + i);
    //       }

    //       //Animacion click opciones
    //       for(int i = 0; i<opcionesClick.Length; i++)
    //       {
    //           opcionesClick[i] = Resources.Load<Sprite>("Botones/Opciones/click_" + i);
    //       }

    //       //Animacion click salir
    //       for(int i = 0; i<salirClick.Length; i++)
    //       {
    //           salirClick[i] = Resources.Load<Sprite>("Botones/Salir/click_" + i);
    //       }

    //       //Cargar los sprites para la seleccion de partida.
    //       idle_Partida1 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta1/idle");
    //       idle_Partida2 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta2/idle");
    //       idle_Partida3 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta3/idle");
    //       fondo = Resources.Load<Sprite>("Botones/Seleccion_Partida/fondo");
    //       selected_Partida1 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta1/selected");
    //       selected_Partida2 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta2/selected");
    //       selected_Partida3 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta3/selected");

    //("Botones/Seleccion_Partida/Carpeta3/selected");

   
    //   }

    //   void SceneState1()
    //   {
    //       sceneState = 1;
    //       sceneBg.sprite = secondSprite;
    //       for(int i = 0; i<buts.Length; i++)
    //       {
    //           buts[i].SetActive(true);
    //       }
    //       mouseInputScript.SetActive(true);
    //   }

    //   void ExitSceneState2()
    //   {
    //       for(int i = 0; i<partidas.Length; i++)
    //       {
    //           partidas[i].SetActive(false);
    //       }
    //   }

    //   void LoadTextures()
    //   {
    //       //Dos imagenes de fondo del menu
    //       firstSprite = Resources.Load<Sprite>("pre_menu");
    //       secondSprite = Resources.Load<Sprite>("menu_sin_botones");

    //       //Cargar animacion selected del boton continuar
    //       for (int i = 0; i < continuarSelected.Length; i++)
    //       {
    //           continuarSelected[i] = Resources.Load<Sprite>("Botones/Continuar/selected_" + i);
    //       }

    //       //Cargar animacion selected del boton nueva partida
    //       for(int i = 0; i<nuevaSelected.Length; i++)
    //       {
    //           nuevaSelected[i] = Resources.Load<Sprite>("Botones/Nueva_Partida/selected_" + i);
    //       }

    //       //Cargar animacion selected del boton opciones
    //       for (int i = 0; i<opcionesSelected.Length; i++)
    //       {
    //           opcionesSelected[i] = Resources.Load<Sprite>("Botones/Opciones/selected_" + i);
    //       }

    //       //Cargar animacion selected del boton salir
    //       for(int i = 0; i<salirSelected.Length; i++)
    //       {
    //           salirSelected[i] = Resources.Load<Sprite>("Botones/Salir/selected_" + i);
    //       }

    //       //Cargar animacion clicked del boton continuar
    //       for(int i = 0; i<continuarClick.Length; i++)
    //       {
    //           continuarClick[i] = Resources.Load<Sprite>("Botones/Continuar/click_"+i);
    //       }

    //       //Animacion click nueva partida
    //       for (int i = 0; i<nuevaClick.Length; i++)
    //       {
    //           nuevaClick[i] = Resources.Load<Sprite>("Botones/Nueva_Partida/click_" + i);
    //       }

    //       //Animacion click opciones
    //       for(int i = 0; i<opcionesClick.Length; i++)
    //       {
    //           opcionesClick[i] = Resources.Load<Sprite>("Botones/Opciones/click_" + i);
    //       }

    //       //Animacion click salir
    //       for(int i = 0; i<salirClick.Length; i++)
    //       {
    //           salirClick[i] = Resources.Load<Sprite>("Botones/Salir/click_" + i);
    //       }

    //       //Cargar los sprites para la seleccion de partida.
    //       idle_Partida1 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta1/idle");
    //       idle_Partida2 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta2/idle");
    //       idle_Partida3 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta3/idle");
    //       fondo = Resources.Load<Sprite>("Botones/Seleccion_Partida/fondo");
    //       selected_Partida1 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta1/selected");
    //       selected_Partida2 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta2/selected");
    //       selected_Partida3 = Resources.Load<Sprite>("Botones/Seleccion_Partida/Carpeta3/selected");

   //("Botones/Seleccion_Partida/Carpeta3/selected");
        
 //   }
}
