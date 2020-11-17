using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    [SerializeField] float _bounciness = 200;

    private Collider2D _collider;
    

    protected virtual void Awake()
    {
        Debug.Log("enemy awake");
        _collider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerMotor2>().HitPlayer(_bounciness*collision.contacts[0].normal.x, _bounciness*collision.contacts[0].normal.y);
        }
    }


}
