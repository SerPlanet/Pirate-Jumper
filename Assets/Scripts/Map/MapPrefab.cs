using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapPrefab : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private Transform endPosition;

    [SerializeField] private Tilemap tilemap;

    public Vector3 GetEndPosition()
    {
        float y = 0;
        float x ;
        //x = (startPosition.position.x * -1) + endPosition.position.x + transform.position.x;
        x= endPosition.position.x;
        y = endPosition.position.y;
        
        return new Vector3(x,y);
    }

    public Vector2 GetStartPos()
    {
        return startPosition;
    }

    public void SetStartPos(Vector2 startPos)
    {
        startPosition = startPos;
    }

    public Transform GetTransform()
    {
        return this.transform;
    }

    public Vector3 GetAnchorOriginWorldPos()
    {
        Vector3 loc = tilemap.localBounds.center;
        return new Vector3(loc.x,loc.y);
    }

    public void RemoveMapPrefab()
    {
        Destroy(gameObject);
    }
}
