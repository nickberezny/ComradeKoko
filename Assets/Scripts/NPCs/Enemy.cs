using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int healthDecrement;

    protected virtual void Awake()
    {
        Debug.Log("enemy awake");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().HitByEnemy(healthDecrement);
            //collision.gameObject.GetComponent<Player>().fallDown();
            //set animation
            //Destroy(this.gameObject);
            //audioSource.Play();
            //StartCoroutine(fallDown());
        }
    }



}
