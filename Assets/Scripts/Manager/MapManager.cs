using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Transform map;
    [SerializeField] private float speed;
    [SerializeField] private bool moveMap;

    private void FixedUpdate()
    {
        if (moveMap)
        {
            map.position = new Vector3(map.position.x - speed, map.position.y);
        }
       
    }
}
