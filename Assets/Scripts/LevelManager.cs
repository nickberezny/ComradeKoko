using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerType.type _playerType;

    [SerializeField] AudioClip _levelAudio;

    private void Start()
    {

        if(_levelAudio)
        {
            AudioManager.Instance.PlayAudio(_levelAudio);
        }
    }

}
