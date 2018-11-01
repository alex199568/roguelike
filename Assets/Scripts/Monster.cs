using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private System.Random random = new System.Random();

    private Vector3 targetPosition;
    private Vector2Int location;

    public Vector3 TargetPosition
    {
        get { return targetPosition; }
        set { targetPosition = value; }
    }

    public Vector2Int Location
    {
        get { return location; }
        set { location = value; }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public Vector2Int Move(Level level)
    {
        Vector2Int newLocation;
        Cell cell;
        do
        {
            newLocation = new Vector2Int(location.x + random.Next(-1, 2), location.y + random.Next(-1, 2));
            cell = level.GetCellAt(newLocation.x, newLocation.y);
        } while (cell == null);
        location = newLocation;
        return newLocation;
    }
}
