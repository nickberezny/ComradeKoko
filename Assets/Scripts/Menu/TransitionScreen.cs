using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionScreen : Singleton<TransitionScreen>
{

    [SerializeField] Sprite[] _images;
    [SerializeField] Image[] _title;

    [SerializeField] string[] _titleStrings;

    private Image _transitionImage;
    private Text[] _titleText = new Text[2];

    private void Start()
    {
        _transitionImage = GetComponent<Image>();
        _titleText[0] = _title[0].GetComponentInChildren<Text>();
        _titleText[1] = _title[1].GetComponentInChildren<Text>();
    }
    
    public void SetTransitionScreen(int index, int time = 0)
    {
        foreach(Image title in _title)
        {
            title.gameObject.SetActive(false);
        }
        //Debug.Log("Transition" + index);
        _titleText[1].text = _titleStrings[index-1];
        _titleText[0].text = "LEVEL " + (index).ToString();
        _transitionImage.color = Color.black;
        _transitionImage.sprite = _images[index-1];
        StartCoroutine(FadeIn((float)time/3f));
    }

    IEnumerator FadeIn(float time)
    {
        float dt = 0;

        while(dt < time)
        {
            //Debug.Log("Fade:" + dt);
            _transitionImage.color = Color.Lerp(Color.black, Color.white, dt / (float)time);
            dt += Time.unscaledDeltaTime;
            yield return new WaitForEndOfFrame();
        }

        foreach (Image title in _title)
        {
            title.gameObject.SetActive(true);
        }

    }
}
