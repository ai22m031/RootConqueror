using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour{

    [SerializeField] private DummyTower _dummyTowerPrefab;
    // Start is called before the first frame update

    public List<DummyTower> towers;
    
    void Start()
    {
        towers = new List<DummyTower>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            mousePos.z = 0;

            DummyTower dT = Instantiate(_dummyTowerPrefab, mousePos, Quaternion.identity);
            towers.Add(dT);
        }
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
