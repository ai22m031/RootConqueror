using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public bool isCollected = false;
    
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        
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
