using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor2 : MonoBehaviour
{

    [SerializeField] GameObject _playerObject;
    [SerializeField] GameObject _balloonObject;
    [SerializeField] GameObject _webObject;

    [SerializeField] AudioClip _popClip;

    private Animator _balloonAnimator;

    private Collider2D _playerCollider;
    private Rigidbody2D _balloonRigibody;
    private PlayerHealth _playerHealth;

    private Vector3 _playerPosition;
    private Vector3 _balloonPosition;

    public PhysicsParameters param;


    public float applyVertical = 0;
    public float applyHorizontal = 0;

    private float dt = 0;
    private float _theta = 0, _dtheta = 0;
    private Vector2 _hitForce = new Vector2(0,0);

    private bool isDead = false;

    private GameObject _activeSpawn;

    private void Awake()
    {
        _playerPosition = _playerObject.transform.position;
        _balloonPosition = _balloonObject.transform.position;
        _playerCollider = GetComponent<Collider2D>();
        _balloonRigibody = _balloonObject.GetComponent<Rigidbody2D>();
        _playerHealth = _balloonObject.GetComponent<PlayerHealth>();
        _balloonAnimator = GetComponent<Animator>();



    }

    private void FixedUpdate()
    {

        if(!isDead)
        {
            
            if (applyHorizontal == 1)
            {
                _playerObject.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            else if (applyHorizontal == -1)
            {
                 _playerObject.transform.eulerAngles = new Vector3(0, 180, 0);
            }
            else
            {
                
            }


            //if (applyVertical != 0) _playerHealth.changeHealth(-1);


            dt = Time.deltaTime;
            _balloonRigibody.AddForce(new Vector2(applyHorizontal*param.horizontalVel, -applyVertical*param.verticalForce));
     

            PlayerPhysics();

            _playerObject.transform.position = RoundPosition(_playerPosition);
            _balloonObject.transform.position = RoundPosition(_balloonObject.transform.position);
            applyVertical = 0;

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
        _hitForce.x = forceX;
        _hitForce.y = forceY;

    }

    public Animator GetPlayerAnimator()
    {
        return _playerObject.GetComponent<Animator>();
    }

    public void Death()
    {
        //fall to spawn
        _balloonAnimator.SetBool("Pop", true);
        PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.DEAD);
        AudioManager.Instance.PlaySFX(_popClip);
       // _balloonObject.GetComponent<SpriteRenderer>().enabled = false;
        _playerCollider.enabled = false;
        _balloonRigibody.Sleep();
        isDead = true;
        applyVertical = 0;
        applyHorizontal = 0;

        StartCoroutine(FallDown(3));
        
    }

    IEnumerator FallDown(float speed)
    {
        float vel = 3;
        _balloonAnimator.SetBool("Pop", true);
        yield return new WaitForSecondsRealtime(0.15f);
        _balloonObject.GetComponent<SpriteRenderer>().enabled = false;
        _balloonAnimator.SetBool("Pop", false);

        while (_balloonObject.transform.position.y > _activeSpawn.transform.position.y)
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
        float valueInPixels = Mathf.Round(unityUnits * 128);
        return valueInPixels * (1 / 128f);
    }

    private Vector3 RoundPosition(Vector3 pos)
    {
        pos.x = RoundToNearestPixel(pos.x);
        pos.y = RoundToNearestPixel(pos.y);

        return pos;
    }

}
