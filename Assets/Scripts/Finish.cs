using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] int level;

    private void Start()
    {
        Leaderboard.Instance.GetLeaderboard(level);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Finish Collision w/ " + collision.gameObject.name);
        if (collision.gameObject.tag == "Player")
        {
            CameraManager.Instance.SetFollow(false);
            PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.UNACTIVE);
            

            StartCoroutine(WaitToLoad(2f));

            
        }
    }

    IEnumerator WaitToLoad(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Leaderboard.Instance.SendLeaderboard(level, (int)GameManager.Instance.dt);
        

        //GameManager.Instance.LoadNextLevel();
        //CameraManager.Instance.SetFollow(true);
    }
}
