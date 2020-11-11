using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMotor : MonoBehaviour
{
    [SerializeField] GameObject _playerObject;
    [SerializeField] GameObject _balloonObject;

    public PhysicsParameters param;
    private PlayerHealth _playerHealth;
    private Vector3 _playerPositions;

    public float horizontal = 0;
    public float vertical = 0;

    public float wind = 0;

    private float _velocity = 0;
    private float _horizontal_velocity = 0;
    private float dt = 0;
    private bool hit = false;

    private float _theta = 0;
    private float _dtheta = 0;

    private void Awake()
    {
        _playerHealth = GetComponent<PlayerHealth>();
        _playerPositions = transform.position;
    }

    private void FixedUpdate()
    {
        dt = Time.deltaTime;
        
        if (vertical != 0) _playerHealth.changeHealth(-1);

        if (param.balloon)
        {
            Debug.Log("Balloon");
            BalloonPhysics();
            return;
        }
        else
        {
            if (vertical != 0) Debug.Log(vertical);
            _velocity += ((vertical * param.vertical + param.lift - param.drag * _velocity) / param.mass) * dt;
            vertical = 0;
            _horizontal_velocity += (((horizontal * param.horizontal) + wind - param.drag * _horizontal_velocity) / param.mass) * dt;
            _playerPositions = _playerPositions + new Vector3(_horizontal_velocity * dt, _velocity * dt, 0);
        }

        if (hit)
        {
            hit = false;
            PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.CONTROLLABLE);

        }

        _playerObject.transform.position = RoundPosition(_playerPositions);

    }

    private void BalloonPhysics()
    {
        _theta = Mathf.Asin((_playerPositions.x - _balloonObject.transform.position.x) / param.radius);

        float torque = (horizontal * param.horizontal * Mathf.Cos(_theta) - param.mass * 5 * Mathf.Sin(_theta)) / param.mass;
        float axialForce = (horizontal * param.horizontal + wind) * Mathf.Abs(Mathf.Sin(_theta));

        _dtheta += ((torque - param.balloon_damp * _dtheta) / param.balloon_damp) * dt;

        _theta += _dtheta * dt;
        Debug.Log(_theta);

        float newX = dt * dt * axialForce / (param.ballon_mass);

        _velocity = _velocity + ((vertical * param.vertical + param.lift - param.drag * _velocity) / param.mass) * dt;
        vertical = 0;

        _balloonObject.transform.position = _balloonObject.transform.position + new Vector3(newX, _velocity * dt, 0);
        _playerPositions = _balloonObject.transform.position + new Vector3(param.radius * Mathf.Sin(_theta), -param.radius * Mathf.Cos(_theta), 0);

    }

    public void HitPlayer(float forceX, float forceY)
    {
        hit = true;
        horizontal = forceX;
        vertical = forceY;
    }

    public void Death()
    {
        vertical = 0;
        horizontal = 0;
        _horizontal_velocity = 0;
        param = new PhysicsParameters();
        param.lift = -10;
        param.balloon = false;
        param.mass = 1;
    }

    private float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = Mathf.Round(unityUnits * 64);
        return valueInPixels * (1 / 64f);
    }

    private Vector3 RoundPosition(Vector3 pos)
    {
        pos.x = RoundToNearestPixel(pos.x);
        pos.y = RoundToNearestPixel(pos.y);

        return pos;
    }
}
