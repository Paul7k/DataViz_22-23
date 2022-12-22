using System;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    private Camera _cam;
    private Controls _controls;
    private Transform _transform;
    public Vector3 move;
    public Vector2 look;
    public float moveSpeed = 1f;
    public float lookSpeed = 1f;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _controls = new Controls();
        _transform = transform;
        
        _controls.MoveLook.Move.performed += ctx => move = ctx.ReadValue<Vector3>();
        _controls.MoveLook.Move.canceled += ctx => move = Vector3.zero;
        
        _controls.MoveLook.Look.performed += ctx => look = ctx.ReadValue<Vector2>();
        _controls.MoveLook.Look.canceled += ctx => look = Vector2.zero;
        
        _controls.MoveLook.Debug.performed += ctx => Test();
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
        
        var euler = _transform.rotation.eulerAngles;
        //Inverted x and y due to the rotation around the corresponding axis
        euler.x += Time.deltaTime * lookSpeed * -look.y;
        euler.y += Time.deltaTime * lookSpeed * look.x;
        _transform.rotation = Quaternion.Euler(euler);
    }

    private void Test()
    {
        Debug.Log("Gamepad works");
    }
}