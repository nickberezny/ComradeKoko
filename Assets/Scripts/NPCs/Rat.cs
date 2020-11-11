using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : Enemy
{
    [SerializeField] float speed;
    private List<float> limits = new List<float>();
    private float direction = 1;
    private int turnSpeed = 10;

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

        limits.Sort();

        Debug.Log(limits[0] + "," + limits[1]);

    }

    private void FixedUpdate()
    {
        transform.position = transform.position + new Vector3(direction * speed, 0, 0);

        if (direction == 1 && transform.position.x > limits[1])
        {
            direction = -1;
            StartCoroutine(turnAround());
        }
        else if (direction == -1 && transform.position.x < limits[0])
        {
            direction = 1;
            StartCoroutine(turnAround());
        }

    }

    IEnumerator turnAround()
    {
        int rotation = 0;
        while (rotation < 180)
        {
            transform.Rotate(0f, turnSpeed, 0f, Space.Self);
            rotation += turnSpeed;
            yield return new WaitForSeconds(0.01f);
        }
    }

}
