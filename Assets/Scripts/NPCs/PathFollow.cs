using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : Enemy
{

    [SerializeField] float speed;
    [SerializeField] GameObject _enemyPrefab;

    private List<Vector3> nodes = new List<Vector3>();
    private float dt = 0;
    private GameObject _enemy;
    private int nodeIndex = 0;
    private int nextNodeIndex = 0;
    private Vector3 dir;
    private float intersectionMargin = 2;

    protected override void Awake()
    {

        base.Awake();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != this.gameObject)
            {
                nodes.Add(t.position);
            }
        }

        _enemy = Instantiate(_enemyPrefab, nodes[0], Quaternion.identity, transform);
        newNode();

    }
    private void FixedUpdate()
    {
        dt = Time.deltaTime;

        if(Vector3.Distance(_enemy.transform.position, nodes[nextNodeIndex]) < intersectionMargin)
        {
            
            newNode();
        }
        else
        {
            Debug.Log(Vector3.Distance(_enemy.transform.position, nodes[nextNodeIndex]));
            _enemy.transform.position += dir*speed*dt;
        }

    }

    private void newNode()
    {
        nodeIndex = nextNodeIndex;
        if (nodeIndex + 1 > nodes.Count - 1) nextNodeIndex = 0;
        else nextNodeIndex = nodeIndex + 1;

        dir = Vector3.Normalize(nodes[nextNodeIndex] - nodes[nodeIndex]);

    }
}
