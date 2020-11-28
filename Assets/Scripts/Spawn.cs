using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.TryGetComponent<PlayerMotor2>(out PlayerMotor2 motor);
            motor.SetNewSpawn(this.gameObject); 
        }

    }

}
