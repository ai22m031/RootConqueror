using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public static int RESOURCE_COUNT = 10;
    public static float RESOURCE_RADIUS = 10.0f;

    public GameObject resourcePrefab;
    
    private ResourceManager resourceManager;

    // Start is called before the first frame update
    void Start()
    {
        resourceManager = GameManager.instance.resourceManager;    
        spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void spawn()
    {
        for (int i = 0; i < RESOURCE_COUNT; i++) {
            resourceManager.add(intantiateResource());
        }
    }

    private GameObject intantiateResource() 
    {
        return Instantiate(resourcePrefab, generateRandomVector(), Quaternion.identity);
    }

    private Vector3 generateRandomVector() 
    {
        return new Vector3(Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), 0.0f);
    }
}
