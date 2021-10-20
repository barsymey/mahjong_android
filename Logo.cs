using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Logo : MonoBehaviour
{
    float timer;
    private void Start()
    {
        timer = Time.realtimeSinceStartup;
        GetComponent<VideoPlayer>().Play();
    }

    void Update()
    {
        if (Input.touchCount>1)
        {
            Destroy();
        }

        if (Time.realtimeSinceStartup - timer > 5 & !GetComponent<VideoPlayer>().isPlaying)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);

    }
}
