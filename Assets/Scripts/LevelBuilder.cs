using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public GameObject floor;
    public int width;
    public int height;

    private float cellWidth;
    private float cellHeight;

    void Start()
    {
        Vector3 size = floor.GetComponent<Renderer>().bounds.size;
        cellWidth = size.x;
        cellHeight = size.z;

        Level level = GenerateLevel();
        BuildLevel(level);
    }

    private void BuildLevel(Level level)
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
                    GameObject newCell = cell.CellObject;
                    Vector3 newPosition = new Vector3
                    (
                    transform.position.x + i * cellWidth,
                    0.0f,
                    transform.position.y + j * cellHeight
                    );
                    Instantiate(newCell, newPosition, transform.rotation);
                }
            }
        }
    }

    private Level GenerateLevel()
    {
        Level result = new Level();
        var rooms = new List<RectInt>()
        {
            new RectInt(21, 21, 14, 14),
            new RectInt(-26, 21, 14, 14),
            new RectInt(-26, -26, 14, 14),
            new RectInt(21, -26, 14, 14)
        };

        foreach (var room in rooms)
        {
            PlaceRoom(result, room);
        }

        ConnectWithPassage(result, rooms[0], rooms[1]);
        ConnectWithPassage(result, rooms[1], rooms[2]);
        ConnectWithPassage(result, rooms[2], rooms[3]);
        ConnectWithPassage(result, rooms[3], rooms[0]);

        ConnectWithPassage(result, rooms[0], rooms[2]);
        ConnectWithPassage(result, rooms[1], rooms[3]);

        return result;
    }

    private void ConnectWithPassage(Level level, RectInt room1, RectInt room2)
    {
        System.Random random = new System.Random();
        Cell floorCell = new Cell(floor);

        int startX = random.Next(room1.xMin + 1, room1.xMax - 1);
        int startY = random.Next(room1.yMin + 1, room1.yMax - 1);
        int endX = random.Next(room2.xMin + 1, room2.xMax - 1);
        int endY = random.Next(room2.yMin + 1, room2.yMax - 1);

        int stepX = startX < endX ? 1 : -1;
        int stepY = startY < endY ? 1 : -1;

        int i = startX;
        int j = startY;
        while (true)
        {
            bool xChanged = false;
            if (Math.Abs(startX - i) < Math.Abs(startX - endX))
            {
                i += stepX;
                xChanged = true;
            }

            bool yChanged = false;
            if (Math.Abs(startY - j) < Math.Abs(startY - endY))
            {
                j += stepY;
                yChanged = true;
            }

            if (xChanged && yChanged)
            {
                level.AddCell(i, j, floorCell);
                level.AddCell(i - stepX, j, floorCell);
                level.AddCell(i, j - stepY, floorCell);
            }
            else if (xChanged || yChanged)
            {
                level.AddCell(i, j, floorCell);
            }
            else
            {
                break;
            }
        }
    }

    private void PlaceRoom(Level level, RectInt position)
    {
        Cell floorCell = new Cell(floor);

        for (int i = position.xMin + 1; i < position.xMax; ++i)
        {
            for (int j = position.yMin + 1; j < position.yMax; ++j)
            {
                level.AddCell(i, j, floorCell);
            }
        }
    }

}
