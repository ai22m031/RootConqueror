using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
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

    private float minFOV = 10f;
    private float maxFOV = 40f;
    [SerializeField]
    private float camModifcation = 0.2f;
    
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
            List<TowerBehaviour> closestTowers = gm.tm.towers;
            
            if (closestTowers.Count > 2){
                if (gm.chm.GetConvexHullCentroid() == null){
                    gm.chm.CreateConvexHull();
                }
                Bounds bounds = CalculateBounds(closestTowers);
                bounds = AddPlayerBounds(bounds, _player.GetComponent<PlayerAction>());
                if (gm.chm.IsPointInsideConvexHull(_player.transform.position)){
                    Vector3 centroid = gm.chm.GetConvexHullCentroid().transform.position;
                    newCamPos = new Vector3(centroid.x,centroid.y, mainCam.transform.position.z);
                }
                else{
                    Vector3 centroid = gm.chm.GetConvexHullCentroid().transform.position;
                    Vector3 helpPos = (centroid + _player.transform.position) / 2;
                    newCamPos = new Vector3(helpPos.x, helpPos.y, mainCam.transform.position.z);
                }
                if (!IsWithinCameraView(mainCam,bounds))
                {
                    IncreaseOrthographicSize(mainCam);
                }
                else{
                    DecreaseOrthographicSize(mainCam);
                }
            }
            mainCam.transform.position = newCamPos;
        }else{
            mainCam.transform.position = miniMapCam.transform.position;
            mainCam.orthographicSize = 15;
        }
    }

    void Test(){
        //float size = 0f;
        //Vector3 center = bounds.center;
        //Vector3 extents = bounds.extents;
        //Vector2 viewport = Camera.main.WorldToViewportPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z));
        //if (viewport.x >= 0f && viewport.x <= 1f && viewport.y >= 0f && viewport.y <= 1f)
        //{
        //    Vector3 boundSize = bounds.size;
        //    size = Mathf.Max(boundSize.x / GetComponent<Camera>().aspect, boundSize.y);
        //}
        //else
        //{
        //    Vector3 boundSize = bounds.size;
        //    size = Mathf.Max(boundSize.x / GetComponent<Camera>().aspect, boundSize.y);
        //    size /= 2f;
        //}
        //Camera.main.orthographicSize = Mathf.Clamp(size, minOrthographicSize, maxOrthographicSize);
    }
    
    public List<TowerBehaviour> GetClosestXTowersToPlayer(int x){
        List<TowerBehaviour> closestTowers = new List<TowerBehaviour>();
        if (gm.tm.GetTowers().Count > x){
            // Iterate over the objects
            foreach (TowerBehaviour tower in gm.tm.GetTowers()){
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
            closestTowers = gm.tm.GetTowers();
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


    private Bounds CalculateBounds(List<TowerBehaviour> objectsInBounds)
    {
        Bounds bounds = new Bounds(objectsInBounds[0].transform.position, Vector3.zero);
        foreach (TowerBehaviour tower in objectsInBounds)
        {
            Renderer renderer = tower.GetComponent<Renderer>();
            bounds.Encapsulate(renderer.bounds);
            float attackRadius = tower.GetAttackRange();
            Vector3 attackRadiusVector = new Vector3(attackRadius, attackRadius, 0);
            bounds.Encapsulate(renderer.bounds.center + attackRadiusVector);
            bounds.Encapsulate(renderer.bounds.center - attackRadiusVector);
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
    
    private bool IsWithinCameraView(Camera cam,Bounds bounds)
    {
        Vector3 bottomLeft = cam.WorldToViewportPoint(bounds.min);
        Vector3 topRight = cam.WorldToViewportPoint(bounds.max);
        return bottomLeft.x >= 0 && bottomLeft.y >= 0 && topRight.x <= 1 && topRight.y <= 1;
    }

    private void IncreaseOrthographicSize(Camera cam)
    {
        if (cam.orthographicSize < maxFOV){
            cam.orthographicSize+=camModifcation;
        }
    }
    
    private void DecreaseOrthographicSize(Camera cam)
    {
        if (cam.orthographicSize > minFOV){
            cam.orthographicSize-=camModifcation;
        }
    }
    
    private float CalculateOrthoSize(Camera cam,Bounds bounds)
    {
        Vector3 topRight = new Vector3(bounds.max.x, bounds.max.y, bounds.center.z);
        Vector3 topRightAsViewport = cam.WorldToViewportPoint(topRight);
        float size = topRight.y - bounds.center.y;
        return size;
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
        List<TowerBehaviour> closestTowers = gm.tm.towers;
        if (closestTowers.Count > 0){
            Bounds bounds = CalculateBounds(closestTowers);
            //bounds = AddPlayerBounds(bounds, player.GetComponent<PlayerMovement>());
           
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
 
    
    
    
}
