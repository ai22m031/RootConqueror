using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour{
    [SerializeField] private int _xTiles;
    [SerializeField] private int _yTiles;
    [SerializeField] private float _tileWidth;
    [SerializeField] private float _tileHeight;
    [SerializeField] private GameObject _tilePrefab;
    [SerializeField] private GameObject _centerPoint;

    [SerializeField] private Vector3 minPos = new Vector3(0,0,0);
    [SerializeField] private Vector3 maxPos;
    
    // Start is called before the first frame update
    void Start(){
        maxPos = new Vector3(_xTiles * _tileWidth, _yTiles * _tileHeight, 0);
        for (int i = 0; i < _xTiles; i++){
            for (int j = 0; j < _yTiles; j++){
                GameObject tile = Instantiate(_tilePrefab, transform, true);
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                //float width = sr.sprite.rect.width / 2;
                //float height = sr.sprite.rect.height / 2;
                Vector2 position = new Vector2(i * _tileWidth, j * _tileHeight);
                tile.transform.position = position;
            }    
        }

        _centerPoint.transform.position =
            new Vector3(Random.Range(minPos.x, maxPos.x), Random.Range(minPos.y, maxPos.y), 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}