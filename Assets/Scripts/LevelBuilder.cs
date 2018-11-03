﻿using System;
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

    public void Build(Level.Level level)
    {
        if (level.IsEmpty)
        {
            return;
        }

        foreach (var cell in level.Cells)
        {
            Vector3 newPosition = new Vector3
                    (
                    transform.position.x + cell.Location.x * cellWidth,
                    0.0f,
                    transform.position.y + cell.Location.y * cellHeight
                    );
            Instantiate(Floor, newPosition, transform.rotation);
        }
    }

    public Vector3 LevelLocationToWorldPosition(Vector2Int levelLocation)
    {
        return new Vector3
            (
            transform.position.x + levelLocation.x * cellWidth,
            1.0f,
            transform.position.z + levelLocation.y * cellHeight
            );
    }
}
