using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour{

     public DummyTower dummyTowerPrefab;
    // Start is called before the first frame update

    public List<DummyTower> towers;
    
    void Start()
    {
        towers = new List<DummyTower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddTower(DummyTower tower){
        towers.Add(tower);
    }
    
    public List<Vector2> GetVector2s(){
        List<Vector2> transforms = new List<Vector2>();
        foreach (DummyTower dt in towers){
            transforms.Add(dt.transform.position);
        }
        return transforms;
    }
    
    public List<Transform> GetTransforms(){
        List<Transform> transforms = new List<Transform>();
        foreach (DummyTower dt in towers){
            transforms.Add(dt.transform);
        }
        return transforms;
    }
    
    public List<DummyTower> GetTowers(){
        return towers;
    }
    
}
