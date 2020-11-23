using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] AudioSource _musicSource;
    [SerializeField]  AudioSource _sfxSource;

    private AudioClip[] _clips;
    private bool[] _loops;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    } 

    public void PlayAudio(AudioClip[] clip, bool[] loop)
    {
        _clips = clip;
        _loops = loop;
        StartCoroutine(ManageMusic());
    }

    IEnumerator ManageMusic()
    {
        for(int i = 0; i < _clips.Length; i++)
        {
            PlayClip(_clips[i], _loops[i]);
            yield return new WaitForSecondsRealtime(5);
            while (_musicSource.isPlaying)
            {
                yield return new WaitForSecondsRealtime(5);
            }
        }
    }

    private void PlayClip(AudioClip clip, bool loop)
    {
        _musicSource.clip = clip;
        _musicSource.loop = loop;
        _musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        _sfxSource.clip = clip;
        _sfxSource.Play();
    }


}
