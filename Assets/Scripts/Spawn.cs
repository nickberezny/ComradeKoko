using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("new Spawn");
        Debug.Log("Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {

            collision.gameObject.GetComponent<PlayerMotor2>().SetNewSpawn(this.gameObject);
            Debug.Log("new Spawn");
        }

    }

}
