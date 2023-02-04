using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CameraManager : MonoBehaviour{
    [SerializeField] private Vector3 offset;

    private GameObject _player; 
    [SerializeField] private CamMode _camMode = CamMode.CenterPlayer;
    private GameManager gm;
    public enum CamMode{
        CenterPlayer,
        ClampPlayerAndTowers,
        CenterMiddle
    }

    private Camera mainCam;
    [SerializeField]
    private Camera miniMapCam;

    [SerializeField]private float minFOV = 10f;
    [SerializeField]private float maxFOV = 40f;
    [SerializeField]
    private float camModifcation = 0.2f;

    [SerializeField] private float lerpSpeed = 0.2f;
    [SerializeField] private float treshhold = 0.2f;
    
    public void Start(){
        mainCam = Camera.main;
        gm = GameManager.instance;
        _player = gm._player;
    }

    private void LateUpdate(){

        HandleMinimapCam();
        
        if (_camMode == CamMode.CenterPlayer){
            Vector3 newCamPos = new Vector3(_player.transform.position.x, _player.transform.position.y, mainCam.transform.position.z);
            mainCam.transform.position = newCamPos;
            
        }else if (_camMode == CamMode.ClampPlayerAndTowers){
            Vector3 newCamPos = mainCam.transform.position;
            //List<DummyTower> closestTowers = GetClosestXTowersToPlayer(3);
            List<GameObject> closestTowers = gm.tm.towers;
            
            if (closestTowers.Count > 2){
                if (gm.chm.GetConvexHullCentroid() == null){
                    gm.chm.CreateConvexHull();
                }
                Bounds bounds = CalculateBounds(closestTowers,true);
                if (gm.chm.IsPointInsideConvexHull(_player.transform.position)){
                    Vector3 centroid = gm.chm.GetConvexHullCentroid().transform.position;
                    newCamPos = new Vector3(centroid.x,centroid.y, mainCam.transform.position.z);
                }
                else{
                    Vector3 centroid = gm.chm.GetConvexHullCentroid().transform.position;
                    Vector3 helpPos = (centroid + _player.transform.position) / 2;
                    newCamPos = new Vector3(helpPos.x, helpPos.y, mainCam.transform.position.z);
                }

                //if (CalculateOrthographicSize(mainCam, bounds) + treshhold > mainCam.orthographicSize){ 
                if (!IsWithinCameraView(mainCam,bounds)){
                    IncreaseOrthographicSize(mainCam);
                }
                else{
                    DecreaseOrthographicSize(mainCam);
                }
            }
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, newCamPos, lerpSpeed/2 * Time.deltaTime);
        }else{
            mainCam.transform.position = miniMapCam.transform.position;
            mainCam.orthographicSize = 15;
        }
    }

    
    
    public List<TowerBehaviour> GetClosestXTowersToPlayer(int x){
        List<TowerBehaviour> closestTowers = new List<TowerBehaviour>();
        if (gm.tm.towers.Count > x){
            // Iterate over the objects
            foreach (GameObject sobject in gm.tm.towers)
            {
                TowerBehaviour tower = sobject.GetComponent<TowerBehaviour>(); 
                // Calculate the distance between the player and the current object
                float distance = Vector3.Distance(_player.transform.position, tower.transform.position);

                // Check if the closestElements list has less than numberOfElements elements
                if (closestTowers.Count < x){
                    // If the list is not full, add the current object to the list
                    closestTowers.Add(tower);
                }
                else{
                    // If the list is full, find the farthest element in the list
                    TowerBehaviour farthestElement = closestTowers[0];
                    float farthestDistance =
                        Vector3.Distance(_player.transform.position, farthestElement.transform.position);

                    for (int i = 1; i < closestTowers.Count; i++){
                        float currentDistance = Vector3.Distance(_player.transform.position,
                            closestTowers[i].transform.position);
                        if (currentDistance > farthestDistance){
                            farthestDistance = currentDistance;
                            farthestElement = closestTowers[i];
                        }
                    }

                    // If the current object is closer than the farthest element, remove the farthest element and add the current object
                    if (distance < farthestDistance){
                        closestTowers.Remove(farthestElement);
                        closestTowers.Add(tower);
                    }
                }
            }
        }
        else{
            List<TowerBehaviour> allTowers = new List<TowerBehaviour>();
            //fill allTowers from towers
            foreach (GameObject sobject in gm.tm.towers)
            {
                TowerBehaviour tower = sobject.GetComponent<TowerBehaviour>();
                allTowers.Add(tower);
            }
            closestTowers = allTowers;
        }

        return closestTowers;
    }

    bool IsWithinFOV(Camera camera, Vector3 position)
    {
        // Calculate the viewport position of the position
        Vector3 viewportPoint = camera.WorldToViewportPoint(position);

        // Check if the position is within the bounds of the viewport
        return viewportPoint.x >= 0f && viewportPoint.x <= 1f &&
               viewportPoint.y >= 0f && viewportPoint.y <= 1f &&
               viewportPoint.z >= 0f;
    }

    
    public Bounds CalculateBounds(List<GameObject> objectsInBounds,bool includePlayer)
    {
        Bounds bounds = new Bounds(objectsInBounds[0].transform.position, Vector3.zero);
        foreach (GameObject sobject in objectsInBounds)
        {
            TowerBehaviour tower = sobject.GetComponent<TowerBehaviour>();
            Renderer renderer = tower.GetComponent<Renderer>();
            bounds.Encapsulate(renderer.bounds);
            float attackRadius = tower.GetAttackRange();
            Vector3 attackRadiusVector = new Vector3(attackRadius, attackRadius, 0);
            bounds.Encapsulate(renderer.bounds.center + attackRadiusVector);
            bounds.Encapsulate(renderer.bounds.center - attackRadiusVector);
        }

        if (includePlayer){
            bounds = AddPlayerBounds(bounds,_player.GetComponent<PlayerAction>());
        }
        return bounds;
    }

    private Bounds AddPlayerBounds(Bounds bounds,PlayerAction pa){
        Renderer renderer = pa.GetSpriteRenderer();
        bounds.Encapsulate(renderer.bounds);
        float viewDistance = pa.GetViewDistance();
        Vector3 viewDistanceVector = new Vector3(viewDistance, viewDistance, 0);
        bounds.Encapsulate(renderer.bounds.center + viewDistanceVector);
        bounds.Encapsulate(renderer.bounds.center - viewDistanceVector);
        return bounds;
    }
    
    private bool IsWithinCameraView(Camera cam,Bounds bounds){
        Vector3 bottomLeft = cam.WorldToViewportPoint(bounds.min);
        Vector3 topRight = cam.WorldToViewportPoint(bounds.max);
        return bottomLeft.x >= 0 && bottomLeft.y >= 0 && topRight.x <= 1 && topRight.y <= 1;
    }

    private void IncreaseOrthographicSize(Camera cam)
    {
        if (cam.orthographicSize-treshhold < maxFOV){
            float targetOrthographicSize = cam.orthographicSize + camModifcation;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthographicSize, lerpSpeed * Time.deltaTime);
        }
    }
    
    private void DecreaseOrthographicSize(Camera cam)
    {
        if (cam.orthographicSize+treshhold > minFOV){
            float targetOrthographicSize = cam.orthographicSize - camModifcation;
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthographicSize, lerpSpeed * Time.deltaTime);
        }
    }
  

    void HandleMinimapCam(){
        if (gm.chm.GetConvexHullCentroid() != null){
            Vector3 centroidPos = gm.chm.GetConvexHullCentroid().transform.position;
            miniMapCam.transform.position = new Vector3(centroidPos.x, centroidPos.y, miniMapCam.transform.position.z);
            CalculateMiniMapZoom();
        }
        else{
            //miniMapCam.transform.position = new Vector3(_centerPont.transform.position.x, _centerPont.transform.position.y, miniMapCam.transform.position.z);
        }

        
    }

    void CalculateMiniMapZoom(){
        Vector3 newCamPos = new Vector3(_player.transform.position.x, _player.transform.position.y, miniMapCam.transform.position.z);
        //List<DummyTower> closestTowers = GetClosestXTowersToPlayer(3);
        List<GameObject> closestTowers = gm.tm.towers;
        if (closestTowers.Count > 0){
            Bounds bounds = CalculateBounds(closestTowers,true);
           
            if (!IsWithinCameraView(miniMapCam,bounds))
            {
                IncreaseOrthographicSize(miniMapCam);
            }
            else{
                DecreaseOrthographicSize(miniMapCam);
            }
            
        }
        Vector3 centroid = gm.chm.GetConvexHullCentroid().transform.position;
        miniMapCam.transform.position = new Vector3(centroid.x, centroid.y, miniMapCam.transform.position.z);
    }
 
    private float CalculateOrthographicSize(Camera cam, Bounds bounds)
    {
        Vector3 bottomLeft = cam.WorldToScreenPoint(bounds.min);
        Vector3 topRight = cam.WorldToScreenPoint(bounds.max);
        float sizeX = topRight.x - bottomLeft.x;
        float sizeY = topRight.y - bottomLeft.y;
        float size = Mathf.Max(sizeX, sizeY) * 0.5f / Screen.dpi * 0.0254f;
        return size / 2f;
    }
    
    
}
