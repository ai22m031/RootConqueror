using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    public bool PlantCheck()
    {
        if (Input.GetMouseButtonDown(1)||Input.GetButton("Jump"))
        {
            Vector3 playerPosition = GameManager.instance.lm._player.transform.position;

            TowerBehaviour dT = Instantiate(GameManager.instance.lm.tm.towerPrefab, GameManager.instance.lm.tm.transform, true);
            dT.transform.position= playerPosition;
            GameManager.instance.lm.tm.AddTower(dT);
            GameManager.instance.lm.chm.CreateConvexHull();
            return true;
        }
        return false;
    }



}

