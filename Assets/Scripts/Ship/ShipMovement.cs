using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ShipMovement : MonoBehaviour
{
    // Variables
    public bool canMove = false;

    [Header("Speed")]
    public float speed = 10f;
    public float boostSpeed = 15f;
    public float boostEnterSpeed = 3f;
    public float boostLeaveSpeed = 3f;

    [Header("Movement")]
    public float yawSpeed;
    public float pitchSpeed;
    public float rollSpeed;
    public float rollSpeedAlt;
    public float maxRollAngle = 10f;
    public float minFlyHeight = 30f;
    public AnimationCurve rollCurve;

    [Header("Stats")]
    [ReadOnly]
    public bool isBoosted = false;

    [ReadOnly]
    public float tempSpeed = 0;

    [HideInInspector]
    public float lastYaw = 0f;
    [HideInInspector]
    public float lastPitch = 0f;
    [HideInInspector]
    public float lastRoll = 0f;

    // References
    private ShipInput shipInput;

    void Start()
    {
        // Cache
        shipInput = GetComponent<ShipInput>();

        tempSpeed = speed;
    }

    void Update()
    {
        if (canMove)
        {
            MoveShip();

            CheckMinFlyHeight();
        }
    }

    public void ControlShip(float yaw, float pitch, float roll, bool isAlt = false)
    {
        lastYaw = Mathf.Lerp(lastYaw, yaw, yawSpeed * Time.deltaTime);
        lastPitch = Mathf.Lerp(lastPitch, pitch, pitchSpeed * Time.deltaTime);

        float clampedRoll = Mathf.Clamp(-roll, -maxRollAngle, maxRollAngle);
        lastRoll = Mathf.Lerp(lastRoll, clampedRoll, rollCurve.Evaluate(isAlt ? rollSpeedAlt : rollSpeed * Time.deltaTime));

        Vector3 rot = new(lastPitch, lastYaw, lastRoll);

        Quaternion quat = Quaternion.Euler(rot);

        transform.rotation = quat;
    }

    void MoveShip()
    {
        tempSpeed = isBoosted ? BoostLerp(boostSpeed, tempSpeed, boostEnterSpeed) : BoostLerp(speed, tempSpeed, boostLeaveSpeed);

        transform.Translate(Vector3.forward * tempSpeed * Time.deltaTime);
    }

    float BoostLerp(float targetSpeed, float currentSpeed, float lerpSpeed)
    {
        return Mathf.Lerp(currentSpeed, targetSpeed, lerpSpeed * Time.deltaTime);
    }

    void CheckMinFlyHeight()
    {
        // TODO - FIX PITCH 
        if (transform.position.y <= minFlyHeight)
        {
            shipInput.ignorePositivePitchInput = true;

            if (lastPitch < -0.1 && transform.position.y > minFlyHeight)
            {
                shipInput.minFlyHeightInverse = 0;
            }
            else
            {
                shipInput.minFlyHeightInverse = -360;

                if ((transform.position.y - minFlyHeight) < 5)
                {
                    transform.position = new Vector3(transform.position.x, minFlyHeight, transform.position.z);
                }
            }
        }
        else
        {
            shipInput.ignorePositivePitchInput = false;
        }
    }
}
