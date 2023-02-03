using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : AlliedObjectBehaviour{
    private SpriteRenderer sr;
    float horizontal;
    float vertical;
    private float viewDistance = 5f;

    Vector3 mousePosition;
    private GameObject player;

    public float runSpeed = 20.0f;
    //load script into script
    private PlayerAction Player;
    public GameObject EndScreen;

    public enum PlayerState
    {
        Moving,
        Planting
    }
    private PlayerState _state;
    private float plantCooldown = 3f, plantTimeStamp = 0f;
    public int maxHealth;
    // Start is called before the first frame update
    void Start(){
        sr = GetComponentInChildren<SpriteRenderer>();
        this.health = maxHealth;
        Player = GameManager.instance._player.GetComponent<PlayerAction>();
        _state = PlayerState.Moving;
    }

    float deltaTime;
    // Update is called once per frame
    void Update()
    {
        if(Time.time >= plantTimeStamp) {
            if(_state == PlayerState.Planting) {
                Instantiate(GameManager.instance.tm.towerPrefab, transform.position, Quaternion.identity);
                _state = PlayerState.Moving;
            }
            if(_state == PlayerState.Moving && (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Jump"))) {
                if (GameManager.instance.tm.CanPlaceTower(GameManager.instance.tm.towerPrefab.GetComponent<TowerBehaviour>())) {
                    _state = PlayerState.Planting;
                    plantTimeStamp = Time.time + plantCooldown;
                } else {
                    // TODO: Play sound effect
                    Debug.Log("Cannot place tower");
                }
            }
        }
        if (_state == PlayerState.Moving)
        {
            Player.Move();
        }
        //check if player is in ConvexHull with isPointInsideConvexHull from ConvexHullManager
        if (GameManager.instance.chm.IsPointInsideConvexHull(this.transform.position))
        {
            //add 1 health each second
            deltaTime += Time.deltaTime;
            if (deltaTime > 1)
            {
                this.health++;
                deltaTime = 0;
            }
        }
        else
            deltaTime = 0;

    }


    public override void TakeDamage(int damage)
    {
        Debug.Log("Player took " + damage + " damage");
        health -= damage;
        if (health <= 0)
        {
            Debug.Log("Player died");
            Time.timeScale = 0f;
            EndScreen.SetActive(true);
            //set pause true
            GameManager.instance.pause = true;
        }
    }

    public void Move()
    {
        // Get the input from the player
        // Gives a value between -1 and 1
        player = this.gameObject;

        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        if (Input.GetMouseButton(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        // Move the player
        if (horizontal != 0 || vertical != 0)
        {
            player.transform.position += new Vector3(horizontal, vertical, 0) * runSpeed * Time.deltaTime;
            mousePosition = player.transform.position;
        }
        else
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(mousePosition.x, mousePosition.y, 0), runSpeed * Time.deltaTime);
        }
    }

    public SpriteRenderer GetSpriteRenderer(){
        return sr;
    }

    public float GetViewDistance(){
        return viewDistance;
    }
    
}

