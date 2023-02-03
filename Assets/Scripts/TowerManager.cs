using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject [] getLocations() {
        GameObject [] towers = GameObject.FindGameObjectsWithTag("Tower");
        /*
        List<Vector2> locations = new List<Vector2>();
        foreach (GameObject tower in towers) {
            locations.Add(tower.transform.position);
        }*/
        return towers;
    }
}
