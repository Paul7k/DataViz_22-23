using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwapper : MonoBehaviour
{
    public List<GameObject> cams = new List<GameObject>();
    private Controls _controls;

    private void Awake()
    {
        _controls = new Controls();
        
        _controls.CamSwapping.Persp.performed += ctx => ActivateCam(0);
        _controls.CamSwapping.Front.performed += ctx => ActivateCam(1);
        _controls.CamSwapping.Side.performed += ctx => ActivateCam(2);
        _controls.CamSwapping.Top.performed += ctx => ActivateCam(3);
    }
    
    private void OnEnable()
    {
        _controls.CamSwapping.Enable();
    }

    private void OnDisable()
    {
        _controls.CamSwapping.Disable();
    }

    private void ActivateCam(int index)
    {
        foreach (var cam in cams)
        {
            cam.SetActive(false);
        }
        cams[index].SetActive(true);
    }
}
