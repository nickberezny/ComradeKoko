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

    public bool _storyMode = false;

    private int _currentLevelNum = 0;
    private bool _paused = false;
    

    GameState _currentGameState = GameState.PREGAME;

    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private Canvas HUD;
    [SerializeField] private UIManager _uiManager;

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
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && PlayerManager.Instance._currentPlayerState == PlayerManager.PlayerState.CONTROLLABLE)
        {
           
            if (_paused)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }

            _paused = !_paused;
            _uiManager.togglePause();
        }
    }

    void OnLoadLevelComplete(AsyncOperation ao)
    {
       
        StartCoroutine(DelayedTransition(2));
        Time.timeScale = 0;
        _paused = true;
        AsyncUnloadLevel(_sceneToUnload);
        


    }

    void OnUnloadOperationComplete (AsyncOperation ao)
    {
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Spawn").transform;

        PlayerType.type type = FindObjectOfType<LevelManager>()._playerType;

        if (spawnPoint)
        {
            _player = _playerManager.CreatePlayer(spawnPoint.parent, type);
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

    public void LoadNextLevel()
    {
         AsyncLoadLevel(_currentLevelNum+1);
        _currentLevelNum += 1;
    }

    public void LoadLevel(int levelNum = -1, string levelName = "")
    {
        if (!string.IsNullOrEmpty(levelName)) AsyncLoadLevel(-1,levelName);
        else if(levelNum != -1) AsyncLoadLevel(levelNum, "");   
    }
    
    private void AsyncLoadLevel(int levelNum = -1, string levelName = "")
    {
        HUD.enabled = false;
        _uiManager.SetTransitionScreen(true);

        PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.UNACTIVE);
        AsyncOperation ao;
        if (!string.IsNullOrEmpty(levelName)) ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        else if (levelNum != -1)
        {
            ao = SceneManager.LoadSceneAsync(levelNum, LoadSceneMode.Additive);
            levelName = SceneManager.GetSceneByBuildIndex(levelNum).name;
        }
        else
        {
            Debug.Log("[GameManager] Failed to load level - incorrect number or name " + levelNum + ", " + levelName);
            return;
        }

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
        ao.completed += OnUnloadOperationComplete;
    }

    IEnumerator DelayedTransition(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        _uiManager.SetTransitionScreen(false);
        Time.timeScale = 1;
        _paused = false;
        HUD.enabled = true;
    }    

}
