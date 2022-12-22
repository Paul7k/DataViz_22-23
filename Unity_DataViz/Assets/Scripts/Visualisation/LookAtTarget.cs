using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target;

    private void Awake()
    {
        if (target == null)
            target = GameObject.FindWithTag("MainCamera").transform;
    }

    private void Update()
    {
        transform.LookAt(2 * transform.position - target.position);
    }
}
