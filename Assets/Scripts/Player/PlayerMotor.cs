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
    private Vector3 _balloonPositions;
    private Collider2D _collider;

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
        _playerPositions = transform.position;//_playerObject.transform.position;
        _balloonPositions = _balloonObject.transform.position;

        _collider = _balloonObject.GetComponent<Collider2D>();
    }
    /*
    private void FixedUpdate()
    {
        dt = Time.deltaTime;
        
        if (vertical != 0) _playerHealth.changeHealth(-1);

        if (param.balloon)
        {
           // Debug.Log("Balloon");
            BalloonPhysics();

        }
        else
        {
            if (vertical != 0) Debug.Log(vertical);
            _velocity += ((vertical * param.vertical + param.lift - param.drag * _velocity) / param.mass) * dt;
            vertical = 0;

            Vector3 tempDir = _playerPositions - _playerObject.transform.position;
            tempDir.y = 0;

            RaycastHit2D[] hits = new RaycastHit2D[10];
            if (_collider.Cast(new Vector2(tempDir.x, 0), hits, tempDir.x*3) == 0)
            {
                _horizontal_velocity += (((horizontal * param.horizontal) + wind - param.drag * _horizontal_velocity) / param.mass) * dt;
            }

           
            _playerPositions = _playerPositions + new Vector3(_horizontal_velocity * dt, _velocity * dt, 0);
        }

        if (hit)
        {
            hit = false;
            PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.CONTROLLABLE);

        }

        if (param.balloon) _balloonObject.transform.position = RoundPosition(_balloonPositions);
        _playerObject.transform.position = RoundPosition(_playerPositions);



    }

    private void BalloonPhysics()
    {
        _theta = Mathf.Asin((_playerPositions.x - _balloonPositions.x) / param.radius);

        float torque = (horizontal * param.horizontal * Mathf.Cos(_theta) - param.mass * param.gravity * Mathf.Sin(_theta)) / param.mass;
        float axialForce = (horizontal * param.horizontal + wind);  // * Mathf.Abs(Mathf.Sin(_theta));

        _dtheta += ((torque - param.balloon_damp * _dtheta) / param.balloon_damp) * dt;

        _theta += _dtheta * dt;
        Debug.Log("theta:" + _theta);


        Vector3 tempDir = _playerPositions - _playerObject.transform.position;
        tempDir.y = 0;
        tempDir.Normalize();
        float newX = 0;

        RaycastHit2D[] hits = new RaycastHit2D[10];
        if (_collider.Cast(new Vector2(tempDir.x,0), hits, tempDir.x * 0.1f) == 0)
        {
            newX = dt * dt * axialForce / (param.ballon_mass);
        }

        

        _velocity = _velocity + ((vertical * param.vertical + param.lift - param.drag * _velocity) / param.mass) * dt;
        vertical = 0;

        Debug.Log("Vel:" + _velocity * dt);

        _balloonPositions = _balloonPositions + new Vector3(newX, _velocity * dt, 0);
        _playerPositions = _balloonPositions + new Vector3(param.radius * Mathf.Sin(_theta), -param.radius * Mathf.Cos(_theta), 0);

        _playerObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * _theta);
        _balloonObject.transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * _theta);
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
        float valueInPixels = Mathf.Round(unityUnits * 100);
        return valueInPixels * (1 / 100f);
    }

    private Vector3 RoundPosition(Vector3 pos)
    {
        pos.x = RoundToNearestPixel(pos.x);
        pos.y = RoundToNearestPixel(pos.y);

        return pos;
    }
    */
}
