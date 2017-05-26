/*Este código está pensado para manejar la lógica de selección y movimiento de los zombies, así como el listado de estos 
 y de los villagers en partida*/
/*Se hace una referencia general al InputHandler
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine.SceneManagement;



public class GameLogicScript : MonoBehaviour
{
    public static GameLogicScript gameLogic;    //SINGLETON, es una variable estatica que se asigna al primer GameLogicScript que ejecute
    public CameraScript camara; //INSTANCIA DE LA CAMARA
    public int currentLevel;    //INTEGER QUE MANTIENE TRACK DEL NIVEL, SE UTILIZA PARA RECARGAR NIVEL O MODIFICAR NIVEL

    int defeatCounter;          //CONTADOR DE DERROTAS

    public bool isPaused;   //BOOLEANO GENERAL QUE SUSTITUYE AL TIMESCALE = 0


    public PausaCanvasScript elPausaScript; //INSTANCIA DEL SCRIPT DE PAUSADO DE LA ESCENA, SE CARGA EN CADA CAMBIO DE ESCENA

    //Vector que en su momento representara el punto destino de los zombies que se mueven
    private Vector3 endPoint;

    //vertical position of the gameobject
    private float yAxis;

    public bool loadingScene = false;
    public bool hasGamedOver = false;
    public bool waitAFrame = false;
    //Las máscaras se indican en el inspector

    //Mascara para el suelo (mask1)
    public LayerMask mascaraSuelo;

    //Mascara para los zombies (mask2)
    public LayerMask mascaraZombies;

    //Mascara para los villagers (mask2)
    public LayerMask mascaraVillagers;

    //Referenca al script de inputs

    public LayerMask mascaraRompible;
    GameObject aZombieToAdd;
    public LayerMask mascaraTorreta;

    InputHandlerScript _input;//INSTANCIA DEL INPUT HANDLER,SE CARGA EN CADA CAMBIO DE ESCENA

    //Listas de zombies: Existentes, seleccionados en un momento y los que se quedan en la lista tras terminar la selección
    public List<GameObject> _zombies;
    public List<GameObject> _selectedZombies;
    public List<GameObject> _keptSelectedZombies;
    public List<GameObject> _bases;
    public List<GameObject> _barricadas;
    public List<GameObject> _torretas;

    //Lista de villagers

    public List<GameObject> _villagers;

    //Objeto de cuadro de selección
    public GameObject _selectionBox;

    //Origen de la selección actual
    public Vector3 _selectionOrigin;

    //Con esta variable sabemos si hemos comenzado una selección
    public bool _selecting;

    //Estas variables son las posiciones de los 3 zombies que se crean por codigo más abajo
    //public Vector3 position1;
    // public Vector3 position2;
    // public Vector3 position3;

    public GameObject elPathfinder; //INSTANCIA DEL OBJETO DEL PATHFINDER
    public float timeSelecting = 0;

    GameObject walker;  //"PUNTERO" AL PREFAB DEL WALKER
    GameObject soldier; //"PUNTERO" AL PREFAB DEL SOLDIER
    GameObject mutank;  //"PUNTERO" AL PREFAB DEL MUTANK
    GameObject runner;  //"PUNTERO" AL PREFAB DEL RNUNER

    GameObject selectedBarricade;//"PUNTERO" A LA BARRICADA SOBRE LA CUAL SE HACE CLICK PARA MOSTRAR SU VIDA

    GameObject villager;//"PUNTERO" AL PREFAB DEL VILLAGER

    public EventManager eventManager; //INSTANCIA DE LA CLASE EVENTMANAGER

    #region MetodosDeAparicion

    //METODOS DE SPAWNEADO DE PERSONAJES DEL JUEGO, LOS HUMANOS TIENEN EL METODO SOBRECARGADO PARA PODER MANDARLES UNA POSICION ESPECFÍCIA DE PATRUYA
    public void SpawnVillager(Vector3 patrolPos, Vector3 unaPos) {
        unaPos.y = 0.02499896f;
        GameObject villagerToSpawn = Instantiate(villager, unaPos, Quaternion.identity) as GameObject;
        GameObject anEmptyGameObject = new GameObject();
        villagerToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.villager;
        anEmptyGameObject.transform.position = patrolPos;
        _villagers.Add(villagerToSpawn);
        villagerToSpawn.GetComponent<VillagerScript>().patrolPointObject = anEmptyGameObject;
        Debug.Log("Spawning a villager with a patrollingPosition");
    }

    public void SpawnVillager(Vector3 unaPos) {

        unaPos.y = 0.02499896f;
        GameObject villagerToSpawn = Instantiate(villager, unaPos, Quaternion.identity) as GameObject;
        villagerToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.villager;
        _villagers.Add(villagerToSpawn);
        Debug.Log("Spawning a villager without a patrollingPosition");
        Debug.Log(unaPos);

    }

    public void SpawnSoldier(Vector3 patrolPos, Vector3 unaPos)
    {
        unaPos.y = 0.02499896f;
        Debug.Log("Spawning a soldier with a patrollingPosition");
        Debug.Log(unaPos);

        GameObject soldierToSpawn = Instantiate(soldier, unaPos, Quaternion.identity) as GameObject;
        GameObject anEmptyGameObject = new GameObject();
        soldierToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
        anEmptyGameObject.transform.position = patrolPos;
        _villagers.Add(soldierToSpawn);
        soldierToSpawn.GetComponent<VillagerScript>().patrolPointObject = anEmptyGameObject;
    }

    public void SpawnSoldier(Vector3 unaPos, GameObject unObjetoDePatrulla)
    {
        unaPos.y = 0.02499896f;

        Debug.Log("Spawning a soldier with a patrollingPosition at ");
        Debug.Log(unaPos);

        GameObject soldierToSpawn = Instantiate(soldier, unaPos, Quaternion.identity) as GameObject;
        soldierToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
        soldierToSpawn.GetComponent<VillagerScript>().patrolPointObject = unObjetoDePatrulla;
        _villagers.Add(soldierToSpawn);
    }

    public void SpawnSoldier(Vector3 unaPos) {
        unaPos.y = 0.02499896f;

        Debug.Log("Spawning a soldier without a patrollingPosition");
        Debug.Log(unaPos);

        GameObject soldierToSpawn = Instantiate(soldier, unaPos, Quaternion.identity) as GameObject;
        soldierToSpawn.GetComponent<VillagerScript>().tipo = VillagerScript.humanClass.soldier;
        _villagers.Add(soldierToSpawn);
    }

    public void SpawnWalker(Vector3 unaPos) {
        unaPos.y = 0.02499896f;

        Debug.Log("Spawning a walker");

        GameObject zombieToSpawn = Instantiate(walker, unaPos, Quaternion.identity) as GameObject;
        _zombies.Add(zombieToSpawn);
    }

    public void SpawnMutank(Vector3 unaPos) {
        unaPos.y = 0.02499896f;

        Debug.Log("Spawning a mutank");
        GameObject zombieToSpawn = Instantiate(mutank, unaPos, Quaternion.identity) as GameObject;
        _zombies.Add(zombieToSpawn);
    }

    public void SpawnRunner(Vector3 unaPos) {
        unaPos.y = 0.02499896f;

        Debug.Log("Spawning a runner");

        GameObject zombieToSpawn = Instantiate(runner, unaPos, Quaternion.identity) as GameObject;
        _zombies.Add(zombieToSpawn);
    }
    #endregion

    //EN EL AWAKE DEL GAMELOGIC SE COMPRUEBA SI EL SINGLETON ESTA ASIGNADO Y EN CASO DE NO ESTARLO SE
    //ASIGNA LA INSTANCIA ACTUAL, EN CASO CONTRADO, SE AUTODESTRUYE
    void Awake()
    {
        if (gameLogic == null)
        {
            DontDestroyOnLoad(gameObject);
            gameLogic = this;
        }
        else if (gameLogic != this) {
            Destroy(gameObject);
        }
    }

    //INICIALIZA LAS VARIABLES PERTINENTES Y ASIGNA LOS PREFABS Y LAS INSTANCIAS DE LOS OBJETOS ESPECIALES (PAUSESCRIPT,PATHFINDER)
    void Start()
    {
        defeatCounter = 0;

        camara = FindObjectOfType<CameraScript>();

        eventManager = EventManager.eventManager;

        //eventManager.SetEvents(eventos,10);

        elPathfinder = GameObject.FindGameObjectWithTag("A*");

        isPaused = false;

        elPausaScript = FindObjectOfType<PausaCanvasScript>();

        //posicionBase1 = new Vector3(0.69f,0.05f,13.72f);

        yAxis = gameObject.transform.position.y;

        //Guardamos la referencia al input en nuestra clase
        _input = this.GetComponent<InputHandlerScript>();

        //Inicializamos las listas
        _zombies = new List<GameObject>();
        _selectedZombies = new List<GameObject>();
        _keptSelectedZombies = new List<GameObject>();
        _villagers = new List<GameObject>();

        //Se cargan los prefabs
        walker = Resources.Load("WalkerObject") as GameObject;
        runner = Resources.Load("RunnerObject") as GameObject;
        mutank = Resources.Load("MutankObject") as GameObject;
        villager = Resources.Load("survivorObject") as GameObject;
        soldier = Resources.Load("SoldierObject") as GameObject;

        //Se oblga al pathfinder a hacer un escaneo inicial del mapa tras inicializar los elementos
        elPathfinder.GetComponent<AstarPath>().Scan();
    }

    //VACIA TODAS LISTAS
    public void ClearLists()
    {
        _bases.Clear();
        _torretas.Clear();
        _barricadas.Clear();
        _villagers.Clear();
        _zombies.Clear();
        _keptSelectedZombies.Clear();
        _selectedZombies.Clear();
    }


    void Update()
    {
        if (!waitAFrame)
        {
            waitAFrame = true;
        }else { 
        foreach (GameObject z in _keptSelectedZombies)
        {
            z.GetComponent<ZombieScript>().isSelected = true;
        }

            if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
                gameLogic.currentLevel++;
                SceneManager.LoadScene(currentLevel);
            }

        //COMPRUEBA SI LAS INSTANCIAS DE LOS OBJETOS ESPECIALES SON NULAS, EN CASO DE SERLO, LAS REASIGNA
        //COMPRUEBA SI NO QUEDAN ZOMBIES, EN CUYO CASO, SE CONSIDERA QUE EL JUGADOR HA PERDIDO, SE REINICIA EL NIVEL PASANDO POR ESCENAINTER Y SE AUMENTA EL
        //CONTADOR DE DERROTAS

        if (_zombies.Count == 0 && !hasGamedOver)
        {
            defeatCounter++;
            ClearLists();
            loadingScene = true;
            hasGamedOver = true;
            SceneManager.LoadScene("EscenaInter");
        }

        if (camara == null)
        {
            camara = FindObjectOfType<CameraScript>();
        }

        if (elPathfinder == null)
        {
            elPathfinder = GameObject.FindGameObjectWithTag("A*");
        }

        if (elPausaScript == null)
        {
            elPausaScript = FindObjectOfType<PausaCanvasScript>();
        }

        if (eventManager == null)
        {
                eventManager = EventManager.eventManager;
        }

        //Por encima de todo lo demás se maneja el booleano del pausado
        if (eventManager != null && !eventManager.onEvent && Input.GetKeyDown(KeyCode.Escape))
        {
            changePause();
        }

        //EN CASO DE QUE NO ESTE PAUSADO, EL FUNCONAMIENTO NORMAL CONSISTE EN ACTUALIZAR LAS SELECCIONES Y REALIZAR LOS RAYCASTS EN FUNCION DEL CLICK IZQUIERDO
        //EL CLICK DERECHO MUEVE A LOS ZOMBIES SELECCIONADOS HACIA LA POSICION SOBRE LA CUAL SE HACE CLICK DERECHO

        //LA VARIABLE QUE ES EL OBJETO SOBRE EL CUAL LA CAMARA SE CONCENTRA ES EL ZOMBIE EN LA POSICION 0 DE LA LISTA DE _keptSelectedZombies

        //LA TECLA A Y S VARÍAN EL TOGGLE DE ATAQUE QUE CONSISTE EN DETERMINAR SI LOS ZOMBIES DEBEN O NO ATACAR A LOS HUMANOS CERCANOS AL ACABAR SU MOVIMIENTO
        if (!isPaused)
        {
                if (!loadingScene)
                {
                    if (-_keptSelectedZombies.Count != 0)
                    {
                        camara.objetoAFocusear = _keptSelectedZombies[0];
                    }
                    else if (camara != null)
                    {
                        camara.objetoAFocusear = null;
                    }

                    if (eventManager == null)
                    {
                        eventManager = EventManager.eventManager;
                    }
                    if (eventManager != null) { 
                    if (!eventManager.onEvent)
                    {

                                //Se hace true el booleano attackToggle en cada zombie seleccionado al pulsar la tecla A
                                if (_input._attackToggle && _keptSelectedZombies.Count > 0)
                        {
                            foreach (GameObject t in _keptSelectedZombies)
                            {
                                if (t.GetComponent<ZombieScript>() != null)
                                    t.GetComponent<ZombieScript>().attackToggle = true;
                            }
                        }

                        //O Se hace false en los zombies seleccionados al pulsar la tecla S
                        else if (!_input._attackToggle && _keptSelectedZombies.Count > 0)
                        {
                            foreach (GameObject t in _keptSelectedZombies)
                            {
                                t.GetComponent<ZombieScript>().attackToggle = false;
                            }
                        }
                        //Funcion que dibuja la caja de seleccion
                        DrawSelectionBox();

                        //Funcion que actualiza la seleccion
                        UpdateSelection();

                        //Funcion de apoyo para actualizar la seleccion cuando los zombies mueren
                        UpdateSelection2();

                        //Al pulsar el boton derecho del ratón, se genera un rayo en el mundo

                        if ((Input.GetMouseButtonDown(0)))
                        {


                            RaycastHit hit;

                            //Se crea la variable de rayo
                            Ray ray;

                            //Al ser el editor de unity se utiliza esta funcion para el Rayo

                            //#if UNITY_EDITOR
                            //                ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            //#endif
                            ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                            if (Physics.Raycast(ray, out hit, 80, mascaraZombies))
                            {
                                    aZombieToAdd = null;
                                GameObject aZombie;

                                aZombie = hit.collider.gameObject;
                                    aZombieToAdd = aZombie;

                                    if (!_keptSelectedZombies.Contains(aZombie))
                                {
                                        Debug.Log("Se añade keptSelected");
                                        _keptSelectedZombies.Add(aZombie);
                                    //  aZombie.GetComponent<ZombieScript>().isSelected = true;
                                }
                            }

                            if (Physics.Raycast(ray, out hit, 80, mascaraRompible))
                            {
                                    aZombieToAdd = null;
                                    if (selectedBarricade != null)
                                    selectedBarricade.GetComponentInParent<BarricadaScript>().ShowCircle(false);
                                selectedBarricade = hit.collider.gameObject;
                                selectedBarricade.GetComponentInParent<BarricadaScript>().ShowCircle(true);

                            }
                            else
                            {
                                if (selectedBarricade != null)
                                {
                                    selectedBarricade.GetComponentInParent<BarricadaScript>().ShowCircle(false);
                                    selectedBarricade = null;
                                }
                            }
                        }

                        if ((Input.GetMouseButtonDown(1)))
                        {
                            //Se declara una variable del struct RayCastHit
                            RaycastHit hit;

                            //Se crea la variable de rayo
                            Ray ray;

                            //Al ser el editor de unity se utiliza esta funcion para el Rayo

                            //#if UNITY_EDITOR
                            //#endif
                            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                                if (Physics.Raycast(ray, out hit, 80, mascaraVillagers)) {
                                    foreach (GameObject z in _keptSelectedZombies) {
                                        Debug.Log("ResetPorVillager");
                                        z.GetComponent<ZombieScript>().ResetStuff("command");
                                        z.GetComponent<ZombieScript>().keepGoingBarricade = false;
                                        z.GetComponent<ZombieMovement>().MoveTo(hit.collider.gameObject.transform.position);
                                        z.GetComponent<ZombieScript>().movingToEnemy = true;
                                        z.GetComponent<ZombieScript>().villagerToAttackOnClick = hit.collider.gameObject;
                                        if (z.GetComponent<ZombieScript>().turretToAttack != null) {
                                            if (z.GetComponent<ZombieScript>().turretToAttack.GetComponentInChildren<TorretaScript>() != null)
                                                z.GetComponent<ZombieScript>().turretToAttack.GetComponentInChildren<TorretaScript>()._targetingZombies.Remove(gameObject);
                                        }
                                        z.GetComponent<ZombieScript>().turretToAttack = null;
                                    }
                                }

                                else if (Physics.Raycast(ray, out hit, 80, mascaraRompible)) {
                                    GameObject laBarricada = hit.collider.gameObject;
                                    foreach (GameObject z in _keptSelectedZombies) {
                                        //z.GetComponent<ZombieMovement>().moving = false;
                                        z.GetComponent<ZombieMovement>().wasCommanded = false;
                                        z.GetComponent<ZombieScript>().hasArrived = true;
                                        if (z.GetComponent<ZombieScript>().goBarricade && z.GetComponent<ZombieScript>().barricada != null)
                                            z.GetComponent<ZombieMovement>().LookTowards(gameObject.GetComponent<ZombieScript>().barricada.transform.position);


                                        z.GetComponent<ZombieScript>().attackBarricade(laBarricada);
                                        z.GetComponent<ZombieScript>().keepGoingBarricade = true;
                                        if (z.GetComponent<ZombieScript>().turretToAttack != null) {
                                            if (z.GetComponent<ZombieScript>().turretToAttack.GetComponentInChildren<TorretaScript>() != null)
                                                z.GetComponent<ZombieScript>().turretToAttack.GetComponentInChildren<TorretaScript>()._targetingZombies.Remove(gameObject);
                                        }
                                        z.GetComponent<ZombieScript>().turretToAttack = null;
                                        
                                    }

                                }
                                else if (Physics.Raycast(ray, out hit, 80, mascaraTorreta)) {
                                    GameObject laTorreta = hit.collider.gameObject;
                                    foreach (GameObject z in _keptSelectedZombies) {
                                        z.GetComponent<ZombieScript>().keepGoingBarricade = false;
                                        z.GetComponent<ZombieScript>().turretToAttack = laTorreta;
                                        if (!laTorreta.GetComponentInChildren<TorretaScript>()._targetingZombies.Contains(z)&&laTorreta.GetComponent<TorretaScript>().alive) {
                                            laTorreta.GetComponentInChildren<TorretaScript>()._targetingZombies.Add(z);
                                        }
                                    }


                                }


                                //Se comprueba si choca con algun collider, teniendo en cuenta solo los objetos que pertenecen a la mascara mask1 "Ground"
                                else if (Physics.Raycast(ray, out hit, 80, mascaraSuelo)) {
                                    //Se guarda la posicion clicada 
                                    endPoint = hit.point;

                                    //Como no nos interesa que cambie la Y del zombie que se esta moviendo la restauramos a la original
                                    endPoint.y = yAxis;

                                    //Aqui se intenta que el zombie mire a la posicion a la que se esta moviendo
                                    //this.gameObject.transform.LookAt(hit.point);

                                    //Esta i se utiliza de contador para repartir a los zombies alrededor del punto elegido como destino
                                    int i = 0;

                                    //Por cada zombie en la lista de zombies seleccionados, se establece el movimiento final en funcion de la 
                                    //Cantidad de zombies que se han movido ya hacia el punto y van rotando en un angulo de 45 grados alrededor del punto
                                    foreach (GameObject zombie in _keptSelectedZombies) {
                                        if (zombie != null) {
                                            zombie.GetComponent<ZombieScript>().keepGoingBarricade = false;
                                            if (zombie.GetComponent<ZombieScript>().turretToAttack != null) {
                                                if (zombie.GetComponent<ZombieScript>().turretToAttack.GetComponentInChildren<TorretaScript>() != null)
                                                    zombie.GetComponent<ZombieScript>().turretToAttack.GetComponentInChildren<TorretaScript>()._targetingZombies.Remove(gameObject);
                                            }
                                            zombie.GetComponent<ZombieScript>().turretToAttack = null;

                                            Vector3 desplazamientoFinal = Vector3.zero;
                                            if (i >= 1) {
                                                float angle = 45 * i;
                                                Quaternion rotacion = Quaternion.AngleAxis(angle, Vector3.up);
                                                Vector3 distancia = Vector3.right * (1f * (1 + ((i - 1) / 8)));
                                                desplazamientoFinal = rotacion * distancia;
                                                if (zombie.GetComponent<ZombieScript>().barricada != null) {
                                                    if (zombie.GetComponent<ZombieScript>().barricada._atacantes.Contains(zombie))
                                                        zombie.GetComponent<ZombieScript>().barricada.VaciarSitio(zombie.GetComponent<ZombieScript>().barricadaSpot);
                                                    zombie.GetComponent<ZombieScript>().barricada._atacantes.Remove(zombie);
                                                }
                                            }
                                            //Esta funcion hace a los zombies moverse hacia el punto deseado pero teniendo en cuenta el desplazamiento final 
                                            //Para cada zombie
                                            MoveZombies(zombie, endPoint + desplazamientoFinal);
                                            i++;
                                        }
                                    }
                                }

                        }

                            if (_input._selectWalkers) {
                                _keptSelectedZombies.Clear();
                                foreach (GameObject z in _zombies) {
                                    if (z.GetComponent<ZombieScript>().tipo == ZombieScript.zombieClass.walker) {
                                        _keptSelectedZombies.Add(z);
                                        z.GetComponent<ZombieScript>().isSelected = true;

                                    }
                                    else {
                                        z.GetComponent<ZombieScript>().isSelected = false;
                                    }
                                }
                                _input._selectWalkers = false;
                            }
                            else if (_input._selectMutanks) {
                                _keptSelectedZombies.Clear();
                                foreach (GameObject z in _zombies) {
                                    if (z.GetComponent<ZombieScript>().tipo == ZombieScript.zombieClass.mutank) {
                                        _keptSelectedZombies.Add(z);
                                        z.GetComponent<ZombieScript>().isSelected = true;

                                    }
                                    else {
                                        z.GetComponent<ZombieScript>().isSelected = false;
                                    }
                                }
                                _input._selectMutanks = false;
                            }
                            else if (_input._selectRunners) {
                                _keptSelectedZombies.Clear();
                                foreach (GameObject z in _zombies) {
                                    if (z.GetComponent<ZombieScript>().tipo == ZombieScript.zombieClass.runner) {
                                        _keptSelectedZombies.Add(z);
                                        z.GetComponent<ZombieScript>().isSelected = true;

                                    }
                                    else {
                                        z.GetComponent<ZombieScript>().isSelected = false;
                                    }
                                }
                                _input._selectRunners = false;
                            }
                            else if (_input._deSelect) {
                                _input._deSelect = false;
                                _keptSelectedZombies.Clear();
                            }


                        }
                }
            }
        }
    }
}
    //Método que cambia el booleano de pausado
    public void changePause() {
            isPaused = !isPaused;  
    }

    //Un método que devuelve un booleano de forma ineficiente para no tener que escribir todo el codigo de negación
    public bool IsNotAlive(GameObject z)
    {
        if (z != null)
        {
            if (z.GetComponent<ZombieScript>() != null)
            {
                return !z.GetComponent<ZombieScript>().isAlive;
            }
            else
            {
                return !z.GetComponent<VillagerScript>().isAlive;
            }
        }else
        {
            return true;
        }
    }

    //Método que calcula la distancia entre dos GameObjects
    public float CalcularDistancia(GameObject a,GameObject b) {
        if (a == null || b == null)
            return -1;
        else
            return (a.transform.position - b.transform.position).magnitude;
    }

    //El codigo adicional que modifica las listas de zombies y villagers en funcion de una funcion que comprueba si estan vivos o no
    void UpdateSelection2() {
        _zombies.RemoveAll(IsNotAlive);
        _selectedZombies.RemoveAll(IsNotAlive);
        _keptSelectedZombies.RemoveAll(IsNotAlive);
        foreach (GameObject v in _villagers) {
            if (v != null) { 

                if (!v.GetComponent<VillagerScript>().isAlive && !v.GetComponent<VillagerScript>().hasTransformed) {
                    int que = Random.Range(0, 20);
                    v.GetComponent<VillagerScript>().hasTransformed = true;
                    if (que < 10) {
                        SpawnWalker(v.transform.position);
                    }
                    else if (que < 16) {
                        SpawnRunner(v.transform.position);
                    }
                    else {
                        SpawnMutank(v.transform.position);
                    }
                }
        }
    }
        _villagers.RemoveAll(IsNotAlive);
    }

    //Funcion que dibuja la caja de seleccion
    void DrawSelectionBox()
    {
        if (!_selecting)
        {
            //Si no estamos seleccionando, comprobamos que si se ha pulsado la tecla de selección
            if (_input._selectingBegins)
            {
                if (!_input._keepSelection) {
                    _keptSelectedZombies.Clear();
                    if(!_keptSelectedZombies.Contains(aZombieToAdd))
                        _keptSelectedZombies.Add(aZombieToAdd);
                    aZombieToAdd = null;

                }



                RaycastHit hit;
                Ray ray;

                //Lanzamos un rayo desde la pantalla de nuestra cámara, tomando como punto la posición de nuestro puntero
                ray = Camera.main.ScreenPointToRay(_input._mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    //Guardamos el punto en el que colisiona nuestro rayo.
                    _selectionOrigin = hit.point;

                    //Creamos el cuadro de selección instanciandolo a partir de un prefab
                    _selectionBox = Instantiate(Resources.Load("SelectionBox")) as GameObject;
                    _selectionBox.GetComponent<GUITexture>().pixelInset = new Rect(_input._mousePosition.x, _input._mousePosition.y, 1, 1);
                }
            }
        }
        else
        {
            timeSelecting += Time.deltaTime;
            //Si ya hemos comenzado una selección, comprobamos que ésta no ha acabado
            if (_input._selectingEnds)
            {
                //Destruimos el cuadro de selección en caso de que esta haya acabado
                Destroy(_selectionBox);
            }
            else
            {
                //Estos son los límites de nuestro cuadro de selección
                Rect bound = _selectionBox.GetComponent<GUITexture>().pixelInset;

                //Con esta sencilla función pasamos el origen de la selección a coordenadas de pantalla
                Vector3 selectionOriginBox = Camera.main.WorldToScreenPoint(_selectionOrigin);

                //Recogemos los límites de nuestro cuadro en función del punto de origen y la posición actual del ratón
                bound.xMin = Mathf.Min(selectionOriginBox.x, _input._mousePosition.x);
                bound.yMin = Mathf.Min(selectionOriginBox.y, _input._mousePosition.y);
                bound.xMax = Mathf.Max(selectionOriginBox.x, _input._mousePosition.x);
                bound.yMax = Mathf.Max(selectionOriginBox.y, _input._mousePosition.y);

                //Cambiamos el pixelInset de nuestro cuadro de selección
                _selectionBox.GetComponent<GUITexture>().pixelInset = bound;
            }
        }
    }

    //Método que actualiza la selección
    void UpdateSelection()
    {
        if (!_selecting)
        {
            if (_input._selectingBegins)
            {
                //Si no mantenemos la selección ni la invertimos y comenzamos una nueva seleccion
                if (!_input._keepSelection && !_input._invertSelection&&timeSelecting>0.3)
                {
                    //Desmarcamos los zombies 
                    //Limpiamos las listas de zombies seleccionados
                    _selectedZombies.Clear(); //Esta no es necesario limpiarla ya
                    foreach (GameObject z in _keptSelectedZombies) {
                        z.GetComponent<ZombieScript>().isSelected = false;
                    }
                    //_keptSelectedZombies.Clear();

                }

                //Indicamos que hemos empezado una selección
                _selecting = true;
            }
        }
        else
        {
            if (_input._selectingEnds)
            {
                timeSelecting = 0;
                //Guardamos la lista actual de zombies seleccionados
                foreach (GameObject zombie in _selectedZombies)
                    if (zombie != null)
                    {
                        _keptSelectedZombies.Add(zombie);

                        // zombie.GetComponent<ZombieScript>().isSelected = true;
                    }
                //Indicamos que hemos finalizado nuestra selección
                _selecting = false;
            }
            else
            {
                RaycastHit hit;
                Ray ray;

                //Buscamos los zombies se encuentren dentro de la caja de selección
                List<GameObject> zombiesInSelectionBox = new List<GameObject>();

                //Dado que no se puede modificar una lista mientras la estás recorriendo,
                //es mejor utilizar listas alternaticas para agregar y remover

                //Lista de zombies que añadiremos a la selección
                List<GameObject> zombiesToAdd = new List<GameObject>();

                //Lista de zombies que removeremos de la selección
                List<GameObject> zombiesToRemove = new List<GameObject>();

                List<GameObject> zombiesToMove = new List<GameObject>();

                //Primero lanzamos un rayo para guardar el punto de finalización de la selección
                ray = Camera.main.ScreenPointToRay(_input._mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    //Este es el plano tridimensional de selección
                    Rect selectionPlane = new Rect();

                    //Se hace al selectionPlane ser igual que la _selectionbox ya que ambos estan en función de la pantalla
                    selectionPlane = _selectionBox.GetComponent<GUITexture>().pixelInset;

                    //Comprobamos que el rayo no golpea directamente en una unidad
                    if (_zombies.Contains(hit.collider.gameObject))
                    {

                        //Como el collider forma parte del propio objeto se añaden el zombie en contacto con el propio rayo
                        //en funcion de su propio collider

                        zombiesInSelectionBox.Add(hit.collider.gameObject);

                    }

                    //Agregamos a la lista los zombies que se encuentran dentro del cuadro de selección
                    //Iteración realizada una vez por zombie en la lista de zombies
                    foreach (GameObject zombie in _zombies)
                    {
                        if (!zombiesInSelectionBox.Contains(zombie) && (Camera.main.WorldToScreenPoint(zombie.transform.position).x >= selectionPlane.xMin && Camera.main.WorldToScreenPoint(zombie.transform.position).x <= selectionPlane.xMax && Camera.main.WorldToScreenPoint(zombie.transform.position).y >= selectionPlane.yMin && Camera.main.WorldToScreenPoint(zombie.transform.position).y <= selectionPlane.yMax))
                        {
                            zombiesInSelectionBox.Add(zombie);
                          //  zombie.GetComponent<ZombieScript>().isSelected = true;
                        }
                        else
                        {
                                _selectedZombies.Remove(zombie);
                                zombie.GetComponent<ZombieScript>().isSelected = false;
                        }
                    }
                }

                foreach (GameObject zombies in zombiesInSelectionBox)
                {
                    if (zombies != null)
                    {
                        if (!_input._invertSelection)
                        {
                            //Si no está pulsada la tecla de invertSelection seleccionamos los zombies del cuadro
                            if (!_selectedZombies.Contains(zombies))
                            {
                                {
                                    zombiesToAdd.Add(zombies);
                                    zombiesToMove.Add(zombies);
                                }
                            }
                        }
                    }
                    else
                    {
                        //Si está pulsada la tecla de invertSelection removemos los zombies del cuadro
                        if (_selectedZombies.Contains(zombies))
                        {
                            zombiesToRemove.Add(zombies);
                        }
                    }
                }

                if (!_input._keepSelection)
                {
                    foreach (GameObject zombie in _keptSelectedZombies)
                    {
                        if (zombie != null)
                        {
                            if (!_input._invertSelection)
                            {
                                
                                if (!zombiesInSelectionBox.Contains(zombie) && _selectedZombies.Contains(zombie)&&timeSelecting>0.3)
                                {
                                    zombiesToRemove.Add(zombie);
                                }
                            }
                        }
                        else
                        {
                            if (!zombiesInSelectionBox.Contains(zombie) && !_selectedZombies.Contains(zombie))
                            {
                                zombiesToAdd.Add(zombie);
                                zombiesToMove.Add(zombie);

                            }
                        }
                    }
                }

                foreach (GameObject zombie in zombiesToAdd)
                {
                    if (zombie != null)
                    {
                        SelectZombie(zombie);
                    }
                }

                foreach (GameObject zombie in zombiesToRemove)
                {
                    if (zombie != null)
                    {
                        DeselectZombie(zombie);
                    }
                }

            }
        }
    }

    void SelectZombie(GameObject zombie)
    {
        //Comprobamos que el zombie no esté ya seleccionado
        if (!_selectedZombies.Contains(zombie))
        {
            //Agregamos el zombie a la lista
            _selectedZombies.Add(zombie);

            foreach (GameObject z in _selectedZombies) {

                //Se pone en true el booleano de selección
                z.GetComponent<ZombieScript>().isSelected = true;

            }
        }
    }


    //Función para deseleccionar zombies de la lista
    void DeselectZombie(GameObject zombie)
    {
        if (_selectedZombies.Contains(zombie))
        {
            //Quitamos al zombie de la lista
            _selectedZombies.Remove(zombie);
        }
    }
    //Función del movimiento
    #region movimiento
    void MoveZombies(GameObject zombie, Vector3 desiredPosition)
    {
        //En caso de que existan zombies en la lista de _keptSelectedZombies
        if (_keptSelectedZombies.Contains(zombie))
        {
            //Agregamos el zombie a la lista
            //_selectedZombies.Add(zombie);

            //Se hace una referencia al script de movimiento de cada zombie a cada iteración
            ZombieMovement zombieMovement = zombie.GetComponent<ZombieMovement>();

            //El booleano wasCommanded se pone en true, ya que se les ha ordenador moverse
            zombieMovement.wasCommanded = true;
            //Función que mueve a los zombies
            zombie.GetComponent<ZombieScript>().goBarricade = false;


            zombieMovement.MoveTo(desiredPosition);
        }

    }
    #endregion
}
