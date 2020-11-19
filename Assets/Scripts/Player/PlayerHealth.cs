using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float _bounciness = 30;
    [SerializeField] int _maxHealth = 1000;
    
    
    public int _health {  get; private set; }
    private const int fps = 50; //all health in terms of seconds, assuming fixedupdate @ 50Hz
    private PlayerMotor2 _playerMotor;
    private HUD _hud;

    

    private void Awake()
    {
        _health = _maxHealth*fps;
        _playerMotor = GetComponent<PlayerMotor2>();
        _hud = FindObjectOfType<HUD>();
    }
    public void changeHealth(int change)
    {
        _health = Mathf.Clamp(_health + (change * fps), 0, _maxHealth*fps);
        _hud.ChangeHealthBar((float)_health / (float)(_maxHealth*fps));

        if(_health == 0)
        {
            Death();
        }

    }
    
    public void SetHUD(HUD hud)
    {
        _hud = hud;
    }

    public void HitByEnemy(Collision2D col)
    {
        Death();
        Debug.Log("Hit by enemy");
    }

    private void Death()
    {
        _playerMotor.Death();
    }
}
