using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Variables
    public Transform target;
    [Range(-360, 360)]
    public float orbitX = 0;
    [Range(-360, 360)]
    public float orbitY = 0;
    public float distance = 100f;
    public float lookSpeed = 5f;
    public float followOffset = 10f;
    public float followSpeed = 5f;
    private Vector3 followTarget;

    void Awake()
    {
        if (target == null)
        {
            throw new Exception("Target is null!");
        }
    }

    void Update()
    {
        CalcOrbit();
    }

    void OnValidate()
    {
        CalcOrbit();

        LookAtTarget();

        FollowTarget();
    }

    void LateUpdate()
    {
        LookAtTarget();

        FollowTarget();
    }

    void LookAtTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, lookSpeed * Time.deltaTime);
    }

    void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, followTarget, followSpeed * Time.deltaTime);
    }

    void CalcOrbit()
    {
        Vector3 orbitVector = new Vector3(orbitX, orbitY, 0);

        Quaternion orbitQuat = Quaternion.Euler(orbitVector);

        Vector3 orbitForward = orbitQuat * Vector3.forward;

        followTarget = target.TransformPoint(orbitForward * distance);
    }
}
