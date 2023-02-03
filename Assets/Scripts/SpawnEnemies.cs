using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public Vector2 spawnCenter;
    public GameObject enemyPrefab;
    float spawnRadius = 15f;
    float spawnRate = 1f;
    float timeStamp = 0f;
    float angle = 73f;
    float currSpawnAngle = 0f;

    // Start is called before the first frame update
    void Start()
    {
        //spawnCenter = GameManager.instance.lm._player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > timeStamp) {
            Vector2 spawnPos = new Vector2(spawnCenter.x + spawnRadius * Mathf.Cos(currSpawnAngle), spawnCenter.y + spawnRadius * Mathf.Sin(currSpawnAngle));
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            GameManager.instance.lm.em.AddEnemy(enemy);
            currSpawnAngle += angle * Random.Range(1f, 3f);
            timeStamp = Time.time + spawnRate;
        }
    }
}
