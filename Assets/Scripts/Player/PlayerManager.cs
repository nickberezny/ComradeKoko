using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public enum PlayerState
    {
        UNACTIVE,
        CONTROLLABLE,
        DEAD,
    }

    public enum PlayerType
    {
        NULL,
        CLIMB,
        BALLOON,
        HOTAIR,
        PLANE,
        JETPACK,
        ROCKET
    }

    PlayerState _currentPlayerState = PlayerState.CONTROLLABLE;
    PlayerType _currentPlayerType = PlayerType.PLANE;

    [SerializeField] PlayerMotor _playerMotor;

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
                    SlowDown(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S));
                    break;
                case PlayerType.BALLOON:
                    SlowDown(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S));
                    break;
                case PlayerType.HOTAIR:
                    SlowDown(Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S));
                    SpeedUp(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
                    break;
                case PlayerType.PLANE:
                    SpeedUp(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
                    break;
                case PlayerType.JETPACK:
                    SpeedUp(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W));
                    break;
                case PlayerType.ROCKET:
                    SpeedUp(Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W));
                    break;
                default:
                    break;
            }

        }


    }

    private void SlowDown(bool keyPress)
    {
        if (keyPress) _playerMotor.force = -8;
        //else _playerMotor.force = 0;
        return;
    }

    private void SpeedUp(bool keyPress)
    {
        if(keyPress) Debug.Log("Speed Up");
        if (keyPress) _playerMotor.force = 200;
        //else _playerMotor.force = 0;
        return;
    }
}
