using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInput : MonoBehaviour
{
    // Variables
    private Vector3 testPos;

    private float yawTemp = 0;
    private float pitchTemp = 0;
    private float rollTemp = 0;

    [HideInInspector]
    public bool ignorePositivePitchInput = false;
    [HideInInspector]
    public float minFlyHeightInverse = 0;

    private InputActions inputActions;

    // References
    private ShipMovement shipMovement;

    void Start()
    {
        // Cache
        shipMovement = GetComponent<ShipMovement>();

        inputActions = new InputActions();
        inputActions.Enable();
    }

    void Update()
    {
        HandleInput();

        if (ignorePositivePitchInput)
        {
            shipMovement.lastPitch = Mathf.Lerp(shipMovement.lastPitch, minFlyHeightInverse, shipMovement.pitchSpeed * Time.deltaTime);
        }
    }

    void HandleInput()
    {
        // Get boost
        shipMovement.isBoosted = inputActions.HomingMissile.Boost.IsPressed();

        // Get Pitch, Yaw and Roll
        bool yawPressed = false;
        bool pitchPressed = false;

        if (inputActions.HomingMissile.Yaw.IsPressed())
        {
            yawTemp = inputActions.HomingMissile.Yaw.ReadValue<float>();
            yawPressed = true;
        }

        if (inputActions.HomingMissile.Pitch.IsPressed())
        {
            if (!ignorePositivePitchInput || (ignorePositivePitchInput && inputActions.HomingMissile.Pitch.ReadValue<float>() < 0))
            {
                pitchTemp = inputActions.HomingMissile.Pitch.ReadValue<float>();
                pitchPressed = true;
            }
        }

        rollTemp = inputActions.HomingMissile.Yaw.ReadValue<float>(); // Yaw is used here

        // Control the ship
        if (yawPressed && pitchPressed)
        {

            shipMovement.ControlShip(yawTemp, pitchTemp, rollTemp, !yawPressed);
        }
        else if (yawPressed)
        {

            shipMovement.ControlShip(yawTemp, shipMovement.lastPitch, rollTemp, !yawPressed);
        }
        else if (pitchPressed)
        {
            shipMovement.ControlShip(shipMovement.lastYaw, pitchTemp, rollTemp, !yawPressed);
        }
        else
        {
            shipMovement.ControlShip(shipMovement.lastYaw, shipMovement.lastPitch, rollTemp, !yawPressed);
        }
    }
}
