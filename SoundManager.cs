using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] blockSoundBase;
    public AudioClip winSound;
    public AudioClip mixSound;

    public void DestroySound()
    {
        int rand = Random.Range(0, blockSoundBase.Length);
        source.clip = blockSoundBase[rand];
        source.time = 0;
        source.Play();
    }

    public void WinSound()
    {
        source.clip = winSound;
        source.time = 0;
        source.Play();
    }

    public void MixSound()
    {
        source.clip = mixSound;
        source.time = 0;
        source.Play();

    }
}
