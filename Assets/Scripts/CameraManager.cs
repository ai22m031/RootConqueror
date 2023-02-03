using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;

    [SerializeField] private CamMode _camMode = CamMode.CenterPlayer;
    [SerializeField] private GameObject _centerPont;
    public enum CamMode{
        CenterPlayer,
        CenterMiddle
    }

    private Camera mainCam;
    [SerializeField]
    private Camera miniMapCam;
    
    public void Start(){
        mainCam = Camera.main;
    }

    private void LateUpdate(){
        miniMapCam.transform.position = new Vector3(_centerPont.transform.position.x, _centerPont.transform.position.y, miniMapCam.transform.position.z);
        if (_camMode == CamMode.CenterPlayer){
            Vector3 newCamPos = new Vector3(player.transform.position.x, player.transform.position.y, mainCam.transform.position.z);
            mainCam.transform.position = newCamPos;
            mainCam.orthographicSize = 5;
        }
        else{
            //Vector3 newCamPos = new Vector3(player.transform.position.x, player.transform.position.y, mainCam.transform.position.z);
            //mainCam.transform.position = newCamPos;
            mainCam.orthographicSize = 15;
        }
    }
    
}
