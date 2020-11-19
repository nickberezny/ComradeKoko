using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    [SerializeField] float _bounciness = 200;
    [SerializeField] AudioClip _clip;

    private Collider2D _collider;
    

    protected virtual void Awake()
    {
        Debug.Log("enemy awake");
        _collider = GetComponent<Collider2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            AudioManager.Instance.PlaySFX(_clip);
        }
    }


}
