using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    AudioSource _musicSource;
    AudioSource _sfxSource;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        _musicSource = GetComponent<AudioSource>();
        _sfxSource = GetComponentInChildren<AudioSource>();
    }

    public void PlayAudio(AudioClip clip)
    {
        _musicSource.clip = clip;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }


}
