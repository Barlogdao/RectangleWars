using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AIDetector
{
    public AIDetector(string tag)
    {
        _tag = tag;
    }
    private string _tag;
    int RayCount = 4;
    float radius = 0.35f;
    float distance = 40f;

    public Action<StrategicObjectBase> InteractableDetected;
    public Action<UnitBase> EnemyUnitDetected;
    public Action HeroDetected;
    public Action WaterDetected;

    public void CastRay(Vector2 position, Vector2 direction, LayerMask layerMask)
    {
        for (int i = 0; i < RayCount; i++)
        {

            RaycastHit2D hit = Physics2D.CircleCast(position, radius, direction, distance, layerMask);
            if (hit.collider != null)
            {

                Debug.DrawLine(position, hit.centroid, Color.red);
                // Вода
                if (hit.collider.gameObject.layer == 4)
                {
                    WaterDetected?.Invoke();
                    break;
                }
                // Стратегические объекты
                else if (hit.collider.gameObject.layer == 7)
                {
                    InteractableDetected?.Invoke(hit.collider.GetComponent<StrategicObjectBase>());
                    break;
                }
                // Герои
                else if (hit.collider.gameObject.layer == 13 && !hit.transform.CompareTag(_tag))
                {
                    HeroDetected?.Invoke();
                    break;
                }
                // Юниты
                else if (hit.collider.gameObject.layer != 10 && !hit.transform.CompareTag(_tag))
                {
                    if (hit.collider.gameObject.TryGetComponent<UnitBase>(out var unit))
                    {
                        EnemyUnitDetected?.Invoke(unit);
                        break;
                    }
                }
                position = hit.centroid + hit.normal * 0.005f;
                direction = Vector2.Reflect(direction, hit.normal);
            }
        }
    }
}
