using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class PlayerManager : Singleton<PlayerManager>
{
    public enum PlayerState
    {
        UNACTIVE,
        CONTROLLABLE,
        HIT,
        DEAD,
    }


    public PlayerState _currentPlayerState = PlayerState.UNACTIVE;
    public PlayerType.type _currentPlayerType = PlayerType.type.NULL;

    //[SerializeField] GameObject _playerPrefab;
    

    [SerializeField] private float balloonLift = 15;
    [SerializeField] private float balloonDrag = 1;
    [SerializeField] private float balloonDamp = 1;
    [SerializeField] private float mass = 1;
    [SerializeField] private float gravity = -3;
    [SerializeField] private HUD _hud;

    private GameObject _currentPlayer;
    private PlayerMotor2 _playerMotor;
    private PlayerHealth _playerHealth;
    private PhysicsParameters param;

    private Animator _playerAnim;
   

    private void Start()
    {
        
        DontDestroyOnLoad(gameObject);
        



    }

    private void Update()
    {

        if (_currentPlayerState != PlayerState.UNACTIVE)
        {
            _hud.UpdateAltimeter(_currentPlayer.gameObject.transform.position.y);
        }    


        if (_currentPlayerState == PlayerState.CONTROLLABLE)
        {
            
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _playerMotor.Death();
                _playerAnim.SetBool("IsMoving", false);
            }
            if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                _playerMotor.applyHorizontal = 1;
                _playerAnim.SetBool("IsMoving", true);
                //move right
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                _playerMotor.applyHorizontal = -1;
                _playerAnim.SetBool("IsMoving", true);
                //move left
            }
            else
            {
                _playerMotor.applyHorizontal = 0;
                _playerAnim.SetBool("IsMoving", false);
            }

            switch(_currentPlayerType)
            {
                case PlayerType.type.CLIMB:
                    if (Input.GetKey(KeyCode.Space)) ApplyForce();
                    break;
                case PlayerType.type.BALLOON:
                    if (Input.GetKey(KeyCode.Space)) ApplyForce();
                    break;
                case PlayerType.type.HOTAIR:
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) ApplyForce();
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) ApplyForce();
                    break;
                case PlayerType.type.PLANE:
                    if (Input.GetKey(KeyCode.Space)) ApplyForce();
                    break;
                case PlayerType.type.JETPACK:
                    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) ApplyForce();
                    break;
                case PlayerType.type.ROCKET:
                    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) ApplyForce();
                    break;
                default:
                    break;
            }

        }

        
    }

    public void ChangeState(PlayerState state)
    {
        _currentPlayerState = state;
    }



    public GameObject CreatePlayer(Transform spawnPoint, PlayerType.type type, GameObject _playerPrefab)
    {
        //if(type == PlayerType.type.BALLOON) _currentPlayer = Instantiate(_balloonPrefab, spawnPoint);
        _currentPlayer = Instantiate(_playerPrefab, spawnPoint);
        _playerMotor = _currentPlayer.GetComponent<PlayerMotor2>();
        _playerHealth = _currentPlayer.GetComponent<PlayerHealth>();
        _playerAnim = _playerMotor.GetPlayerAnimator();

        Debug.Log(_playerAnim);

        UpdateType(type);

        return _currentPlayer;
    }

    public void UpdateType(PlayerType.type type)
    {
        _currentPlayerType = type;
        Debug.Log(type);

        switch (type)
        {
            case PlayerType.type.CLIMB:

                break;

            case PlayerType.type.BALLOON:
                param = new PhysicsParameters();
                param.drag = balloonDrag;
                param.lift = balloonLift;
                param.mass = mass;
                param.horizontalVel = 2f;
                param.verticalForce = 2f;
                param.damp = balloonDamp;
                param.radius = 1;
                param.gravity = gravity;
                break;

            case PlayerType.type.HOTAIR:
                param = new PhysicsParameters();
                break;
            case PlayerType.type.PLANE:
                break;
            case PlayerType.type.JETPACK:
                param = new PhysicsParameters();
                break;
            case PlayerType.type.ROCKET:
                param = new PhysicsParameters();
                break;
            default:
                Debug.LogError("Incorrect type " + type);
                break;
        }

        _playerMotor.param = param;
        _currentPlayerState = PlayerState.CONTROLLABLE;
    }

    

    private void ApplyForce()
    {
        _playerMotor.applyVertical = 1;
        //_playerHealth.ChangeHealth(-1);
        return;
    }


}
