using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCollisionFix : MonoBehaviour
{
    // Variables
    public Transform target;
    public float castRadius = 30f;
    public float castRadiusBoosted = 10f;
    public Vector3 castOffset = new Vector3(0, 0, 70);
    public Vector3 castOffsetBoost = new Vector3(0, 0, 70);
    public LayerMask castLayerMask;
    [Range(-360, 360)]
    public float orbitX = 0;
    [Range(-360, 360)]
    public float orbitY = 0;
    public float distance = 100f;

    [Header("Debug")]
    public bool debug = false;

    private float floorHitFixInput = 0;
    private Vector3 followTarget;

    // References
    private ShipInput shipInput;
    private ShipMovement shipMovement;

    void Start()
    {
        // Cache
        shipInput = GetComponent<ShipInput>();
        shipMovement = GetComponent<ShipMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        CalcOrbit();

        CheckForCollision();
    }

    void OnValidate()
    {
        CalcOrbit();
    }

    void CalcOrbit()
    {
        Vector3 orbitVector = new Vector3(orbitX, orbitY, 0);

        Quaternion orbitQuat = Quaternion.Euler(orbitVector);

        Vector3 orbitForward = orbitQuat * Vector3.forward;

        followTarget = target.TransformPoint(orbitForward * distance);
    }

    void CheckForCollision()
    {
        RaycastHit hit;

        float radius = shipMovement.isBoosted ? castRadiusBoosted : castRadius;
        Vector3 offset = shipMovement.isBoosted ? castOffsetBoost : castOffset;

        if (Physics.SphereCast(followTarget, radius, transform.forward, out hit, radius * 2, castLayerMask, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.name == "Floor")
            {
                FixFloorHit();
            }
            else
            {
                shipInput.ignorePositivePitchInput = false;
            }
        }
        else
        {
            shipInput.ignorePositivePitchInput = false;
        }
    }

    void FixFloorHit()
    {
        shipInput.ignorePositivePitchInput = true;

        if (shipMovement.lastPitch < -0.1)
        {
            floorHitFixInput = 0;
        }
        else
        {
            floorHitFixInput = -360;
        }
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(followTarget, castRadius);
        }
    }
}
