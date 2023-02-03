using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : AlliedObjectBehaviour
{
    private const float ATTACK_RANGE = 4f;
    private float attackCooldown = 0.3f, attackCooldownTS = 0f;
    public int cost = 1;
    public GameObject bullet;
    // Start is called before the first frame update
    void Start()
    {
        this.health = 5;
        GameManager.instance.tm.AddTower(this);
        GameManager.instance.chm.CreateConvexHull();
        GameManager.instance.resourceManager.countActiveResources();
        GameManager.instance.tm.UpdateCosts();
    }

    // Update is called once per frame
    void Update()
    {
        float minDis = float.MaxValue;
        GameObject closestEnemy = null;
        GameManager.instance.em.GetEnemies().ForEach(enemy => {
            float newDis = Vector2.Distance(enemy.transform.position, this.transform.position);
            if(newDis < minDis) {
                minDis = newDis;
                closestEnemy = enemy;
            }
        });
        if(closestEnemy != null) {
            Vector2 direction = (closestEnemy.transform.position - this.transform.position);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, rotation, 1f);
        }
        if (minDis < ATTACK_RANGE) {
            if (Time.time > attackCooldownTS) {
                this.transform.localScale = new Vector3(0.15f, 0.2f, 1f);
                GameObject newBullet = Instantiate(bullet, this.transform.position, Quaternion.identity);
                newBullet.GetComponent<BulletBehaviour>().direction = (closestEnemy.transform.position - this.transform.position).normalized;
                attackCooldownTS = Time.time + attackCooldown;
            }
            else {
                this.transform.localScale = new Vector3(0.15f + 0.05f * ((attackCooldownTS - Time.time) / attackCooldown), 0.2f, 1f);
            }
        }
    }

    public override void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            GameManager.instance.tm.RemoveTower(this);
            Destroy(gameObject);
        }
    }
    
    public float GetAttackRange (){
        return ATTACK_RANGE;
    }
}
