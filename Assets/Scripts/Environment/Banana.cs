using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banana : MonoBehaviour
{

    private int _healthIncrement = 50;

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Banana Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerHealth>().changeHealth(_healthIncrement);
            Destroy(this.gameObject);
        }
        else if(collision.gameObject.tag == "Koko")
        {
            collision.gameObject.GetComponentInParent<PlayerHealth>().changeHealth(_healthIncrement);
            Destroy(this.gameObject);
        }

    }
}
