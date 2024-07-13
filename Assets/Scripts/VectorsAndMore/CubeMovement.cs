using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    // Variables
    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;
    public float minDistanceTillInput = 0.2f;
    public float minDistanceTillStopping = 0.01f;
    public float minAngleTillInput = 0.2f;
    public float minAngleTillStopping = 0.01f;

    private InputActions inputActions;

    private bool rotating = false;
    private bool moving = false;
    private bool canTakeInput = true;
    private Vector3 targetPos = Vector3.zero;
    private Quaternion targetRot = Quaternion.identity;

    void Start()
    {
        inputActions = new InputActions();
        inputActions.Enable();

        targetRot = transform.rotation;
    }

    void Update()
    {
        Move();
    }

    void OnMoveUp()
    {
        MoveAndRotate(Vector3.forward);
    }

    void OnMoveDown()
    {
        MoveAndRotate(-Vector3.forward);
    }

    void OnMoveLeft()
    {
        MoveAndRotate(-Vector3.right);
    }

    void OnMoveRight()
    {
        MoveAndRotate(Vector3.right);
    }

    void Move()
    {
        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < minDistanceTillStopping)
            {
                transform.position = targetPos;
                moving = false;
            }

            if (Vector3.Distance(transform.position, targetPos) < minDistanceTillInput)
            {
                canTakeInput = true;
            }
        }

        if (rotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            if (Quaternion.Angle(transform.rotation, targetRot) < minAngleTillStopping)
            {
                transform.rotation = targetRot;
                rotating = false;
            }

            if (Quaternion.Angle(transform.rotation, targetRot) < minAngleTillInput)
            {
                canTakeInput = true;
            }
        }
    }

    void MoveAndRotate(Vector3 newDirection)
    {
        if (!moving && !rotating)
        {
            targetPos = transform.position + newDirection;
        }
        else if (canTakeInput)
        {
            targetPos = targetPos + newDirection;
        }else{
            return;
        }

        moving = true;
        rotating = true;
        canTakeInput = false;

        Vector3 crossProduct = Vector3.Cross(newDirection, Vector3.up);
        targetRot *= Quaternion.Euler(-(crossProduct * 90));
    }
}
