using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField]
    AudioClip beepKey;

    [SerializeField]
    AudioClip endCall;
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
}
