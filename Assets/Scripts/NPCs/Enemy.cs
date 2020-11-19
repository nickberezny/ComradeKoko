using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
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
            _collider.enabled = false;
            StartCoroutine(WaitToReactivateCollider(1));
            collision.gameObject.GetComponent<PlayerHealth>().HitByEnemy(collision);
        }

    }

    IEnumerator WaitToReactivateCollider(float t)
    {
        yield return new WaitForSeconds(t);
        _collider.enabled = true;
    }



}
