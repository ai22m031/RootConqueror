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
    
    //setter getter for Image.fillAmount
    public float FillAmount
    {
        get { return Healthbar.fillAmount; }
        set { Healthbar.fillAmount = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateCurrentEnergy(int currentEnergy) {
        text_currentEnergy.text = currentEnergy.ToString();
    }

    public void updateMaxEnergy(int maxEnergy) {
        text_maxEnergy.text = maxEnergy.ToString();
    }
}
