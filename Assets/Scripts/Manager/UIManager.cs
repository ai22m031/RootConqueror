using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//import TextMeshProUGui
using TMPro;
using static System.Net.Mime.MediaTypeNames;
//import UI
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI text_currentEnergy;
    public TextMeshProUGUI text_maxEnergy;

    public TextMeshProUGUI text_score;

    public TextMeshProUGUI text_dsKills;
    public TextMeshProUGUI text_dsTime;
    public TextMeshProUGUI text_dsTowers;

    public TextMeshProUGUI text_towerAmount;
    public GameObject EndScreen;

    //public Image UI
    public UnityEngine.UI.Image Healthbar;
    private PlayerAction player;
    //setter getter for Image.fillAmount
    public float health
    {
        get { return Healthbar.fillAmount; }
        set { Healthbar.fillAmount = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameManager.instance._player.GetComponent<PlayerAction>();
    }

    // Update is called once per frame
    void Update()
    {
        //Set TowerAmount to TowerManager AMount
        text_towerAmount.text = GameManager.instance.tm.TowerAmount.ToString();
        
        updateCurrentEnergy(GameManager.instance.tm.TowerAmount);
        updateMaxEnergy(GameManager.instance.resourceManager.resourceCount);
        health = (float)GameManager.instance._player.GetComponent<PlayerAction>().health / (float)GameManager.instance._player.GetComponent<PlayerAction>().maxHealth;

        //get enemies killed vom enemybehaviour script
        int enemiesKilled = GameManager.instance.em.enemiesKilled;
        //set text to enemies killed
        text_score.text = enemiesKilled.ToString();

        //create timestamp and get from totaltime from GameManager
        DateTime timestamp = GameManager.instance.totaltime;
        //calculate delta time from timestamp and current time
        //if player is not dead
        if (GameManager.instance.gameRunning)
        {
            //set delta time to text
            int deltaTime = (int)(DateTime.Now - timestamp).TotalSeconds;
            text_dsTime.text = deltaTime.ToString();
        }
        
        text_dsKills.text = enemiesKilled.ToString();
        text_dsTowers.text = GameManager.instance.tm.TowerAmount.ToString();
        

    }

    private void updateCurrentEnergy(int currentEnergy) {
        text_currentEnergy.text = currentEnergy.ToString();
    }

    private void updateMaxEnergy(int maxEnergy) {
        text_maxEnergy.text = maxEnergy.ToString();
    }
}
