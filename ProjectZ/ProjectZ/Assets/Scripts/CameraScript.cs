using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    GameLogicScript gameLogic;
    //CONSTANTES DE CÁMARA
    //Velocidad de movimiento de la cámara
    const float CAMERA_SPEED = 20.0f;
    //Margen de pantalla donde se podrá mover la cámara situando allí el ratón
    const int CAMERA_MOVE_MARGINX = 100;
    const int CAMERA_MOVE_MARGINY = 50;

    //Limites de movimiento de camara
    const int TOPLIMIT = 10;
    const int BOTLIMIT = -20;
    const int RIGHTLIMIT = 36;
    const int LEFTLIMIT = -36;

    //Posicion original de la camara
    Vector3 originalPos;

    //El Objeto sobre el cual se gestionan los limites
    GameObject targetObject;
    //Manejador de input
    InputHandlerScript _input;
    
    void Start()
    {
        originalPos = gameObject.transform.position;
        targetObject = GameObject.FindGameObjectWithTag("Ground");
        //Guardamos la referencia al input en nuestra clase
        gameLogic = GameLogicScript.gameLogic;
        _input = gameLogic.GetComponent<InputHandlerScript>();
    }

    void Update()
    {
        if (!gameLogic.isPaused && !gameLogic.eventManager.onEvent)
        {
            if (_input._centerCamera) {
                gameObject.transform.position = originalPos;
            }


            //Declaramos un vector velocidad de la Cámara
            Vector3 cameraVector;

            //Comprobamos si el ratón se encuentra en los margenes de movimiento
            CheckMousePosition(out cameraVector);

            

            //Y ahora comprobamos las entradas del teclado
            if ((_input._cameraUp)&&(gameObject.transform.position.z - targetObject.transform.position.z) <TOPLIMIT)
                cameraVector.z = CAMERA_SPEED;
            else if ((_input._cameraDown) && (gameObject.transform.position.z - targetObject.transform.position.z) > BOTLIMIT)
                cameraVector.z = -CAMERA_SPEED;
            if ((_input._cameraRight)&& (gameObject.transform.position.x - targetObject.transform.position.x)<RIGHTLIMIT)
                cameraVector.x = CAMERA_SPEED;
            else if ((_input._cameraLeft)&& (gameObject.transform.position.x - targetObject.transform.position.x)>LEFTLIMIT)
                cameraVector.x = -CAMERA_SPEED;

            //Movemos la cámara en el vector que hemos especificado
            transform.Translate(cameraVector * Time.deltaTime, Space.World);
        }
    }
    void CheckMousePosition(out Vector3 cameraVector)
    {
        cameraVector = new Vector3();

        if (_input._zoomIn&&gameObject.transform.position.y>4)
        {
            _input._zoomIn = false;
            cameraVector.y = 0;
            cameraVector.y-=5;
        }
        else if (_input._zoomOut&&gameObject.transform.position.y<9)
        {
            _input._zoomOut = false;
            cameraVector.y = 0;
            cameraVector.y+=5;
        }

        if ((_input._mousePosition.x < CAMERA_MOVE_MARGINX) && (gameObject.transform.position.x - targetObject.transform.position.x) > LEFTLIMIT) 
        {
            cameraVector.x = -CAMERA_SPEED;
        }
        else if ((_input._mousePosition.x > (Screen.width - CAMERA_MOVE_MARGINX))&& (gameObject.transform.position.x - targetObject.transform.position.x)<RIGHTLIMIT)
        {
            cameraVector.x = CAMERA_SPEED;
        }

        if (_input._mousePosition.y < CAMERA_MOVE_MARGINY && (gameObject.transform.position.z - targetObject.transform.position.z) > BOTLIMIT)
      
        {
            cameraVector.z = -CAMERA_SPEED;
        }
        else if ((_input._mousePosition.y > (Screen.height - CAMERA_MOVE_MARGINY))&& (gameObject.transform.position.z - targetObject.transform.position.z) < TOPLIMIT)
        {
            cameraVector.z = CAMERA_SPEED;
        }
    }
}