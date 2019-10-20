using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindFloorSpawnManager : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject floorMoveUp, floorMoveDown, floorDodge, floorStop, floorRest;

    [Header("Configuration")]
    public float InstanceDistance = 6f;
    public float yPosition = -1.05f;


    public void SpawnFloor(GameObject player, int ID_Floor, bool isTop)
    {
        if (ID_Floor == 0) // MoveUp
        {
            Vector3 floorPosition = player.transform.position + new Vector3(InstanceDistance, yPosition, -2.65f);
            Instantiate(floorMoveUp, floorPosition, player.transform.rotation);
        }
        else if (ID_Floor == 1) // MoveDown
        {
            Vector3 floorPosition = player.transform.position + new Vector3(InstanceDistance, yPosition, -2.92f);
            Instantiate(floorMoveDown, floorPosition, player.transform.rotation);
        }
        else if(ID_Floor == 2) // Dodge
        {
            Vector3 floorPosition = player.transform.position;
            if (isTop)
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, yPosition, -2.9f);
            }
            else
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, yPosition, -2.7f);
            }
            
            Instantiate(floorDodge, floorPosition, player.transform.rotation);
        }
        else if(ID_Floor == 3) // Stop
        {
            Vector3 floorPosition = player.transform.position;
            if (isTop)
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, yPosition, -0.175f);
            }
            else
            {
                floorPosition = player.transform.position + new Vector3(InstanceDistance, yPosition, 0.05f);
            }
            
            Instantiate(floorStop, floorPosition, player.transform.rotation);
        }
        else // Rest
        {
            Vector3 floorPosition = player.transform.position + new Vector3(InstanceDistance, yPosition, 0.0f);
            Instantiate(floorRest, floorPosition, player.transform.rotation);
        }
        
    }
}
