using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public PlayerType.type _playerType;
    public GameObject _playerPrefab;

    [SerializeField] AudioClip[] _levelAudio;
    [SerializeField] bool[] _loop;
    [SerializeField] string[] _levelTitles;

    

    private void Start()
    {

     AudioManager.Instance.PlayAudio(_levelAudio, _loop);
     //GameManager.Instance.SetPlayer(_playerPrefab);
        
    }



}
