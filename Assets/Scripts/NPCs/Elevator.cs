using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] GameObject _boxPrefab;
    [SerializeField] float[] _gapOrder;
    [SerializeField] float _speed;

    private List<GameObject> limits = new List<GameObject>();
    private List<GameObject> boxes = new List<GameObject>();

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

        if(_speed>0)
        {
            limits.Sort((a, b) => a.transform.position.y.CompareTo(b.transform.position.y));
        }
        else
        {
            limits.Sort((a, b) => b.transform.position.y.CompareTo(a.transform.position.y));
        }
        

        Debug.Log(limits[0].transform.position.y + "," + limits[1].transform.position.y);

    }

    private void Start()
    {
        AddBox();
        
    }

    private void FixedUpdate()
    {
        //move everything up
        //if above max point, destroy
        //if above gap, spawn 

        dt = Time.deltaTime;
        if(_lastCreatedBox)
        {
            foreach (GameObject box in boxes.ToArray())
            {
                box.transform.position += new Vector3(0, _speed * dt, 0);
                if (_speed*box.transform.position.y > _speed*limits[1].transform.position.y)
                {
                    boxes.Remove(box);
                    Destroy(box);

                }
            }

            if (_speed*_lastCreatedBox.transform.position.y - Mathf.Abs(_speed)*_gapOrder[gapIndex] > _speed*limits[0].transform.position.y)
            {
                AddBox();

            }
        }
        

    }

    private void AddBox()
    {

        _lastCreatedBox = Instantiate(_boxPrefab, limits[0].transform.position + new Vector3(0, 0, 0.05f), Quaternion.identity, limits[0].transform);
        boxes.Add(_lastCreatedBox);
        NextIndex();
    }

    private void NextIndex()
    {
        gapIndex++;
        if (gapIndex > _gapOrder.Length - 1) gapIndex = 0;
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
}
