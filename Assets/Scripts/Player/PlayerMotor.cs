using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] GameObject _playerObject;
    [SerializeField] GameObject _balloonObject;

    public PhysicsParameters param;

    public float horizontal = 0;
    public float vertical = 0;

    private float _velocity = 0;
    private float dt = 0;
    private bool _grounded = true;

    private float _theta = 0;
    private float _dtheta = 0;

    private void FixedUpdate()
    {
        dt = Time.deltaTime;

        if (param.balloon)
        {
            BalloonPhysics();
            return;
        }

        if (!_grounded)
        {
            _velocity = _velocity + ((vertical * param.vertical + param.lift - param.drag * _velocity) / param.mass) * dt;
            vertical = 0;
            _playerObject.transform.position = _playerObject.transform.position + new Vector3(horizontal * param.horizontal * dt, _velocity * dt, 0);
        }
        else
        {
            _playerObject.transform.position = _playerObject.transform.position + new Vector3(horizontal * dt, 0, 0);
        }

    }

    private void BalloonPhysics()
    {
        _theta = Mathf.Asin((_playerObject.transform.position.x - _balloonObject.transform.position.x) / param.radius);

        float torque = (horizontal * param.horizontal * Mathf.Cos(_theta) - param.mass * 5 * Mathf.Sin(_theta)) / param.mass;
        float axialForce = horizontal * param.horizontal * Mathf.Abs(Mathf.Sin(_theta));

        _dtheta += ((torque - param.balloon_damp * _dtheta) / param.balloon_damp) * dt;

        _theta += _dtheta * dt;
        Debug.Log(_theta);

        float newX = dt * dt * axialForce / (param.ballon_mass);

        _velocity = _velocity + ((vertical * param.vertical + param.lift - param.drag * _velocity) / param.mass) * dt;
        vertical = 0;

        _balloonObject.transform.position = _balloonObject.transform.position + new Vector3(newX, _velocity * dt, 0);
        _playerObject.transform.position = _balloonObject.transform.position + new Vector3(param.radius * Mathf.Sin(_theta), -param.radius * Mathf.Cos(_theta), 0);

    }

    public void UpdateGrounded(bool val)
    {
        _grounded = val;
    }
}
