using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HomingMissile : MonoBehaviour
{
    // Variables
    public Transform target;
    public float movementSpeed = 10f;
    public float rotationSpeed = 10f;

    [Header("PREDICTION")]
    public float maxDistancePredict = 100;
    public float minDistancePredict = 5;
    public float maxTimePrediction = 5;
    private Vector3 standardPrediction;
    private Vector3 deviatedPrediction;

    // References
    private ShipMovement shipMovement;

    void Start()
    {
        shipMovement = target.GetComponent<ShipMovement>();
    }

    void Update()
    {
        MoveUsingShipSpeed();

        var leadTimePercentage = Mathf.InverseLerp(minDistancePredict, maxDistancePredict, Vector3.Distance(transform.position, target.transform.position));
    }

    void MoveUsingShipSpeed()
    {
        transform.Translate(Vector3.up * shipMovement.tempSpeed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        /*  Gizmos.color = Color.red;
          Gizmos.DrawLine(transform.position, standardPrediction);
          Gizmos.color = Color.green;
          Gizmos.DrawLine(standardPrediction, deviatedPrediction);*/
    }
}
