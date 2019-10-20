using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindFloorSpawnManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject floorMoveUp, floorMoveDown, floorDodge, floorStop, floorRest;

    [Header("Configuration")]
    public float InstanceDistance = 6f;


    public void SpawnFloor(GameObject player, int ID_Floor, bool isTop)
    {
        

        if (ID_Floor == 0) // MoveUp
        {
            Vector3 floorPosition = player.transform.position + new Vector3(InstanceDistance, -1.10f, -1.8f);
            Instantiate(floorMoveUp, floorPosition, player.transform.rotation);
        }
        else if (ID_Floor == 1) // MoveDown
        {
            Vector3 floorPosition = player.transform.position + new Vector3(InstanceDistance, -1.10f, -2.91f);
            Instantiate(floorMoveDown, floorPosition, player.transform.rotation);
        }
        else if(ID_Floor == 2) // Dodge
        {
            Vector3 floorPosition = player.transform.position;
            if (isTop)
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, -1.10f, -2.91f);
            }
            else
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, -1.10f, -1.8f);
            }
            
            Instantiate(floorDodge, floorPosition, player.transform.rotation);
        }
        else if(ID_Floor == 3) // Stop
        {
            Vector3 floorPosition = player.transform.position;
            if (isTop)
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, -1.13f, 0.0f);
            }
            else
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, -1.13f, 1.0f);
            }
            
            Instantiate(floorStop, floorPosition, player.transform.rotation);
        }
        else // Rest
        {
            Vector3 floorPosition = player.transform.position + new Vector3(InstanceDistance, -1.13f, 0.0f);
            Instantiate(floorRest, floorPosition, player.transform.rotation);
        }
        
    }
}
