using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointPath : MonoBehaviour
{

    public Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public int GetNextWaypointIndex(int currentWaypoint)
    {
        int nextWaypointIndex = currentWaypoint + 1;

        if(nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0; 
        }

        return nextWaypointIndex;
    }
}
