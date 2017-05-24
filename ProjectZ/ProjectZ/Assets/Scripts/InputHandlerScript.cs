using UnityEngine;
using System.Collections;

public class InputHandlerScript : MonoBehaviour
{
    //ACCESOS DE TECLADO
    //Camera
     KeyCode _cameraUpKey1 = KeyCode.UpArrow;
     //KeyCode _cameraUpKey2 = KeyCode.W;
     KeyCode _cameraDownKey1 = KeyCode.DownArrow;
     //KeyCode _cameraDownKey2 = KeyCode.S;
     KeyCode _cameraLeftKey1 = KeyCode.LeftArrow;
     //KeyCode _cameraLeftKey2 = KeyCode.A;
     KeyCode _cameraRightKey1 = KeyCode.RightArrow;
    //KeyCode _cameraRightKey2 = KeyCode.D;
    KeyCode _centerCameraKey = KeyCode.Space;

    //Control
    KeyCode _continueInteract = KeyCode.E;
    KeyCode _selectionKey1 = KeyCode.Mouse0;
    KeyCode _selectionKey2 = KeyCode.Return;
    KeyCode _keepSelectionKey1 = KeyCode.LeftShift;
    KeyCode _keepSelectionKey2 = KeyCode.RightShift;
    KeyCode _invertSelectionKey1 = KeyCode.LeftControl;
    KeyCode _invertSelectionKey2 = KeyCode.RightControl;
    KeyCode _dontAttack = KeyCode.S;
    KeyCode _Attack = KeyCode.A;
    //Input State
    public Vector3 _mousePosition;
    
    public bool _cameraUp;
    public bool _cameraDown;
    public bool _cameraLeft;
    public bool _cameraRight;
    public bool _centerCamera;

    public bool _mustAttack;
    public bool _attackToggle;
    public bool _selectingBegins;
    public bool _selectingEnds;
    public bool _keepSelection;
    public bool _invertSelection;
    public bool _continue;
    public bool _zoomIn;
    public bool _zoomOut;
    public float d;
   // Use this for initialization
   void Start()
    {
    _attackToggle = true;
        _continue = false;
    }

    // Update is called once per frame
    void Update()
    {
        d = Input.GetAxis("Mouse ScrollWheel");

        //Reseteamos el input
        this.ResetKeys();
        //Checkeamos los nuevos valores
        this.CheckInput();

        if (d > 0f)
        {
            // scroll up
            _zoomIn = true;
        }
        else if (d < 0f)
        {
            // scroll down
            _zoomOut = true;
        }

    }

    private void ResetKeys()
    {
        //Guardamos la posición del ratón, por si alguien hace uso de ella
        this._mousePosition = Input.mousePosition;

        this._cameraUp = false;
        this._cameraRight = false;
        this._cameraDown = false;
        this._cameraLeft = false;
        _centerCamera = false;

        this._selectingBegins = false;
        this._selectingEnds = false;

        //El keepSelection y el invertSelection se resetean cuando se deja de pulsar el botón
    }

    //Handles keyboard and mouse input
    void CheckInput()
    {
        #region Camera

        if (Input.GetKeyDown(_centerCameraKey)) {
            _centerCamera = true;
        }

        if (Input.GetKey(_cameraUpKey1))
            //|| Input.GetKey(_cameraUpKey2))
        {
            this._cameraUp = true;
        }

        if (Input.GetKey(_cameraDownKey1))
        //|| Input.GetKey(_cameraDownKey2))
        {
            this._cameraDown = true;
        }

        if (Input.GetKey(_cameraLeftKey1))
        //|| Input.GetKey(_cameraLeftKey2))
        {
            this._cameraLeft = true;
        }

        if (Input.GetKey(_cameraRightKey1))
        //|| Input.GetKey(_cameraRightKey2))
        {
            this._cameraRight = true;
        }
        #endregion

        #region Control
        if (Input.GetKeyDown(_selectionKey1)
            || Input.GetKeyDown(_selectionKey2))
        {
            this._selectingBegins = true;
        }

        else if (Input.GetKeyUp(_selectionKey1)
            || Input.GetKeyUp(_selectionKey2))
        {
            this._selectingEnds = true;
        }

        if (Input.GetKeyDown(_keepSelectionKey1)
            || Input.GetKeyDown(_keepSelectionKey2))
        {
            this._keepSelection = true;
        }

        if (Input.GetKeyUp(_keepSelectionKey1)
            || Input.GetKeyDown(_keepSelectionKey2))
        {
            this._keepSelection = false;
        }

        else if (Input.GetKeyDown(_invertSelectionKey1)
            || Input.GetKeyUp(_invertSelectionKey2))
        {
            this._invertSelection = true;
        }

        else if (Input.GetKeyUp(_invertSelectionKey1)
            || Input.GetKeyUp(_invertSelectionKey2))
        {
            this._invertSelection = false;
        }

        else if (Input.GetKeyDown(_Attack))
        {
            _attackToggle = true;
            
        }
        else if (Input.GetKeyDown(_dontAttack)) {
            _attackToggle = false;
            
        }
        if (Input.GetKeyDown(_continueInteract)||Input.GetMouseButtonDown(0))
        {
            _continue = true;
        }
        #endregion
    }
}
