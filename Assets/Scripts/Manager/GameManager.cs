using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //Unity Editor Header 
    [Header("Managers")]
    public MusicManager mm;
    public UIManager uim;
    public ResourceManager resourceManager;
    public ConvexHullManager chm;
    public TowerManager tm;
    public EnemyManager em;
    [Header("Player")]
    public GameObject _player;
    public GameObject _StartObject;
    private DateTime _totaltime;
    //getter setter for totaltime
    public DateTime totaltime
    {
        get { return _totaltime; }
        set { _totaltime = value; }
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            throw new Exception("Created another instance of singleton Game Manager");
    }

    void Start()
    {
        
        Instantiate(_StartObject, new Vector3(), Quaternion.identity);
        totaltime = DateTime.Now;
    }



    private bool Pause;
    //getter setter for pause
    public bool pause
    {
        get { return Pause; }
        set { Pause = value; }
    }
 

    // Update is called once per frame
    void Update()
    {
        if (Pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;

        }
    }
}
