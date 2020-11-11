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

    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _balloonPrefab;

    [SerializeField] private float birdLift = 1;
    [SerializeField] private float birdDrag = 2;
    [SerializeField] private float balloonLift = 15;
    [SerializeField] private float balloonDrag = 1;
    [SerializeField] private float gravity = -3;
    [SerializeField] private float planeDrag = 0f;
    [SerializeField] private HUD _hud;

    private GameObject _currentPlayer;
    private PlayerMotor _playerMotor;
    private PlayerHealth _playerHealth;
    private PhysicsParameters param;

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

            if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                _playerMotor.horizontal = 1;
                //move right
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                _playerMotor.horizontal = -1;
                //move left
            }
            else
            {
                _playerMotor.horizontal = 0;
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



    public GameObject CreatePlayer(Transform spawnPoint, PlayerType.type type)
    {
        if(type == PlayerType.type.BALLOON) _currentPlayer = Instantiate(_balloonPrefab, spawnPoint);
        else _currentPlayer = Instantiate(_playerPrefab, spawnPoint);
        _playerMotor = _currentPlayer.GetComponent<PlayerMotor>();
        _playerHealth = _currentPlayer.GetComponent<PlayerHealth>();
        
        Debug.Log(_currentPlayer.gameObject);

        UpdateType(type);

        return _currentPlayer;
    }

    public void UpdateType(PlayerType.type type)
    {
        _currentPlayerType = type;
        Debug.Log(type);

        /*
        if(type == PlayerType.GROUNDED)
        {
            _playerMotor.UpdateGrounded(true);
        }
        else
        {
            _playerMotor.UpdateGrounded(false);
        }
        */

        switch (type)
        {
            case PlayerType.type.CLIMB:
                param = new PhysicsParameters();
                param.drag = birdDrag;
                param.lift = birdLift;
                param.mass = 1;
                param.horizontal = 3;
                param.vertical = -2.5f;
                Debug.Log("CLIMB!");
                break;

            case PlayerType.type.BALLOON:
                param = new PhysicsParameters();
                param.balloon = true;
                param.drag = balloonDrag;
                param.lift = balloonLift;
                param.mass = 1;
                param.horizontal = 5;
                param.vertical = -8;
                param.ballon_mass = 0.02f;
                param.balloon_damp = 1f;
                param.radius = 3;
                break;

            case PlayerType.type.HOTAIR:
                param = new PhysicsParameters();
                break;
            case PlayerType.type.PLANE:
                param.lift = gravity;
                param.drag = planeDrag;
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
        _playerMotor.vertical = 1;
        //_playerHealth.ChangeHealth(-1);
        return;
    }


}
