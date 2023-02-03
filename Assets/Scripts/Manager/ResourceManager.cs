using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
//using Random



public class ResourceManager : MonoBehaviour
{

    public int baseRessources;
    
    [Range(1, 400)]
    public int maxRessources = 10;

    [Range(1.0f, 400.0f)]
    public float RESOURCE_RADIUS = 10.0f;

    [Range(1, 10)]
    public int START_RESOURCE_COUNT = 4;

    public Vector2 startPosition = new Vector2(0.0f, 0.0f);
    [HideInInspector]
    public List<GameObject> resources { get; private set; } = new List<GameObject>();

    [HideInInspector]
    public int resourceCount { get; private set; } = 0;



    public GameObject resourcePrefab;

    // Start is called before the first frame update
    void Start()
    {
        FindAllResources();
        spawnRandomResources();
        //spawnStartResources();
        getActiveResourceAmount();
    }

    //Find All Resources
    public void FindAllResources()
    {
        //empty resources
        resources.Clear();
        resources.AddRange(GameObject.FindGameObjectsWithTag("Resource"));
        UnityEngine.Debug.Log("Yooooo");
        //Debug resources.Count
        UnityEngine.Debug.Log(resources.Count);

    }

    public int getActiveResourceAmount()
    {
        FindAllResources();
        int count = 0;
        resources.ForEach(resource => {
            if (GameManager.instance.chm.IsPointInsideConvexHull(resource.transform.position))
            {
                resource.GetComponent<Resource>().collect();
                count += resource.GetComponent<Resource>().value;
            }
            else
            {
                resource.GetComponent<Resource>().uncollect();
            }
        });

        resourceCount = count + baseRessources;
        return resourceCount;
    }


    private GameObject InstantiateRessourceAtLocation(Vector3 position)
    {
        return Instantiate(resourcePrefab, position, Quaternion.identity, this.transform);
    }

    public void spawnRandomResources()
    {
        int spawnedRessources = 0;
        //spawn resources
        while (spawnedRessources < maxRessources)
        {
            Vector3 position = new Vector3(Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), 0.0f);
            resources.ForEach(resource =>
            {
                if (Vector3.Distance(resource.transform.position, position) < 1.5f)
                {
                    position = new Vector3(Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), Random.Range(-RESOURCE_RADIUS, RESOURCE_RADIUS), 0.0f);
                }
            });
            InstantiateRessourceAtLocation(position);
            spawnedRessources++;
        }
    }
}
