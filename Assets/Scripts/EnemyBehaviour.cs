using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float ATTACK_RANGE = 0.5f;
    public const float speed = 2.5f;
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
    bool isDead;
    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            animator.SetBool("isDead", true);
            //check if animator is running
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("EnemyDeath") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                Destroy(this.gameObject);
            }

            return;
        }    
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
                //check distance to player
                if (target == null) return;

                if (Vector2.Distance(target.transform.position, this.transform.position) > ATTACK_RANGE)
                {
                    _state = State.Searching;
                    break;
                }
                AttackEnemy();
                animator.SetBool("walking", false);

                break;
        }
    }

    void SearchEnemy()
    {
        GameObject [] towers = GameManager.instance.tm.towers.ToArray();
        Vector2 playerPos = GameManager.instance._player.transform.position;
        float minDis = Vector2.Distance(playerPos, this.transform.position);
        GameObject closestTarget = GameManager.instance._player;
        foreach(GameObject tower in towers) {
            float newDis = Vector2.Distance(tower.transform.position, this.transform.position);
            if(newDis < minDis) {
                minDis = newDis;
                closestTarget = tower;
            }
        }
        target = closestTarget;

        if (minDis < ATTACK_RANGE) {
            _state = State.Attacking;
        } else {
            Vector2 direction = (target.transform.position - this.transform.position);
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
        if(health <= 0 && isDead == false) {
            isDead = true;
            GameManager.instance.em.RemoveEnemy(this);
            GameManager.instance.em.enemiesKilled++;
        }
    }
}
