using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    SpriteFrontSide _frontSide;
    private void Start()
    {
        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
        //transform.rotation = Quaternion.AngleAxis(angle, -Vector3.forward);
    }
    private void Update()
    {
        //transform.LookAtTarget(Vector3.zero,_frontSide);
        //Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //transform.up = direction;
        //transform.LookAtTarget(Camera.main.ScreenToWorldPoint(Input.mousePosition), _frontSide);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out UnitBase unit))
        {
            unit.GetTrueDamage(2);
        }
    }


}
