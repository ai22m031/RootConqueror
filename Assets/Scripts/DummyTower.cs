using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTower : MonoBehaviour{

    [SerializeField] private Sprite _towerSprite;

    private SpriteRenderer _spriterRenderer;

    [SerializeField]
    private GameObject _miniMapIcon;
    // Start is called before the first frame update
    private void Start(){
        _spriterRenderer = GetComponent<SpriteRenderer>();
        _spriterRenderer.sprite = _towerSprite;
        Instantiate(_miniMapIcon, transform);
    }

}
