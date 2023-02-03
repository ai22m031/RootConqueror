using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> resources {get; private set;} = new List<GameObject>();
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void add(GameObject resource) 
    {
        resources.Add(resource);
    }

    public int countActiveResources()
    {
        int count = 0;
        
        resources.ForEach(resource => {
            if (GameManager.instance.chm.isPointInsideConvexHull(resource.transform.position))
            {
                resource.GetComponent<Resource>().collect();
                count++;
            } else {
                resource.GetComponent<Resource>().uncollect();
            }
        });

        GameManager.instance.uim.updateCurrentEnergy(count);

        return count;
    }

    public int countMaxResources()
    {
        GameManager.instance.uim.updateMaxEnergy(resources.Count);
        return resources.Count;
    }
}
