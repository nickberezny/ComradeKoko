using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : Enemy
{
    [SerializeField] float speed;

    private List<float> limits = new List<float>();
    private float direction = 1;
    protected override void Awake()
    {

        base.Awake();
        foreach (Transform t in GetComponentsInChildren<Transform>())
        {
            if (t.gameObject != this.gameObject && t.gameObject.tag == "EndPoint")
            {
                limits.Add(t.position.y);
            }
        }

        limits.Sort();

        Debug.Log(limits[0] + "," + limits[1]);

    }

    private void FixedUpdate()
    {
        transform.position = transform.position + new Vector3(0, direction * speed,  0);

        if (direction == 1 && transform.position.y > limits[1])
        {
            direction = -1;
        }
        else if (direction == -1 && transform.position.y < limits[0])
        {
            direction = 1;
        }

    }
}
