using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MapZone : MonoBehaviour
{
    [SerializeField] private Transform _lUCorner, _rDCorner;

    private void OnDrawGizmos()
    {
        Vector3 LU = _lUCorner.position;
        Vector3 RU = new Vector3(_rDCorner.position.x , _lUCorner.position.y, _lUCorner.position.z);
        Vector3 RD = _rDCorner.position;
        Vector3 LD = new Vector3(_lUCorner.position.x, _rDCorner.position.y, _rDCorner.position.y);

        Gizmos.DrawLine(LU,RU);
        Gizmos.DrawLine(RU,RD);
        Gizmos.DrawLine(RD,LD);
        Gizmos.DrawLine(LD,LU);
    }

    public Vector3 GetRandomPoint()
    {
        Vector3 LU = _lUCorner.position;
        Vector3 RU = new Vector3(_rDCorner.position.x, _lUCorner.position.y, _lUCorner.position.z);
        Vector3 RD = _rDCorner.position;
        Vector3 LD = new Vector3(_lUCorner.position.x, _rDCorner.position.y, _rDCorner.position.y);

        return new Vector3(Random.Range(LU.x,RU.x),Random.Range(LU.y,RD.y),0);
    }

}
