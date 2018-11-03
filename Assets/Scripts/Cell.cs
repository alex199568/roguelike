using UnityEngine;

public class Cell
{
    public Cell(int x, int y)
    {
        Location = new Vector2Int(x, y);
    }

    public Vector2Int Location { get; set; }
}
