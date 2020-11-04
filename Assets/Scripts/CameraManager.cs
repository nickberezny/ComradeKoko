using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private Camera _cameraPrefab;

    [SerializeField] private float upperBound = 10;
    [SerializeField] private float lowerBound = -10;

    private Camera _camera;
    private GameObject _player;

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
       if(_player)
        {
            float dy = _camera.transform.position.y - _player.transform.position.y;
            if (dy > upperBound )
            {
                _camera.transform.position = new Vector3(_camera.transform.position.x, _player.transform.position.y + upperBound , _camera.transform.position.z);
            }
            else if(dy < lowerBound)
            {
                _camera.transform.position = new Vector3(_camera.transform.position.x, _player.transform.position.y + lowerBound, _camera.transform.position.z);
            }
            else
            {
                _camera.transform.position = new Vector3(_camera.transform.position.x, _player.transform.position.y  - dy * Mathf.Abs(dy)/upperBound, _camera.transform.position.z);
            }
        }
    }

    public void CreateCamera(Transform spawnPoint)
    {
        _camera = Instantiate(_cameraPrefab, spawnPoint);
    }

    public void setPlayer(GameObject player)
    {
        _player = player;
    }
}
