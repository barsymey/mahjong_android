using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public AudioClip[] musicBase;
    public AudioSource source;
    private int currentClip;
    
    void Start()
    {
        currentClip = 0;
        source.loop = false;
        source.clip = musicBase[currentClip];
        source.Play();
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            currentClip++;
            if (currentClip > musicBase.Length)
            {
                currentClip = 0;
            }
            source.clip = musicBase[currentClip];
            source.time = 0;
            source.Play();
        }
    }
}
