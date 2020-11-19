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

    public void togglePause()
    {
        _pauseMenu.enabled = !_pauseMenu.enabled;
    }

    public void SetTransitionScreen(bool set)
    {
        _transitionScreen.enabled = set;
    }
}

