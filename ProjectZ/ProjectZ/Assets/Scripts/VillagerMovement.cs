using UnityEngine;
using System.Collections;
using Pathfinding;
public class VillagerMovement : MonoBehaviour
{
    GameLogicScript gameLogic;//INSTANCIA DEL SINGLETON GAMELOGIC


    public bool moving;//BOOLEANO QUE GESTIONA SI ESTA EN MOVIMIENTO
    public Vector3 targetPosition;
    private Path camino;
    private Seeker buscador;
    public float distanciaSiguientePunto = 0.5f;
    public int puntoActual = 0;
    public float contador;
    public float tiempoAContar;
    public bool hasArrived;
    Animator elAnimator; 
    //private bool startedMoving;

    public void LookTowards(Vector3 where)
    {
        gameObject.transform.LookAt(where);
        gameObject.transform.eulerAngles = new Vector3(0, gameObject.transform.eulerAngles.y, gameObject.transform.eulerAngles.z);
    }

    void Start()
    //void Start()
    {
        hasArrived = false;
        gameLogic = GameLogicScript.gameLogic;

        tiempoAContar = 1;
        //startedMoving = false;
        buscador = gameObject.GetComponent<Seeker>();
        elAnimator = gameObject.GetComponent<Animator>();
    }

    public void MoveTo(Vector3 newTargetPosition)
    {
        //if (!startedMoving)
        {
            //  startedMoving = true;
            if (targetPosition != newTargetPosition)
            {
                if (gameObject.GetComponent<VillagerScript>().laVision.enemyInSight&&!GetComponentInParent<VillagerScript>().runAway)
                {
                    contador += Time.deltaTime;
                    if (contador > tiempoAContar)
                    {
                        if (buscador != null)
                        {
                            buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                            contador = 0;
                        }
                    }
                }
                else if(gameObject.GetComponent<VillagerScript>().laVision.enemyInSight && !GetComponentInParent<VillagerScript>().runAway)
                {
                    buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                }

                else
                {
                    if (buscador != null)
                    {
                        buscador.StartPath(transform.position, newTargetPosition, MetodoCamino);
                        contador = 0;
                    }
                    LookTowards(newTargetPosition);
                }
            }
            targetPosition = newTargetPosition;

        }
        moving = true;
    }

    void MetodoCamino(Path path)
    {
        if (!path.error)
        {
            camino = path;
            puntoActual = 0;
        }
    }


    void Update()
    {
        if (gameLogic.eventManager != null)
        {
            if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {

            if (moving)
            {
                if (camino == null)
                    return;
                if (puntoActual >= camino.vectorPath.Count)
                {
                    hasArrived = true;
                    //LlegaAlFinal
                    moving = false;
                    gameObject.GetComponent<VillagerScript>().goingToCheck = false;
                    //Debug.Log("FinalAlcanzado");
                    //startedMoving = false;
                    return;
                }

                    LookTowards(targetPosition);


                    Vector3 direccion = (camino.vectorPath[puntoActual] - gameObject.transform.position).normalized;

                direccion *= gameObject.GetComponent<VillagerScript>().movSpeed * Time.fixedDeltaTime;

                gameObject.transform.position += direccion * 0.5f;

                if (Vector3.Distance(transform.position, camino.vectorPath[puntoActual]) < distanciaSiguientePunto)
                {
                    puntoActual++;
                    return;
                }

            }
        } }
        elAnimator.SetBool("moviendose",moving);
        if (moving == false) {
            elAnimator.SetBool("correr", false);
        }

    }
}