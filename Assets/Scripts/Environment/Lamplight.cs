using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class Lamplight : MonoBehaviour
{
    private Light2D _light;
    private float dt = 0;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
    }

    private void Start()
    {
        StartCoroutine(Flicker(0.1f)) ;
    }


    IEnumerator Flicker(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        _light.intensity = Random.Range(0.8f, 1f);
        StartCoroutine(Flicker(Random.Range(0.01f, 0.05f))); ;
        
    }

}
