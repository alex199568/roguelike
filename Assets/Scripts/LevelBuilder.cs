using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    public int width;
    public int height;

    void Start()
    {
        Level level = GenerateLevel();
        BuildLevel(level);
    }

    private void BuildLevel(Level level)
    {
        if (level.IsEmpty)
        {
            return;
        }

        Vector2 cellSize = level.CellSize;

        float xOffset = width * cellSize.x / 2.0f;
        float zOffset = height * cellSize.y / 2.0f;

        foreach (KeyValuePair<Vector2Int, Cell> cell in level)
        {
            GameObject newCell = cell.Value.CellObject;
            Vector3 newPosition = new Vector3(
                transform.position.x + cell.Key.x * cellSize.x - xOffset,
                0.0f,
                transform.position.y + cell.Key.y * cellSize.y - zOffset
            );
            Instantiate(newCell, newPosition, transform.rotation);
        }
    }

    private Level GenerateLevel()
    {
        Level result = new Level();
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                Cell cell;
                if (Random.value > 0.5)
                {
                    cell = new Cell(floor);
                }
                else
                {
                    cell = new Cell(wall);
                }
                result.AddCell(new Vector2Int(i, j), cell);
            }
        }
        return result;
    }

    class Level : IEnumerable
    {
        private Dictionary<Vector2Int, Cell> cells = new Dictionary<Vector2Int, Cell>();
        private Vector2 cellSize;

        public Vector2 CellSize
        {
            get { return cellSize; }
        }

        public bool IsEmpty
        {
            get { return cells.Count == 0; }
        }

        public void AddCell(Vector2Int position, Cell cell)
        {
            cells.Add(position, cell);

            // TODO: optimize
            Vector3 size = cell.CellObject.GetComponent<Renderer>().bounds.size;
            cellSize = new Vector2(size.x, size.z);
        }

        public Cell GetCellAt(Vector2Int position)
        {
            return cells[position];
        }

        public IEnumerator GetEnumerator()
        {
            return cells.GetEnumerator();
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
