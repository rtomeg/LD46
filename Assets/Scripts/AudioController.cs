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
    AudioSource sfx;
    void Awake(){
        sfx = GetComponent<AudioSource>();
    }
    public void PlayBeepKey(){
        sfx.PlayOneShot(beepKey, 0.2f);
    }
    public void PlayEndCallSound(){
        sfx.PlayOneShot(endCall, 0.2f);
    }

    public void PlayClickSound(){
        sfx.PlayOneShot(click, 0.2f);
    }
}
