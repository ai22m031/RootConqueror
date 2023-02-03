using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    float horizontal;
    float vertical;

    Vector3 mousePosition;
    private GameObject player;

    public float runSpeed = 20.0f;
    //load script into script
    private PlayerAction Player;

    bool isPlanting = false;

    public int health;
    // Start is called before the first frame update
    void Start()
    {
        this.health = 5;
        Player = GameManager.instance._player.GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {

        isPlanting = Player.PlantCheck();
        if (!isPlanting)
        {
            Player.Move();
        }

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Time.timeScale = 0f;
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

    public bool PlantCheck()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetButtonDown("Jump"))
        {
            Vector3 playerPosition = GameManager.instance._player.transform.position;

            TowerBehaviour dT = Instantiate(GameManager.instance.tm.towerPrefab, GameManager.instance.tm.transform, true);
            dT.transform.position = playerPosition;
            GameManager.instance.tm.AddTower(dT);
            GameManager.instance.chm.CreateConvexHull();
            return true;
        }
        return false;
    }
}

