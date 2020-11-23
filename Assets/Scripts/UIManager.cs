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
    

    public IEnumerator _countdownTask;

    private int _levelNum = 0;


    public void RunGame(string mode)
    {
        if(mode == "Story")
        {
            GameManager.Instance._storyMode = true;
            GameManager.Instance.LoadNextLevel();
        }
        else if(mode == "Level")
        {

            GameManager.Instance.LoadLevel(_levelDropDown.value + 1);
            
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
    }


    public void togglePause()
    {
        _pauseMenu.enabled = !_pauseMenu.enabled;
    }

    public void SetTransitionScreen(bool set)
    {
        
        _transitionScreen.enabled = set;
    }
}

