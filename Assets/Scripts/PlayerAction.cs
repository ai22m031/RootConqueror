using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
        private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        gm =  GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1)||Input.GetButton("Jump"))
        {
            Debug.Log("Tower created");
            Vector3 playerPosition = gm.lm._player.GetComponent<PlayerMovement>().PlayerPosition();

            TowerBehaviour dT = Instantiate(gm.lm.tm.towerPrefab, gm.lm.tm.transform, true);
            dT.transform.position= playerPosition;
            gm.lm.tm.AddTower(dT);
            gm.lm.chm.CreateConvexHull();
            gm.resourceManager.countActiveResources();
        }

        if (Input.GetButton("Jump"))
        {
            Debug.Log("Jump");
        }
        if( Input.GetMouseButtonDown(1)){
            Debug.Log("Right Click");
            }
    }
}
