using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] GameObject _playerObject;

    public float horizontal = 0;
    public float force = 0;

    private float _mass = 1;
    private float _velocity = 0;
    private float _accel = -4;
    private float dt = 0;

    private void FixedUpdate()
    {
        dt = Time.deltaTime;

        _velocity = _velocity + (force/_mass + _accel )* dt;
        force = 0;
        _playerObject.transform.position = _playerObject.transform.position + new Vector3(horizontal * dt, _velocity * dt, 0) ;
    }
}
