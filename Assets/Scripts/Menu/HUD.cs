using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] Text _altimeter;

    private float _altimeterScale = 4;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        ChangeHealthBar(1);
    }

    public void UpdateAltimeter(float val)
    {
        _altimeter.text = (val/_altimeterScale).ToString("F1") + "m";
    }

    public void ChangeHealthBar(float val)
    {
        if (val > 1 || val < 0) return;

       _slider.value = val;
    }
}
