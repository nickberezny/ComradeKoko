using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rockslide : MonoBehaviour
{
    [SerializeField] GameObject _rockPrefab;
    [SerializeField] float[] _gapOrder;
    [SerializeField] float _speed;

    private List<GameObject> limits = new List<GameObject>();
    private List<GameObject> rocks = new List<GameObject>();

    private float dt = 0;
    private GameObject _lastCreatedBox;
    private int gapIndex = 0;

    void Awake()
    {

        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != this.gameObject)
            {
                limits.Add(t.gameObject);
            }
        }

        limits.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
        Debug.Log(limits[0].transform.position.y + "," + limits[1].transform.position.y);

    }

    private void Start()
    {
        AddRock();

    }

    private void FixedUpdate()
    {
        //move everything up
        //if above max point, destroy
        //if above gap, spawn 

        dt = Time.deltaTime;
        if (_lastCreatedBox)
        {
            foreach (GameObject rock in rocks.ToArray())
            {
                rock.transform.position += new Vector3(0, -_speed * dt, 0);
                if (rock.transform.position.y < limits[0].transform.position.y)
                {
                    rocks.Remove(rock);
                    Destroy(rock);

                }
            }

            if (_lastCreatedBox.transform.position.y + _gapOrder[gapIndex] < limits[1].transform.position.y)
            {
                AddRock();

            }
        }


    }

    private void AddRock()
    {
        _lastCreatedBox = Instantiate(_rockPrefab, limits[1].transform);
        rocks.Add(_lastCreatedBox);
        NextIndex();
    }

    private void NextIndex()
    {
        gapIndex++;
        if (gapIndex > _gapOrder.Length - 1) gapIndex = 0;
    }
}
