using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isCollected = false;
    public int value;
    
    void Awake()
    {
        spriteRenderer.color = Color.blue;
    }

    public void collect() {
        spriteRenderer.color = Color.white;
        isCollected = true;
    }

    public void uncollect() {
        spriteRenderer.color = Color.blue;
        isCollected = false;
    }
}
