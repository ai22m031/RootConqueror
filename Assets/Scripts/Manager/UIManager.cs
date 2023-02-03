using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//import TextMeshProUGui
using TMPro;
using static System.Net.Mime.MediaTypeNames;
//import UI
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI text_currentEnergy;
    public TextMeshProUGUI text_maxEnergy;

    public TextMeshProUGUI text_towerAmount;

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
        health = (float)GameManager.instance._player.GetComponent<PlayerAction>().health / (float)GameManager.instance._player.GetComponent<PlayerAction>().maxHealth;
    }

    public void updateCurrentEnergy(int currentEnergy) {
        text_currentEnergy.text = currentEnergy.ToString();
    }

    public void updateMaxEnergy(int maxEnergy) {
        text_maxEnergy.text = maxEnergy.ToString();
    }
}
