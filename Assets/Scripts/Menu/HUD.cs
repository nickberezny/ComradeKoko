using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] Text _altimeter;
    [SerializeField] Text _timer;

    private float _time;
    private float _altimeterScale = 4;
    private float _altimeterOffset = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ChangeHealthBar(1);
    }

    public void SetAltimeterOffset(float offset)
    {
        _altimeterOffset = offset;
    }

    public void UpdateTimer(float t)
    {
        int sec = (int)t % 60;
        int min = (int)t / 60;

        _timer.text = min.ToString("F0") + "m " + sec.ToString("D2") + "s";
    }

    public void UpdateAltimeter(float val)
    {
     
        _altimeter.text = ((double)_altimeterOffset + (double)(val / _altimeterScale)).ToString("F1") + "m";
    }

    public void ChangeHealthBar(float val)
    {
        if (val > 1 || val < 0) return;

       _slider.value = val;
    }
}
