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

        for (int i = 0; i < level.Width; ++i)
        {
            for (int j = 0; j < level.Height; ++j)
            {
                Cell cell = level.GetCellAt(i, j);
                if (cell != null)
                {
                    GameObject newCell = cell.CellObject;
                    Vector3 newPosition = new Vector3(
                    transform.position.x + i * cellWidth,
                    0.0f,
                    transform.position.y + j * cellHeight);
                    Instantiate(newCell, newPosition, transform.rotation);
                }
            }
        }
    }

    private Level GenerateLevel()
    {
        Level result = new Level();
        RectInt room1Position = new RectInt(0, 0, 6, 4);
        RectInt room2Position = new RectInt(12, 8, 7, 10);
        RectInt room3Position = new RectInt(1, 12, 5, 3);
        RectInt room4Position = new RectInt(3, 10, 8, 15);
        PlaceRoom(result, room1Position);
        PlaceRoom(result, room2Position);
        PlaceRoom(result, room3Position);
        PlaceRoom(result, room4Position);
        ConnectWithPassage(result, room1Position, room2Position);
        ConnectWithPassage(result, room2Position, room3Position);
        ConnectWithPassage(result, room3Position, room1Position);
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

    class Level
    {
        private ArrayGrid<Cell> cellsGrid = new ArrayGrid<Cell>();

        public bool IsEmpty
        {
            get { return cellsGrid.IsEmpty; }
        }

        public int Width
        {
            get { return cellsGrid.Width; }
        }

        public int Height
        {
            get { return cellsGrid.Height; }
        }

        public void AddCell(int x, int y, Cell cell)
        {
            try
            {
                cellsGrid.Set(x, y, cell);
            }
            catch (IndexOutOfRangeException)
            {
                Debug.LogWarning($"could not add cell at {x}, {y}");
            }
        }

        public Cell GetCellAt(int x, int y)
        {
            return cellsGrid.Get(x, y);
        }
    }

    class Cell
    {
        private GameObject cellObject;

        public GameObject CellObject
        {
            get { return cellObject; }
        }

        public Cell(GameObject cellObject)
        {
            this.cellObject = cellObject;
        }
    }
}
