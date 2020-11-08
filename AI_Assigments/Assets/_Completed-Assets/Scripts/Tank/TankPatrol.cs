using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankPatrol : MonoBehaviour
{
    public GameObject[] patrol_points;

    private int current_point = 0;

    public NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;

        Patrol();
    }
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 1.0f)
        {
            agent.autoBraking = true;
            Patrol();
        }
        else
        {
            agent.autoBraking = false;
        }
    }
    void Patrol()
    {

        agent.destination = patrol_points[current_point].gameObject.transform.position;

        current_point = (current_point + 1) % patrol_points.Length;
    }
}
