using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : AlliedObjectBehaviour
{

    //load script into script
    private PlayerAction pa = new PlayerAction();
    private PlayerMovement pm = new PlayerMovement();

    bool isPlanting=false;
    // Start is called before the first frame update
    void Start()
    {
        this.health = 5;
    }

    // Update is called once per frame
    void Update()
    {

        isPlanting =  pa.PlantCheck();
        if (!isPlanting)
        {
            pm.Move();
        }

    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) {
            Time.timeScale = 0f;
        }
    }


}
