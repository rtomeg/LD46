using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    AudioClip beepKey;
    [SerializeField]
    AudioClip endCall;
    [SerializeField]
    AudioClip click;
    [SerializeField]
    AudioSource sfx;

    [SerializeField]
    List<AudioClip> musicList;

    [SerializeField]
    AudioSource music;

    private bool activeMusic = true;

    public static AudioController instance;

    #region SINGLETON PATTERN 
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
    #endregion

    void Start()
    {
        StartMusic();
    }

    public void MusicIconClick()
    {
        if (activeMusic)
        {
            StopAllCoroutines();
            music.Stop();
            activeMusic = false;
        }
        else
        {
            StartMusic();
            activeMusic = true;
        }
    }
    void StartMusic()
    {
        StartCoroutine(MusicCorroutine());
    }

    IEnumerator MusicCorroutine()
    {
        music.clip = musicList[Random.Range(0, musicList.Count)];
        music.Play();
        yield return new WaitForSeconds(30);
        MusicCorroutine();
    }

    public void PlayBeepKey()
    {
        sfx.PlayOneShot(beepKey, 0.2f);
    }
    public void PlayEndCallSound()
    {
        sfx.PlayOneShot(endCall, 0.2f);
    }

    public void PlayClickSound()
    {
        sfx.PlayOneShot(click, 0.2f);
    }
}
