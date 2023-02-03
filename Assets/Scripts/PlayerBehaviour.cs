using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : AlliedObjectBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.health = 5;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) {
            Time.timeScale = 0f;
        }
    }
}
