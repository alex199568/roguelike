using System;
using System.Collections.Generic;
using UnityEngine;

public class Level
{
    private ArrayGrid<Cell> gridI = new ArrayGrid<Cell>();
    private ArrayGrid<Cell> gridII = new ArrayGrid<Cell>();
    private ArrayGrid<Cell> gridIII = new ArrayGrid<Cell>();
    private ArrayGrid<Cell> gridIV = new ArrayGrid<Cell>();

    private List<RectInt> rooms = new List<RectInt>();

    private System.Random random = new System.Random(); // TODO: lazy

    public bool IsEmpty
    {
        get { return gridI.IsEmpty && gridII.IsEmpty && gridIII.IsEmpty && gridIV.IsEmpty; }
    }

    public int XMin
    {
        get
        { return -Math.Max(gridII.Width, gridIII.Width); }
    }

    public int XMax
    {
        get { return Math.Max(gridI.Width, gridIV.Width); }
    }

    public int YMin
    {
        get { return -Math.Max(gridIII.Height, gridIV.Height); }
    }

    public int YMax
    {
        get { return Math.Max(gridI.Height, gridII.Height); }
    }

    public List<RectInt> Rooms
    {
        set { rooms = value; }
        get { return rooms; }
    }

    public RectInt RandomRoom
    {
        get { return rooms[random.Next(rooms.Count)]; }
    }

    public void AddRoom(RectInt room)
    {
        rooms.Add(room);
    }

    public void AddCell(int x, int y, Cell cell)
    {
        try
        {
            ResolveGrid(x, y).Set(Math.Abs(x), Math.Abs(y), cell);
        }
        catch (IndexOutOfRangeException)
        {
            Debug.LogWarning($"could not add cell at {x}, {y}");
        }
    }

    public Cell GetCellAt(int x, int y)
    {
        try
        {
            return ResolveGrid(x, y).Get(Math.Abs(x), Math.Abs(y));
        }
        catch (IndexOutOfRangeException)
        {
            // ...
        }

        return null;
    }

    private ArrayGrid<Cell> ResolveGrid(int x, int y)
    {
        if (x >= 0 && y >= 0)
        {
            return gridI;
        }
        if (x < 0 && y < 0)
        {
            return gridIII;
        }
        if (x >= 0)
        {
            return gridIV;
        }
        return gridII;
    }
}
