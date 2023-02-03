using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> enemies;
    void Start()
    {
        enemies = new List<GameObject>();
    } 
    
    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public List<GameObject> GetEnemies()
    {
        return enemies;
    }

    internal void RemoveEnemy(EnemyBehaviour enemyBehaviour)
    {
        enemies.Remove(enemyBehaviour.gameObject);
    }
}
