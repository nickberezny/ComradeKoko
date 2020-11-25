using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocks : MonoBehaviour
{
    [SerializeField] float speed;

    private float dt = 0;
    private int direction;

    private void Awake()
    {
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        direction = 2 * Random.Range(0, 2) - 1;

        Debug.Log("Rock: " + direction);
    }

    private void FixedUpdate()
    {
        dt = Time.deltaTime;
        transform.eulerAngles += new Vector3(0, 0, (float)direction*speed * dt);
    }
}
