using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public bool towersEnabled { get; set; } = true;
    // Start is called before the first frame update

    public List<TowerBehaviour> towers;
    public GameObject towerPrefab;

    public int TowerAmount = 0;
    public int towerCost = 0;
    void Start()
    {
        towers = new List<TowerBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public GameObject [] GetGameObjects() {
        GameObject [] locations = new GameObject[towers.Count];
        for(int i = 0; i < towers.Count; i++) {
            locations[i] = towers[i].gameObject;
        }
        return locations;
    }

    public void AddTower(TowerBehaviour tower){
        towerCost += tower.cost;
        towers.Add(tower);
        TowerAmount++;
        RefreshTowers();
    }
    
    public void UpdateCosts() {
        if(towerCost > GameManager.instance.resourceManager.refreshActiveResources()) {
            towersEnabled = false;
        } else {
            towersEnabled = true;
        }
    }

    public List<Vector2> GetVector2s(){
        List<Vector2> transforms = new List<Vector2>();
        foreach (TowerBehaviour dt in towers){
            transforms.Add(dt.transform.position);
        }
        return transforms;
    }
    
    public List<Transform> GetTransforms(){
        List<Transform> transforms = new List<Transform>();
        foreach (TowerBehaviour dt in towers){
            transforms.Add(dt.transform);
        }
        return transforms;
    }
    
    public List<TowerBehaviour> GetTowers(){
        return towers;
    }

    public void RefreshTowers()
    {
        GameManager.instance.chm.CreateConvexHull();
        UpdateCosts();
    }

    internal void RemoveTower(TowerBehaviour tower)
    {
        towers.Remove(tower);
        TowerAmount--;
        towerCost -= tower.cost;
        RefreshTowers();
    }

    public bool CanPlaceTower(TowerBehaviour tower)
    {
        return towerCost + tower.cost <= GameManager.instance.resourceManager.refreshActiveResources();
    }
}
