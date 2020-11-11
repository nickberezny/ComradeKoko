using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollowSmooth : Enemy
{
    //ideally, have an even number of nodes
    //otherwise path will change slightly every other pass

    [SerializeField] float speed;
    [SerializeField] GameObject _enemyPrefab;

    private List<Vector3> nodes = new List<Vector3>();
    private float dt = 0;
    private GameObject _enemy;
    private int nodeIndex = 0;
    private int nodeMidIndex = 0;
    private int nextNodeIndex = 0;
    private Vector3 dir;
    private Vector3 dir1;
    private Vector3 dir2;
    private float distance;
    private float intersectionMargin = 0.3f;

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

        if (Vector3.Distance(_enemy.transform.position, nodes[nextNodeIndex]) < intersectionMargin)
        {

            newNode();
        }
        else
        {
            //Debug.Log(Vector3.Distance(_enemy.transform.position, nodes[nextNodeIndex]));
            float currentDist = Vector3.Distance(_enemy.transform.position, nodes[nextNodeIndex]);
            dir2 = Vector3.Normalize(nodes[nextNodeIndex] - _enemy.transform.position);
            dir = Vector3.Lerp(dir1, dir2, 1.3f - ( currentDist / distance));
            _enemy.transform.position += dir * speed * dt;

        }

    }

    private void newNode()
    {
        nodeIndex = nextNodeIndex;
        if (nodeIndex + 1 > nodes.Count - 1)
        {
            nodeMidIndex = 0;
            nextNodeIndex = 1;
        }
        else if (nodeIndex + 2 > nodes.Count - 1)
        {
            nodeMidIndex = nodeIndex + 1;
            nextNodeIndex = 0;
        }
        else
        {
            nodeMidIndex = nodeIndex + 1;
            nextNodeIndex = nodeIndex + 2;
        }


        dir1 = Vector3.Normalize(nodes[nodeMidIndex] - nodes[nodeIndex]);
        dir2 = Vector3.Normalize(nodes[nextNodeIndex] - nodes[nodeMidIndex]);
        distance = Vector3.Distance(nodes[nodeIndex], nodes[nextNodeIndex]);
    }
}
