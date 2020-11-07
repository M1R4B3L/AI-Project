using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankPatrol : MonoBehaviour
{
    // Start is called before the first frame update

    public List<GameObject> patrol_points;
    //private int destPoint = 0;
    private NavMeshAgent agent;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.autoBraking = false;

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
      if(patrol_points.Count == 0)
      {
            return;
      }

      for(int i = 0; i < patrol_points.Count; i++)
      {
            agent.destination = patrol_points[i].GetComponentInChildren<Transform>().position;

            i = (i + 1) % patrol_points.Count;
      }
    }


    void Update()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            GotoNextPoint();
    }
}
