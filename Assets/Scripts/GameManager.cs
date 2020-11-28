using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;


public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED
    }


    public bool _storyMode = false;
    private bool _videoIntro = true;
    private int _currentLevelNum = 1;
    private bool _paused = false;
    public float dt { private set; get; }

    GameState _currentGameState = GameState.PREGAME;

    [SerializeField] private CameraManager _cameraManager;
    [SerializeField] private PlayerManager _playerManager;
    [SerializeField] private Canvas _hudCanvas;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] Transform _countdownProcessing;
    [SerializeField] VideoPlayer _videoPlayer;

    private HUD _hud;
    private GameObject _player;
    private string _currentLevelName;
    private string _sceneToUnload;

    private int _transitionTime = 4;

    private float[] altitudeOffsets = { 0, 4000, 380000000 };

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    private void Start()
    {
        _hud = _hudCanvas.GetComponent<HUD>();
        dt = 0;
        _currentLevelName = "MainMenu";
        DontDestroyOnLoad(gameObject);

        _videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, "testvid.mp4");
        Debug.Log(_videoPlayer.url);
        _videoPlayer.Play();
        _uiManager.setPause(false);
    }
    void OnInitialLoad(AsyncOperation ao)
    {
        _uiManager.setMain(true);
        SceneManager.UnloadSceneAsync(0);
    }

    private void Update()
    {
        if(_videoIntro)
        {
            if(!_videoPlayer.isPlaying)
            {
                _videoIntro = false;
                
                AsyncOperation ao = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
                ao.completed += OnInitialLoad;
            }
        }

        if(PlayerManager.Instance._currentPlayerState == PlayerManager.PlayerState.CONTROLLABLE)
        {
            dt += Time.deltaTime;
            _hud.UpdateTimer(dt);
        }

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
        
        _countdownProcessing.gameObject.SetActive(true);
        StartCoroutine(DelayedTransition(_transitionTime));
        Time.timeScale = 0;
        _paused = true;
        Leaderboard.Instance.setLeaderboardCanvas(false);
         AsyncUnloadLevel(_sceneToUnload);
        dt = 0;
    }


    void OnUnloadOperationComplete (AsyncOperation ao)
    {
        Transform spawnPoint = GameObject.FindGameObjectWithTag("Spawn").transform;

        LevelManager lm = FindObjectOfType<LevelManager>();

        PlayerType.type type = lm._playerType;
        GameObject playerPrefab = lm._playerPrefab;

        if (spawnPoint)
        {
            _player = _playerManager.CreatePlayer(spawnPoint.parent, type, playerPrefab);
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

    public void SetPlayer(GameObject _playerPrefab)
    {
        
    }

    public void LoadNextLevel()
    {
        _currentLevelNum += 1;
        AsyncLoadLevel(_currentLevelNum);
        CameraManager.Instance.SetFollow(true);
        Leaderboard.Instance.setLeaderboardCanvas(false);
    }

    public void ClickDebug()
    {
        Debug.Log("Main menu button clicked");
    }

    public void LoadMainMenu()
    {
        Debug.Log("Loading main menu...");
        PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.UNACTIVE);
        _uiManager.setMain(true);
        _uiManager.setPause(false);
        Time.timeScale = 1;
        //_paused = true;
        _hudCanvas.enabled = false;

        AsyncOperation ao = SceneManager.LoadSceneAsync(1);
        ao.completed += OnMainMenuLoad;
        _currentLevelNum = 1;
        _currentLevelName = "MainMenu";
    }

    void OnMainMenuLoad(AsyncOperation ao)
    {
        SceneManager.UnloadSceneAsync(_currentLevelNum);
        _currentLevelNum = 1;
    }

    public void LoadLevel(int levelNum = -1, string levelName = "")
    {
        if (!string.IsNullOrEmpty(levelName)) AsyncLoadLevel(-1, levelName);
        else if (levelNum != -1)
        {
            _currentLevelNum = levelNum;
            AsyncLoadLevel(levelNum, "");
            
        }
    }
    
    private void AsyncLoadLevel(int levelNum = -1, string levelName = "")
    {
        _hudCanvas.enabled = false;
        _hud.SetAltimeterOffset(altitudeOffsets[levelNum-2]);
        TransitionScreen.Instance.SetTransitionScreen(_currentLevelNum-1, _transitionTime);
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
        //countdown
        _uiManager._countdownTask = _uiManager.CountDownRoutine();
        StartCoroutine(_uiManager._countdownTask);
        yield return new WaitForEndOfFrame();

        while (_uiManager._countdownTask != null)
        {
            yield return new WaitForEndOfFrame();
        }

        Time.timeScale = 1;
        _paused = false;
        _hudCanvas.enabled = true;
        _countdownProcessing.gameObject.SetActive(false);
    }    

    public void SkipVideo()
    {
        _videoPlayer.Stop();
    }

}
