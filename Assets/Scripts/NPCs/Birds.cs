using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Birds : Enemy
{

    [SerializeField] float speed; 
    [SerializeField] int numberOfBirds;
    [SerializeField] GameObject _birdPrefab;

    private GameObject[] _birds;
    private List<float> limits = new List<float>();
    private float dt = 0;
    private float distance = 0;
    private int dir = 0;
    private Quaternion rot;

    protected override void Awake()
    {

        base.Awake();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != this.gameObject)
            {
                limits.Add(t.position.x);
            }
        }

        if (speed < 0)
        {
            rot.eulerAngles = _birdPrefab.transform.eulerAngles + new Vector3(0, -180, 0);
        }
        else
        {
            rot.eulerAngles = _birdPrefab.transform.eulerAngles + new Vector3(0, 0, 0);
        }

        distance = Mathf.Abs(limits[0] - transform.position.x);
        dir = (int)((limits[0] - transform.position.x) / distance);
        _birds = new GameObject[numberOfBirds];

        Debug.Log("Dir:" + dir + ", Distance: " + distance);

        _birds[0] = Instantiate(_birdPrefab, transform);

        for (int i = 1; i < numberOfBirds; i++)
        {
            _birds[i] = Instantiate(_birdPrefab, transform.position + new Vector3((float)dir * (float)i * distance / (float)numberOfBirds, 0, 0), rot, transform);
            Debug.Log("New Pos: " + transform.position + new Vector3((float)dir * (float)i * distance / (float)numberOfBirds, 0, 0));
        }

       
        
    }

    private void FixedUpdate()
    {
       
        dt = Time.deltaTime;

        for (int i = 0; i < numberOfBirds; i++)
        {
            _birds[i].transform.position += new Vector3(speed * dt, 0f, 0f);
        }

        if (dir * _birds[numberOfBirds - 1].transform.position.x > dir * limits[0])
        {
            DestroyBird();
        }
        
    }

    private void DestroyBird()
    {
        Destroy(_birds[numberOfBirds - 1]);

        for(int i = numberOfBirds - 1; i >= 1; i--)
        {
            _birds[i] = _birds[i-1];
        }

        CreateBird();
    }

    private void CreateBird()
    {
        _birds[0] = Instantiate(_birdPrefab, transform.position, rot, transform);
    }



}
