using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class ShipController : MonoBehaviour
{
    // Variables
    public bool move = false;
    public float speed = 10f;
    public float boostSpeed = 15f;
    public float yawSpeed;
    public float pitchSpeed;
    public float rollSpeed;
    public float rollSpeedAlt;

    private InputActions inputActions;

    private Vector3 testPos;

    float yawTemp = 0;
    float pitchTemp = 0;
    float rollTemp = 0;

    private float lastYaw = 0f;
    private float lastPitch = 0f;
    private float lastRoll = 0f;

    private bool isBoosted = false;

    void Start()
    {
        // Cache
        inputActions = new InputActions();
        inputActions.Enable();
    }

    void Update()
    {
        HandleInput();

        if (move)
        {
            MoveShip();
        }
    }

    void HandleInput()
    {
        isBoosted = inputActions.Default.Boost.IsPressed();

        bool yawPressed = false;
        bool pitchPressed = false;
        bool rollPressed = false;

        if (inputActions.Default.Yaw.IsPressed())
        {
            yawTemp = inputActions.Default.Yaw.ReadValue<float>();
            yawPressed = true;
        }

        if (inputActions.Default.Pitch.IsPressed())
        {
            pitchTemp = inputActions.Default.Pitch.ReadValue<float>();
            pitchPressed = true;
        }

        if (inputActions.Default.Roll.IsPressed())
        {
            rollPressed = true;
        }

        rollTemp = inputActions.Default.Roll.ReadValue<float>();

        if (yawPressed && pitchPressed)
        {

            ControlShip(yawTemp, pitchTemp, rollTemp, !rollPressed);
        }
        else if (yawPressed)
        {

            ControlShip(yawTemp, lastPitch, rollTemp, !rollPressed);
        }
        else if (pitchPressed)
        {

            ControlShip(lastYaw, pitchTemp, rollTemp, !rollPressed);
        }
        else
        {

            ControlShip(lastYaw, lastPitch, rollTemp, !rollPressed);
        }
    }

    void ControlShip(float yaw, float pitch, float roll, bool isAlt = false)
    {
        lastYaw = Mathf.Lerp(lastYaw, yaw, yawSpeed * Time.deltaTime);
        lastPitch = Mathf.Lerp(lastPitch, pitch, pitchSpeed * Time.deltaTime);
        lastRoll = Mathf.Lerp(lastRoll, roll, isAlt ? rollSpeedAlt : rollSpeed * Time.deltaTime);

        Vector3 rot = new(lastPitch, lastYaw, lastRoll);

        Quaternion quat = Quaternion.Euler(rot);

        transform.rotation = quat;
    }

    void MoveShip()
    {
        float tempSpeed = isBoosted ? boostSpeed : speed;

        transform.Translate(Vector3.forward * tempSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(testPos, 10f);
    }
}
