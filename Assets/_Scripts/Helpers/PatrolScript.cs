using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolScript : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField]
    float pointRadius;
    public PatrolType patrolType;
    private bool goingBackwards = false;
    public enum PatrolType 
    { 
        OneWay,
        PingPong,
        Loop
    }



    private void OnDrawGizmos()
    {
        foreach (Transform child in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(child.position, pointRadius);
        }
        Gizmos.color = Color.red;
        for(int i = 0; i < transform.childCount -1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i+1).position);
        }
        switch (patrolType)
        {
            case PatrolType.OneWay:
                break;
            case PatrolType.PingPong:
                break;
            case PatrolType.Loop:
                Gizmos.DrawLine(transform.GetChild(transform.childCount -1).position, transform.GetChild(0).position);
                break;
        }

    }

    public Transform GetNextPoint (Transform currentPatrolPoint)
    {
        if (currentPatrolPoint == null) return transform.GetChild(0);
        switch (patrolType)
        {
            case PatrolType.OneWay:
                if (currentPatrolPoint.GetSiblingIndex() < transform.childCount - 1)
                {
                    return transform.GetChild(currentPatrolPoint.GetSiblingIndex() + 1);
                }
                else
                {
                    return null;
                }
            case PatrolType.PingPong:
                if (!goingBackwards)
                {
                    if (currentPatrolPoint.GetSiblingIndex() < transform.childCount - 1)
                    {
                        return transform.GetChild(currentPatrolPoint.GetSiblingIndex() + 1);
                    }
                    else
                    {
                        goingBackwards = true;
                        return transform.GetChild(currentPatrolPoint.GetSiblingIndex() - 1);
                    }
                }
                else if (goingBackwards)
                {
                    if(currentPatrolPoint.GetSiblingIndex() > 0)
                    {
                        return transform.GetChild(currentPatrolPoint.GetSiblingIndex() - 1);
                    }
                    else
                    {
                        goingBackwards = false;
                        return transform.GetChild(currentPatrolPoint.GetSiblingIndex() + 1);

                    }
                }
                break;
                
            case PatrolType.Loop:
                return transform.GetChild((currentPatrolPoint.GetSiblingIndex() + 1) % transform.childCount);

                
        }
        return null;
    }
    public Transform GetStartPoint()
    {
        goingBackwards = false;
        return transform.GetChild(0);
    }
}