using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] Dropdown _levelDropDown;
    [SerializeField] Canvas _pauseMenu;
    [SerializeField] Canvas _transitionScreen;
    [SerializeField] Canvas _countdownScreen;
    [SerializeField] Canvas _mainMenu;
    [SerializeField] Camera _mainMenuCamera;
    [SerializeField] Canvas _controlsCanvas;

    public IEnumerator _countdownTask;

    private int _levelNum = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void RunGame(string mode)
    {
        setMain(false);

        if(mode == "Story")
        {
            GameManager.Instance._storyMode = true;
            GameManager.Instance.LoadNextLevel();
        }
        else if(mode == "Level")
        {

            GameManager.Instance.LoadLevel(_levelDropDown.value + 2);
            
        }


      
    }


    public IEnumerator CountDownRoutine()
    {
        _countdownScreen.enabled = true;
        
        Text text = _countdownScreen.GetComponentInChildren<Text>();
        int count = 5;

        while (count > 0)
        {
            text.text = count.ToString();
            count--;
            yield return new WaitForSecondsRealtime(1f);
        }
        _countdownScreen.enabled = false;
        
        _countdownTask = null;
        PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.CONTROLLABLE);
    }


    public void togglePause()
    {
        //if(_pauseMenu)
        
        _pauseMenu.enabled = !_pauseMenu.enabled;
        _pauseMenu.gameObject.SetActive(_pauseMenu.enabled);
        
        
    }

    public void setPause(bool val)
    {
        _pauseMenu.gameObject.SetActive(val);
        _pauseMenu.enabled = val;
        
    }

    public void SetTransitionScreen(bool set)
    {
        
        _transitionScreen.enabled = set;
    }

    public void setMain(bool val)
    {
        //_mainMenuCamera.gameObject.SetActive(val);
        _mainMenu.gameObject.SetActive(val);
        _mainMenu.enabled = val;
        //_mainMenu.gameObject.SetActive(val);
    }

    public void setControls(bool val)
    {
        _controlsCanvas.enabled = val;
    }
}

