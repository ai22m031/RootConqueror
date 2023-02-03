using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float ATTACK_RANGE = 0.5f;
    public const float speed = 3f;
    public GameObject Visual;
    private int health = 6;
    

    public enum State {
        Searching,
        Attacking
    }

    private State _state;
    private GameObject target;
    private float attackCooldownTS = 0f;
    private float attackCooldown = 1f;

    public Animator animator;
    
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
                //rotate towards target
                if (target == null) return; 
                Vector2 direction = (target.transform.position - this.transform.position);
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                Visual.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 1f);
                
                animator.SetBool("walking", true);
                
                break;
            case State.Attacking:
                AttackEnemy();
                animator.SetBool("walking", false);

                break;
        }
    }

    void SearchEnemy()
    {
        GameObject [] locations = GameManager.instance.tm.GetGameObjects();
        Vector2 playerPos = GameManager.instance._player.transform.position;
        float minDis = Vector2.Distance(playerPos, this.transform.position);
        GameObject closestLocation = GameManager.instance._player;
        foreach(GameObject loc in locations) {
            float newDis = Vector2.Distance(loc.transform.position, this.transform.position);
            if(newDis < minDis) {
                minDis = newDis;
                closestLocation = loc;
            }
        }
        target = closestLocation;

        if (minDis < ATTACK_RANGE) {
            _state = State.Attacking;
        } else {
            Vector2 direction = (closestLocation.transform.position - this.transform.position);
            direction.Normalize();
            this.transform.Translate(direction * speed * Time.deltaTime);
        }
    }

    void AttackEnemy()
    {
        if(target == null) {
            _state = State.Searching;
            return;
        }
        if(Time.time > attackCooldownTS) {
            target.GetComponent<AlliedObjectBehaviour>().TakeDamage(1);
            _state = State.Searching;
            attackCooldownTS = Time.time + attackCooldown;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0) {
            GameManager.instance.em.RemoveEnemy(this);
            Destroy(this.gameObject);
            GameManager.instance.em.enemiesKilled++;

        }
    }
}
