using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMvmtThr : MonoBehaviour
{
    public float speed;
    public float rotationSpeed = 25f;
    private Transform target;
    private int waypointIdx = 0;

    void Start()
    {
        target = EnemyPathThr.waypoints[0];
    }

    // move btw waypoints
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        RotateTowardsTarget(dir);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            GetNextWaypoint();
        }
    }

    void RotateTowardsTarget(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion adjustedRotation = targetRotation * Quaternion.Euler(-90f, -90f, 0f); // manual adjustment
        transform.rotation = Quaternion.Slerp(transform.rotation, adjustedRotation, rotationSpeed * Time.deltaTime);
    }

    void GetNextWaypoint()
    {
        // kill enemy at last waypt
        if (waypointIdx >= EnemyPathThr.waypoints.Length - 1)
        {
            Destroy(gameObject);
            return;
        }

        waypointIdx++;
        target = EnemyPathThr.waypoints[waypointIdx];
    }
}
