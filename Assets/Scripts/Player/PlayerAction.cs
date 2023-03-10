using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

public class PlayerAction : AlliedObjectBehaviour{
    private SpriteRenderer sr;
    float horizontal;
    float vertical;
    private float viewDistance = 5f;

    Vector3 mousePosition;
    private GameObject player;
    public LineRenderer lr;
    public LineRenderer lr2;
    public float runSpeed = 20.0f;
    //load script into script
    private PlayerAction Player;
    public ParticleSystem LifeAdd;
    public ParticleSystem Planting;

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
                Planting.Stop();
                UnityEngine.Debug.Log("Planted tower");
            }
            if (_state == PlayerState.Moving && (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Jump"))) {
                if (GameManager.instance.tm.CanPlaceTower(GameManager.instance.tm.towerPrefab)) {
                    Planting.Play();
                    _state = PlayerState.Planting;
                    plantTimeStamp = Time.time + plantCooldown;
                } else {
                    // TODO: Play sound effect
                    UnityEngine.Debug.Log("Cannot place tower");
                }
            }
        }
        if (_state == PlayerState.Moving)
        {
            Player.Move();
        }
        //check if player is in ConvexHull with isPointInsideConvexHull from ConvexHullManager
        if (health < maxHealth && GameManager.instance.chm.IsPointInsideConvexHull(this.transform.position))
        {
            //add 1 health each second
            deltaTime += Time.deltaTime;
            if (deltaTime > 1)
            {
                this.health++;
                LifeAdd.Play();
                deltaTime = 0;
            }
        }
        else
            deltaTime = 0;

    }


    public override void TakeDamage(int damage)
    {
        UnityEngine.Debug.Log("Player took " + damage + " damage");
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.EndGame();
        }
    }

    public void Move()
    {
        // Get the input from the player
        // Gives a value between -1 and 1
        player = this.gameObject;

        horizontal = Input.GetAxisRaw("Horizontal"); // -1 is left
        vertical = Input.GetAxisRaw("Vertical"); // -1 is down

        if (Input.GetMouseButton(0)){
            if (!EventSystem.current.IsPointerOverGameObject()) {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // get the ray from the main camera
                RaycastHit hit;
                int layerMask = 1 << LayerMask.NameToLayer("Map");
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) // perform the raycast and check if it hit a collider
                {
                    mousePosition = hit.point; // get the position of the raycast hit
                    Debug.Log("hit to " + mousePosition);
                }
            }
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

