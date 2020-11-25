using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Satellite : Enemy
{
    [SerializeField] float _speed;

    private float dt = 0;

    private void FixedUpdate()
    {
        dt = Time.deltaTime;
        transform.eulerAngles += new Vector3(0, 0, _speed * dt);
    }
}
