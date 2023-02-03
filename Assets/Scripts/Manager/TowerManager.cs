using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public bool towersEnabled { get; set; } = true;
    // Start is called before the first frame update

    public List<GameObject> towers = new List<GameObject>();
    public GameObject towerPrefab;

    public int TowerAmount = 0;
    public int towerCost = 0;

    public void RefreshTowers()
    {
        GameManager.instance.chm.CreateConvexHull();
        //Call getActiveResourceAmount
        int activeRessources = GameManager.instance.resourceManager.getActiveResourceAmount();
    }

    public void AddTower(GameObject sobject)
    {
        TowerBehaviour tower = sobject.GetComponent<TowerBehaviour>();
        towerCost += tower.cost;
        towers.Add(sobject);
        TowerAmount++;
        RefreshTowers();
    }

    internal void RemoveTower(GameObject sobject)
    {
        TowerBehaviour tower = sobject.GetComponent<TowerBehaviour>();

        towers.Remove(sobject);
        TowerAmount--;
        towerCost -= tower.cost;
        RefreshTowers();
        if(TowerAmount < 3)
        {
            //EndGame
            GameManager.instance.EndGame();
        }
    }

    public bool CanPlaceTower(GameObject sobject)
    {
        TowerBehaviour tower = sobject.GetComponent<TowerBehaviour>();
        return towerCost + tower.cost <= GameManager.instance.resourceManager.getActiveResourceAmount();
    }
}
