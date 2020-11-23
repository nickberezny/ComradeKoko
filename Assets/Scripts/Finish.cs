using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Finish Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            CameraManager.Instance.SetFollow(false);
            PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.UNACTIVE);

            StartCoroutine(WaitToLoad(3f));
        }
    }

    IEnumerator WaitToLoad(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        
        GameManager.Instance.LoadNextLevel();
        CameraManager.Instance.SetFollow(true);
    }
}
