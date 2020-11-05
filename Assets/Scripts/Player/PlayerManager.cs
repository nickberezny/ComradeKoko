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

    public enum PlayerType
    {
        NULL,
        GROUNDED,
        CLIMB,
        BALLOON,
        HOTAIR,
        PLANE,
        JETPACK,
        ROCKET
    }

    PlayerState _currentPlayerState = PlayerState.UNACTIVE;
    PlayerType _currentPlayerType = PlayerType.CLIMB;

    

    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _balloonPrefab;

    [SerializeField] private float birdLift = 1;
    [SerializeField] private float birdDrag = 2;
    [SerializeField] private float balloonLift = 15;
    [SerializeField] private float balloonDrag = 1;
    [SerializeField] private float gravity = -3;
    [SerializeField] private float planeDrag = 0f;

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
                case PlayerType.CLIMB:
                    if (Input.GetKey(KeyCode.Space)) ApplyForce();
                    break;
                case PlayerType.BALLOON:
                    if (Input.GetKey(KeyCode.Space)) ApplyForce();
                    break;
                case PlayerType.HOTAIR:
                    if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) ApplyForce();
                    if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) ApplyForce();
                    break;
                case PlayerType.PLANE:
                    if (Input.GetKey(KeyCode.Space)) ApplyForce();
                    break;
                case PlayerType.JETPACK:
                    if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) ApplyForce();
                    break;
                case PlayerType.ROCKET:
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

    public GameObject CreatePlayer(Transform spawnPoint)
    {
        if(_currentPlayerType == PlayerType.BALLOON) _currentPlayer = Instantiate(_balloonPrefab, spawnPoint);
        else _currentPlayer = Instantiate(_playerPrefab, spawnPoint);
        _playerMotor = _currentPlayer.GetComponent<PlayerMotor>();
        _playerHealth = _currentPlayer.GetComponent<PlayerHealth>();

        _currentPlayerState = PlayerState.CONTROLLABLE;
        UpdateType(PlayerType.CLIMB);

        return _currentPlayer;
    }

    void UpdateType(PlayerType type)
    {
        _currentPlayerType = type;

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
            case PlayerType.CLIMB:
                param = new PhysicsParameters();
                param.drag = birdDrag;
                param.lift = birdLift;
                param.mass = 1;
                param.horizontal = 3;
                param.vertical = -2.5f;
                break;

            case PlayerType.BALLOON:
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

            case PlayerType.HOTAIR:

                break;
            case PlayerType.PLANE:
                param.lift = gravity;
                param.drag = planeDrag;
                break;
            case PlayerType.JETPACK:

                break;
            case PlayerType.ROCKET:

                break;
            default:
                break;
        }

        _playerMotor.param = param;
    }

    

    private void ApplyForce()
    {
        _playerMotor.vertical = 1;
        //_playerHealth.ChangeHealth(-1);
        return;
    }


}
