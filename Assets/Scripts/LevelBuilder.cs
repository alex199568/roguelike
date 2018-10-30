using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public GameObject Floor;

    private float cellWidth;
    private float cellHeight;

    void Awake()
    {
        Vector3 size = Floor.GetComponent<Renderer>().bounds.size;
        cellWidth = size.x;
        cellHeight = size.z;
    }

    public void Build(Level level)
    {
        if (level.IsEmpty)
        {
            return;
        }

        for (int i = level.XMin; i < level.XMax; ++i)
        {
            for (int j = level.YMin; j < level.YMax; ++j)
            {
                Cell cell = level.GetCellAt(i, j);
                if (cell != null)
                {
                    Vector3 newPosition = new Vector3
                    (
                    transform.position.x + i * cellWidth,
                    0.0f,
                    transform.position.y + j * cellHeight
                    );
                    Instantiate(Floor, newPosition, transform.rotation);
                }
            }
        }
    }
}
