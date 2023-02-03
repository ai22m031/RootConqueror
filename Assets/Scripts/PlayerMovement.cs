using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float horizontal;
    float vertical;
    
    Vector3 mousePosition;
    private GameObject player;

    public float runSpeed = 20.0f;

    public void Move(){
        // Get the input from the player
        // Gives a value between -1 and 1
        player = GameManager.instance.lm._player;
        
        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down
        
        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Move the player
        if (horizontal != 0 || vertical != 0){
            player.transform.position += new Vector3(horizontal, vertical, 0) * runSpeed * Time.deltaTime;
            mousePosition = player.transform.position;
        }
        else{
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(mousePosition.x, mousePosition.y, 0), runSpeed * Time.deltaTime);
        }
    }
}
