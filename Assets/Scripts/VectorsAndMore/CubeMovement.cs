using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMovement : MonoBehaviour
{
    // Variables
    public float moveSpeed = 10f;
    public float rotationSpeed = 5f;
    public float minDistanceTileInput = 0.2f;
    public float minDistanceTileStopping = 0.01f;

    private InputActions inputActions;

    private bool rotating = false;
    private bool moving = false;
    private bool canTakeInput = true;
    private Vector3 targetPos = Vector3.zero;
    private Vector3 targetRot = Vector3.zero;

    void Start()
    {
        inputActions = new InputActions();
        inputActions.Enable();
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

            if (Vector3.Distance(transform.position, targetPos) < minDistanceTileStopping)
            {
                transform.position = targetPos;
                moving = false;
            }

            if (Vector3.Distance(transform.position, targetPos) < minDistanceTileInput)
            {
                canTakeInput = true;
            }
        }

        if (rotating)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRot), rotationSpeed * Time.deltaTime);
        }
    }

    void MoveAndRotate(Vector3 newDirection)
    {
        if (!moving && !rotating)
        {
            moving = true;
            rotating = true;
            canTakeInput = false;

            targetPos = transform.position + newDirection;

            targetRot = Vector3.Cross(newDirection, Vector3.up);

            Debug.Log(targetRot);
        }
        else if (canTakeInput)
        {
            moving = true;
            rotating = true;
            canTakeInput = false;

            targetPos = targetPos + newDirection;
        }
    }
}
