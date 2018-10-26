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

        foreach (KeyValuePair<Vector2Int, Cell> cell in level)
        {
            GameObject newCell = cell.Value.CellObject;
            Vector3 newPosition = new Vector3(
                transform.position.x + cell.Key.x * cellSize.x,
                0.0f,
                transform.position.y + cell.Key.y * cellSize.y
            );
            Instantiate(newCell, newPosition, transform.rotation);
        }
    }

    private Level GenerateLevel()
    {
        Level result = new Level();
        PlaceRoom(result, new RectInt(1, 1, 4, 4));
        return result;
    }

    private void PlaceRoom(Level level, RectInt position)
    {
        Cell floorCell = new Cell(floor);
        Cell wallCell = new Cell(wall);

        for (int i = position.xMin + 1; i < position.xMax; ++i)
        {
            level.AddCell(new Vector2Int(i, position.yMin), wallCell);
            level.AddCell(new Vector2Int(i, position.yMax), wallCell);
        }

        for (int i = position.yMin + 1; i < position.yMax; ++i)
        {
            level.AddCell(new Vector2Int(position.xMin, i), wallCell);
            level.AddCell(new Vector2Int(position.xMax, i), wallCell);
        }

        level.AddCell(new Vector2Int(position.xMin, position.yMin), wallCell);
        level.AddCell(new Vector2Int(position.xMin, position.yMax), wallCell);
        level.AddCell(new Vector2Int(position.xMax, position.yMin), wallCell);
        level.AddCell(new Vector2Int(position.xMax, position.yMax), wallCell);
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
