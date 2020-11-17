using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor2 : MonoBehaviour
{

    [SerializeField] GameObject _playerObject;
    [SerializeField] GameObject _balloonObject;
    [SerializeField] GameObject _webObject;

    private Collider2D _playerCollider;
    private Rigidbody2D _balloonRigibody;

    private Vector3 _playerPosition;
    private Vector3 _balloonPosition;

    public PhysicsParameters param;

    private RaycastHit2D[] _hits = new RaycastHit2D[10];

    public float applyVertical = 0;
    public float applyHorizontal = 0;

    private float dt = 0;
    private float _verticalVel = 0;
    private float _horizontalVel = 0;
    private float _theta = 0, _dtheta = 0;
    private Vector2 _hitForce = new Vector2(0,0);
   
    private float tilemapCollisionMargin = 0.08f;

    private bool isDead = false;

    private GameObject _activeSpawn;

    private void Awake()
    {
        _playerPosition = _playerObject.transform.position;
        _balloonPosition = _balloonObject.transform.position;
        _playerCollider = GetComponent<Collider2D>();
        _balloonRigibody = _balloonObject.GetComponent<Rigidbody2D>();

       
    }

    private void FixedUpdate()
    {

        if(!isDead)
        {

        
            if(applyHorizontal == 1) _playerObject.transform.eulerAngles = new Vector3(0, 0, 0);
            if (applyHorizontal == -1) _playerObject.transform.eulerAngles = new Vector3(0, 180, 0);

       

            dt = Time.deltaTime;
            _balloonRigibody.AddForce(new Vector2(applyHorizontal*param.horizontalVel, -applyVertical*param.verticalForce));
     

            PlayerPhysics();

            _playerObject.transform.position = RoundPosition(_playerPosition);
            _balloonObject.transform.position = RoundPosition(_balloonObject.transform.position);
            applyVertical = 0;

            /*
            if (isHit)
            {
                applyHorizontal = 0;
                isHit = false;
                _hitForce = new Vector2(0,0);
                PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.CONTROLLABLE);

            }
            */
        }


    }



    private void PlayerPhysics()
    {
        //_theta = Mathf.Asin((_playerPosition.x - _balloonPosition.x) / param.radius);

        float torque = (applyHorizontal * param.horizontalVel * Mathf.Cos(_theta) - param.gravity * Mathf.Sin(_theta)) / param.mass;
        _dtheta += ((torque - param.damp * _dtheta) / param.mass) * dt;
        _theta += _dtheta * dt;

        _playerPosition = _balloonObject.transform.position + new Vector3(param.radius * Mathf.Sin(_theta), -param.radius * Mathf.Cos(_theta), 0);
        //_playerObject.transform.eulerAngles = new Vector3(0, _playerObject.transform.eulerAngles.y, Mathf.Rad2Deg * _theta);
        _webObject.transform.eulerAngles = new Vector3(0, 0, 1.5f*Mathf.Rad2Deg * _theta);
        //_balloonObject.transform.eulerAngles = new Vector3(0, Mathf.Rad2Deg * _theta / 2, 0);
    }

    public void HitPlayer(float forceX, float forceY)
    {
        //isHit = true;
        _hitForce.x = forceX;
        _hitForce.y = forceY;

    }

    public void Death()
    {
        //fall to spawn
        PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.DEAD);
        _balloonObject.GetComponent<SpriteRenderer>().enabled = false;
        _playerCollider.enabled = false;
        _balloonRigibody.Sleep();
        isDead = true;
        applyVertical = 0;
        applyHorizontal = 0;
        _verticalVel = 0;
        _horizontalVel = 0;


        StartCoroutine(FallDown(3));
        
    }

    IEnumerator FallDown(float speed)
    {
        float vel = 3;
        while(_balloonObject.transform.position.y > _activeSpawn.transform.position.y)
        {
            float dt = Time.deltaTime;
            vel += speed * dt;
            _balloonObject.transform.position += new Vector3(0, -vel * dt, 0);
            yield return new WaitForFixedUpdate();
        }

        _balloonObject.transform.position = _activeSpawn.transform.position;
        _balloonObject.GetComponent<SpriteRenderer>().enabled = true;
        _playerCollider.enabled = true;
        PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.CONTROLLABLE);
        _balloonRigibody.WakeUp();
        isDead = false;
    }

    public void SetNewSpawn(GameObject spawn)
    {
        _activeSpawn = spawn;
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

}
