using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Block : MonoBehaviour
{
    public int blockX;
    public int blockY;
    public int blockZ;
    public int type;
    public Sprite[] spriteBase;
    public Sprite[] frameBase;
    public SpriteRenderer heightMask;
    public SpriteRenderer symbol;
    public bool destroyed;
    private void Start()
    {
        SetType();
        destroyed = false;
    }

    private void SetType()
    {
        GetComponent<SpriteRenderer>().sprite = frameBase[Random.Range(0, 3)];
    if (type > 0)
        {
            symbol.sprite = spriteBase[type - 1];
            heightMask.color = new Color(0f,0f,0f,0.45f - blockZ * 0.15f);
        }
    }

    public void Select()
    {
        GetComponent<Animator>().Play("Select");
    }
    
    public void Unselect()
    {
        GetComponent<Animator>().Play("Deselect");
    }

    public void MarkDestroy()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<Animator>().Play("Destroy");
    }
    
    public void Destroy()
    {
        Destroy(gameObject);
    }

    public void Unavailable()
    {
        GetComponent<Animator>().Play("Unavaliable");
    }

    public void Idle()
    {
        GetComponent<Animator>().Play("Idle");
    }
}
