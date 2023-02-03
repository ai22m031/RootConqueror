using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : AlliedObjectBehaviour
{
    private const float ATTACK_RANGE = 4f;
    private float attackCooldown = 0.3f, attackCooldownTS = 0f;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        this.health = 5;
    }

    // Update is called once per frame
    void Update()
    {
        float minDis = float.MaxValue;
        GameObject closestEnemy = null;
        GameManager.instance.lm.em.GetEnemies().ForEach(enemy => {
            float newDis = Vector2.Distance(enemy.transform.position, this.transform.position);
            if(newDis < minDis) {
                minDis = newDis;
                closestEnemy = enemy;
            }
        });
        if (minDis < ATTACK_RANGE) {
            if (Time.time > attackCooldownTS) {
                GameObject newBullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
                newBullet.GetComponent<BulletBehaviour>().direction = (closestEnemy.transform.position - this.transform.position).normalized;
                attackCooldownTS = Time.time + attackCooldown;
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.lm.tm.RemoveTower(this);
            Destroy(gameObject);
        }
    }
}
