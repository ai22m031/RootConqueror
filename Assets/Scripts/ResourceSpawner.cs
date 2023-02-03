using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Range(1, 10)]
    public int RESOURCE_COUNT = 10;
    
    [Range(1.0f, 10.0f)]
    public float RESOURCE_RADIUS = 10.0f;

    [Range(1, 10)]
    public int START_RESOURCE_COUNT = 4; 

    public Vector2 startPosition = new Vector2(0.0f, 0.0f);


    public GameObject resourcePrefab;

    // Start is called before the first frame update
    void Start()
    {  
        spawnRandomResources();
        spawnStartResources();
        GameManager.instance.resourceManager.refreshActiveResources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawnRandomResources()
    {
        for (int i = 0; i < RESOURCE_COUNT; i++) {
            GameManager.instance.resourceManager.add(intantiateResource(generateRandomVector()));
        }
    }

    private void spawnStartResources()
    {
        for (int i = 0; i < START_RESOURCE_COUNT; i++) {
            GameManager.instance.resourceManager.add(intantiateResource(new Vector3(startPosition.x, startPosition.y, 0.0f)));
        }
    }

    private GameObject intantiateResource(Vector3 position) 
    {
        return Instantiate(resourcePrefab, position, Quaternion.identity, this.transform);
    }

    private Vector3 generateRandomVector() 
    {
        return new Vector3(Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), 0.0f);
    }
}
