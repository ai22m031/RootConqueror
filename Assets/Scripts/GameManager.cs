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

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
            instance = this;
        else
            throw new Exception("Created another instance of singleton Game Manager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
