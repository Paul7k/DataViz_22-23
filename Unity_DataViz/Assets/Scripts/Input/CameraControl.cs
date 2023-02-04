using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    private Camera _cam;
    private Controls _controls;
    private Transform _transform;
    public Vector3 move;
    public Vector2 look;
    public Vector2 mouse;
    public Vector2 mouseStartPos;
    public bool useMouseLook;
    public float moveSpeed = 1f;
    public float lookSpeed = 1f;
    public bool escape;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _controls = new Controls();
        _transform = transform;
        
        _controls.MoveLook.MoveController.performed += ctx => move = ctx.ReadValue<Vector3>();
        _controls.MoveLook.MoveController.canceled += ctx => move = Vector3.zero;
        
        _controls.MoveLook.MoveUpDown.performed += ctx => move.y = ctx.ReadValue<float>();
        _controls.MoveLook.MoveUpDown.canceled += ctx => move.y = 0f;
        
        _controls.MoveLook.MoveRightLeft.performed += ctx => move.x = ctx.ReadValue<float>();
        _controls.MoveLook.MoveRightLeft.canceled += ctx => move.x = 0f;
        
        _controls.MoveLook.MoveForwardBack.performed += ctx => move.z = ctx.ReadValue<float>();
        _controls.MoveLook.MoveForwardBack.canceled += ctx => move.z = 0f;
        
        _controls.MoveLook.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        _controls.MoveLook.Look.canceled += ctx => look = Vector2.zero;
        
        _controls.MoveLook.Mouse.performed += ctx => mouse = ctx.ReadValue<Vector2>();
        _controls.MoveLook.Mouse.canceled += ctx => mouse = Vector2.zero;
        
        _controls.MoveLook.MouseClick.performed += ctx => useMouseLook = true;
        _controls.MoveLook.MouseClick.canceled += ctx => useMouseLook = false;
        
        _controls.MoveLook.Debug.performed += ctx => Test();
        
        _controls.MoveLook.Escape.performed += ctx => escape = true;
        _controls.MoveLook.Escape.canceled += ctx => escape = false;
    }

    private void OnEnable()
    {
        _controls.MoveLook.Enable();
    }

    private void OnDisable()
    {
        _controls.MoveLook.Disable();
    }

    private void Update()
    {
        var nextMove = Vector3.zero;
        nextMove += move.z * _transform.forward;
        nextMove += move.x * _transform.right;
        nextMove += move.y * _transform.up;
        _transform.position += Time.deltaTime * moveSpeed * nextMove;
        
        if(escape)
            return;

        var euler = _transform.rotation.eulerAngles;
        if (!useMouseLook)
        {
            //Mouse
            mouseStartPos = mouse;
            
            //Controller
            //Inverted x and y due to the rotation around the corresponding axis
            euler.x += Time.deltaTime * lookSpeed * -look.y;
            euler.y += Time.deltaTime * lookSpeed * look.x;
            _transform.rotation = Quaternion.Euler(euler);
        }
        else
        {
            var deltaPos = mouse - mouseStartPos;
            euler.x += Time.deltaTime * -deltaPos.y;
            euler.y += Time.deltaTime * deltaPos.x;
            _transform.rotation = Quaternion.Euler(euler);
        }
            
        
        
        

        


    }

    private void Test()
    {
        Debug.Log("Gamepad works");
    }
}