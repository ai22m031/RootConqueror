using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public const float ATTACK_RANGE = 0.5f;
    public const float speed = 3f;

    public enum State {
        Searching,
        Attacking
    }

    private State _state;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        _state = State.Searching;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_state) {
            case State.Searching:
                SearchEnemy();
                break;
            case State.Attacking:
                AttackEnemy();
                break;
        }
    }

    void SearchEnemy()
    {
        GameObject [] locations = GameManager.instance.lm.tm.getLocations();
        Vector2 playerPos = GameManager.instance.lm._player.transform.position;
        float minDis = Vector2.Distance(playerPos, this.transform.position);
        GameObject closestLocation = GameManager.instance.lm._player;
        foreach(GameObject loc in locations) {
            float newDis = Vector2.Distance(loc.transform.position, this.transform.position);
            if(newDis < minDis) {
                minDis = newDis;
                closestLocation = loc;
            }
        }
        if(minDis < ATTACK_RANGE) {
            _state = State.Attacking;
            target = closestLocation;
        } else {
            this.transform.position = Vector2.MoveTowards(this.transform.position, closestLocation.transform.position, speed * Time.deltaTime);
        }
    }

    void AttackEnemy()
    {
        target.GetComponent<TowerScript>().TakeDamage(1);
        _state = State.Searching;
    }
}
