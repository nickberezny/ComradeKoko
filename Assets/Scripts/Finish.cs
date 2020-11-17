using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {

            GameManager.Instance.LoadNextLevel();
        }
    }
}
