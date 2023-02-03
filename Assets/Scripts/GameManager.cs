using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LogicManager lm;
    public MusicManager mm;
    public UIManager uim;
    public ResourceManager resourceManager;

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
            instance = this;
        else
            throw new Exception("Created another instance of singleton Game Manager");
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
