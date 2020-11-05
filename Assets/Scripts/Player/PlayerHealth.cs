using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float _bounciness = 30;
    [SerializeField] int _maxHealth;
    
    public int _health {  get; private set; }
    private const int fps = 50; //all health in terms of seconds, assuming fixedupdate @ 50Hz
    private PlayerMotor _playerMotor;

    private void Awake()
    {
        _health = _maxHealth*fps;
        _playerMotor = GetComponent<PlayerMotor>();
    }
    public void changeHealth(int change)
    {
        _health = Mathf.Clamp(_health + change*fps, 0, _maxHealth*fps);

        if(_health == 0)
        {
            //die
        }
    }
    
    public void HitByEnemy(Collision2D col, int healthDecrement)
    {
        //decrease health
        //enter brief immune state
        //bounce // fall ... ?
        
        PlayerManager.Instance.ChangeState(PlayerManager.PlayerState.HIT);

        Vector2 dir = col.contacts[0].normal;
        _playerMotor.HitPlayer(0, _bounciness);

        changeHealth(healthDecrement);
        
        Debug.Log("Hit by enemy");
    }
}
