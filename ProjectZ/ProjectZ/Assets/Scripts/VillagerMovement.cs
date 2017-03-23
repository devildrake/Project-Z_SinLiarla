using UnityEngine;
using System.Collections;
using Pathfinding;
public class VillagerMovement : MonoBehaviour
{

    GameLogicScript gameLogic;


    public bool moving;
    public Vector3 targetPosition;
    private Path camino;
    private Seeker buscador;
    public float distanciaSiguientePunto = 0.5f;
    public int puntoActual = 0;
    public float contador;
    public float tiempoAContar;
    public bool hasArrived; 
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

    }

    public void MoveTo(Vector3 newTargetPosition)
    {
        //if (!startedMoving)
        {
            //  startedMoving = true;
            if (targetPosition != newTargetPosition)
            {
                if (gameObject.GetComponent<VillagerScript>().laVision.enemyInSight)
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


    //IEnumerator buscarCamino(float tiempo)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(tiempo);
    //        buscador.StartPath(transform.position, targetPosition, MetodoCamino);
    //    }

    //}




    void Update()
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

                    //startedMoving = false;
                    return;
                }

                Vector3 direccion = (camino.vectorPath[puntoActual] - gameObject.transform.position).normalized;

                direccion *= gameObject.GetComponent<VillagerScript>().movSpeed * Time.fixedDeltaTime;

                gameObject.transform.position += direccion * 0.5f;

                if (Vector3.Distance(transform.position, camino.vectorPath[puntoActual]) < distanciaSiguientePunto)
                {
                    puntoActual++;
                    return;
                }
                else
                {

                }

            }
        }
    }
}