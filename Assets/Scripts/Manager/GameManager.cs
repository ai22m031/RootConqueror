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
    public CameraManager cm;
    [Header("Player")]
    public GameObject _player;
    public GameObject _StartObject;
    private DateTime _totaltime;
    private bool _gamerunning;

    //getter setter for _gameRunning
    public bool gameRunning
    {
        get { return _gamerunning; }
        set { _gamerunning = value; }
    }
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
        Instantiate(_StartObject, new Vector3(), Quaternion.identity);

    }

    public void EndGame()
    {
        UnityEngine.Debug.Log("Player died");
        gameRunning = false;
        uim.EndScreen.SetActive(true);
        Time.timeScale = 0;

        pause = true;
    }
    void Start()
    {
        
        totaltime = DateTime.Now;
        gameRunning = true;
    }



    public bool pause;
    public void SetPause(bool x)
    {
        pause = x;
    }
 

    // Update is called once per frame
    void Update()
    {
        if (pause)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;

        }
    }
    
    
}
