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


    [Space(20)]
    public bool set = false;

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
        if (set)
        {
            set = false;

            SetCamera();
        }
    }

    void LateUpdate()
    {
        LookAtTarget();

        FollowTarget();
    }

    void LookAtTarget()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, CalcRotation(), lookSpeed * Time.deltaTime);
    }

    Quaternion CalcRotation()
    {
        Vector3 direction = (target.position - transform.position).normalized;

        return Quaternion.LookRotation(direction);
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

    void SetCamera()
    {
        CalcOrbit();

        transform.position = followTarget;

        transform.rotation = CalcRotation();
    }
}
