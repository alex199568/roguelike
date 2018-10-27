using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
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
        PlaceRoom(result, new RectInt(0, 0, 24, 24));
        return result;
    }

    private void PlaceRoom(Level level, RectInt position)
    {
        Cell floorCell = new Cell(floor, CellType.Floor);
        Cell wallCell = new Cell(wall, CellType.Wall);

        for (int i = position.xMin + 1; i < position.xMax; ++i)
        {
            level.AddCell(i, position.yMin, wallCell);
            level.AddCell(i, position.yMax, wallCell);
        }

        for (int i = position.yMin + 1; i < position.yMax; ++i)
        {
            level.AddCell(position.xMin, i, wallCell);
            level.AddCell(position.xMax, i, wallCell);
        }

        level.AddCell(position.xMin, position.yMin, wallCell);
        level.AddCell(position.xMin, position.yMax, wallCell);
        level.AddCell(position.xMax, position.yMin, wallCell);
        level.AddCell(position.xMax, position.yMax, wallCell);

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
        private Vector2 cellSize;

        public Vector2 CellSize
        {
            get { return cellSize; }
        }

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
                Debug.LogWarningFormat("could not add cell at {0}, {1}", x, y);
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
        private CellType cellType;

        public CellType Type
        {
            get { return cellType; }
        }

        public GameObject CellObject
        {
            get { return cellObject; }
        }

        public Cell(GameObject cellObject, CellType cellType)
        {
            this.cellType = cellType;
            this.cellObject = cellObject;
        }
    }

    enum CellType
    {
        Floor, Wall
    }
}
