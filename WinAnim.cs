using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinAnim : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Appear()
    {
        anim.Play("Appearance", 0, 0);
    }

    void Rotate()
    {
        anim.Play("Rotation", 0, 0);

    }
}
