using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolUser : MonoBehaviour
{
    [SerializeField]
    private PatrolScript patrol;
    private Transform currentPatrolpoint = null;

    public Transform NextPoint()
    {
        currentPatrolpoint = patrol.GetNextPoint(currentPatrolpoint);
        return currentPatrolpoint;
    }

    public Transform StartPoint()
    {
        currentPatrolpoint = patrol.GetStartPoint();
        return currentPatrolpoint;
    }


}
