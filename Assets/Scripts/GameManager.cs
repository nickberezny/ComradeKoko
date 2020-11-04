using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }

    GameState _currentGameState = GameState.PREGAME;

    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private PlayerManager _playerManager;

    private GameObject _player;
    private string _currentLevelName;
    private string _sceneToUnload;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    private void Start()
    {
        _currentLevelName = SceneManager.GetActiveScene().name;
        DontDestroyOnLoad(gameObject);
        //_cameraManager.setPlayer(_player);
    }

    void OnLoadLevelComplete(AsyncOperation ao)
    {
        AsyncUnloadLevel(_sceneToUnload);
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Spawn").transform;
        if(spawnPoint)
        {
            _player = _playerManager.CreatePlayer(spawnPoint);
            spawnPoint.position = spawnPoint.position + new Vector3(0, 0, -10);
            _cameraManager.CreateCamera(spawnPoint);
            _cameraManager.setPlayer(_player);
        }
        else
        {
            Debug.LogError("[GameManager] Cannot find spawn point");
            return;
        }


    }

    public void LoadLevel(string levelName)
    {
        AsyncLoadLevel(levelName);
        
        
    }

    private void AsyncLoadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to load level " + levelName);
            return;
        }

        ao.completed += OnLoadLevelComplete;
        //_loadOperations.Add(ao);
        _sceneToUnload = _currentLevelName;
        _currentLevelName = levelName;
        

    }
    private void AsyncUnloadLevel(string levelName)
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[GameManager] Unable to unload level " + levelName);
            return;
        }
        //ao.completed += OnUnloadOperationComplete;
    }



}
