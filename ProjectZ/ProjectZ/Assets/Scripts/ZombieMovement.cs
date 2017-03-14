using UnityEngine;
using System.Collections;
using Pathfinding;

public class ZombieMovement : MonoBehaviour
{
    #region variables
    
    //Variable que maneja en que punto del camino se encuentra el zombie
    private int puntoActual = 0;

    //Distancia entre puntos
    public float distanciaSiguientePunto = 0.5f;

    //Variable que maneja el tiempo que ha pasado desde que el zombie ha empezado a moverse hacia el enemigo más cercano
    public float contador;

    //Tiempo que ha de pasar para que el zombie inicie un camino hacia el enemigo más cercano
    public float tiempoAContar;

    //Booleano que hace al zombie moverse, se maneja en función de si el usuario ha dado una orden (GameLogicScript) y desde ZombieScript
    public bool moving;

    //Variable que indicia que el jugador ha dado una orden, este hace que todo lo demás se detenga
    public bool wasCommanded;

    //Variable que maneja que el zombie empiece a aumentar el contador unicamente después de haber establecido un camino inicial hacia un enemigo
    public bool countedOnce;

    //Objeto de lógica de juego
    GameLogicScript gameLogic;

    //Variable (x,y,z) que guarda la posición a la que se va a mover el zombie
    public Vector3 targetPosition;

    //Componente de pathfinding que establece los caminos
    private Seeker buscador;

    //Component de pathfinding que guarda el camino a seguir
    private Path camino;

    #endregion


    //La funcion start inicializa los componentes gameLogic y targetPosition así como los booleanos en false y el tiempo de contador
    #region start
    void Start()
    {
        gameLogic = GameLogicScript.gameLogic;
        buscador = gameObject.GetComponent<Seeker>();
        tiempoAContar = 0.3f;
        moving = wasCommanded = countedOnce = false;
    }
    #endregion


    //Método que recibe un camino y en caso de no producir errores se asigna a la variable camino
    void MetodoCamino(Path path)
    {
        if (!path.error)
        {
            camino = path;
        }
    }

    /*Método llamado desde gameLogic cuando se da ordenes a los zombies y desde zombieScript cuando hay enemigos cerca y pueden atacarles
    Este comprueba si ha habido cambio en la posición objetivo del zombie y en caso de haberlo habido, establece el nuevo camino, a la par que 
    varia el comportamiento en función de si es una orden del jugador o si lo hace el zombie por "voluntad propia" y activa el booleano moving*/
    public void MoveTo(Vector3 newTargetPosition)
    {
        if (wasCommanded)
        {
            buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
            puntoActual = 0;
            contador = 0;
            LookTowards(newTargetPosition);
        }
        else if (targetPosition != newTargetPosition)
            {
                if (gameObject.GetComponent<ZombieScript>().movingToEnemy)
                {
                    if (contador > tiempoAContar)
                    {
                        buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                        puntoActual = 0;
                        contador = 0;
                    }
                }
                else {
                    buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                    puntoActual = 0;
                    contador = 0;
                }
                LookTowards(newTargetPosition);
            }
            targetPosition = newTargetPosition;
            
        
        moving = true;

    }

    public void LookTowards(Vector3 where) {
        gameObject.transform.LookAt(where);
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
    } 

    /*La función update se llama una vez por frame, y todo lo sucedido en ella depende del booleano isPaused de gameLogic
     Maneja el contador de forma independiente al resto de la función, el cuenta siempre que haya enemigos en rango y el zombie
     se este moviendo hacia ellos, tambien se dedica a mover al zombie y aumentar el contador de puntos (pasos) del camino
         */
    void Update()
    {
        if (!gameLogic.isPaused&&!gameLogic.eventManager.onEvent)
        {

                
            if (gameObject.GetComponent<ZombieScript>().movingToEnemy && !gameObject.GetComponentInChildren<AttackRangeZombie>().enemyInRange)
            {
                if (!countedOnce)
                {
                    contador = tiempoAContar;
                    countedOnce = true;
                }
                else {
                    tiempoAContar = 0.5f;
                    contador += Time.deltaTime;
                }
            }
            if (moving)
            {
                //El circulo de selección se debe poder manejar tambien cuando el zombie este en movimiento
                #region CirculoDeSelección
                if (!gameObject.GetComponent<ZombieScript>().isSelected)
                {
                    gameObject.GetComponent<ZombieScript>().elCirculo.SetActive(false);
                }
                else
                {
                    gameObject.GetComponent<ZombieScript>().elCirculo.SetActive(true);
                }

                #endregion

                if (camino == null)
                    return;

                //Si llega al final se cambian algunos booleanos
                if (puntoActual >= camino.vectorPath.Count)
                {
                    moving = false;
                    wasCommanded = false;
                    gameObject.GetComponent<ZombieScript>().hasArrived = true;
                    if (gameObject.GetComponent<ZombieScript>().goBarricade)
                        LookTowards(gameObject.GetComponent<ZombieScript>().barricada.transform.position);
                    gameObject.GetComponent<ZombieScript>().canAttack = true;
                    return;
                }

                //La dirección depende de la posición y el siguiente punto
                Vector3 direccion = (camino.vectorPath[puntoActual] - gameObject.transform.position).normalized;
                
                //Se amplifica en función de la velocidad de movimeinto y el tiempo
                direccion *= gameObject.GetComponent<ZombieScript>().movSpeed * Time.fixedDeltaTime;

                //Se mueve el zombie
                gameObject.transform.position += direccion * 0.5f;

                //Se aumenta el contador de pasos
                if (Vector3.Distance(transform.position, camino.vectorPath[puntoActual]) < distanciaSiguientePunto)
                {
                    puntoActual++;
                    return;
                }
            }
        }
    }
}