using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip intro;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource.PlayOneShot(intro);
        audioSource.PlayScheduled(AudioSettings.dspTime + intro.length);
    }
}
