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
    private bool _followPlayer = true;

    

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void LateUpdate()
    {
       if(_player && _followPlayer)
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

            _camera.transform.position = RoundPosition(_camera.transform.position);
        }

       
    }


    private float RoundToNearestPixel(float unityUnits)
    {
        float valueInPixels = Mathf.Round(unityUnits * 100);
        return valueInPixels * (1 / 100f);
    }

    private Vector3 RoundPosition(Vector3 pos)
    {
        pos.x = RoundToNearestPixel(pos.x);
        pos.y = RoundToNearestPixel(pos.y);

        return pos;
    }

    public void CreateCamera(Transform spawnPoint)
    {
        _camera = Instantiate(_cameraPrefab, spawnPoint);
    }

    public void setPlayer(GameObject player)
    {
        _player = player;
    }

    public void SetFollow(bool follow)
    {
        _followPlayer = follow;
    }
}
