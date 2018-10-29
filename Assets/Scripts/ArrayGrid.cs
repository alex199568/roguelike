using System;
using System.Collections;
using System.Collections.Generic;

public class ArrayGrid<E>
{
    private const int InitialWidth = 12;
    private const int InitialHeight = 12;

    private int width = 0;
    private int height = 0;

    private E[,] grid = new E[InitialWidth, InitialHeight];

    public int Width
    {
        get { return width; }
    }

    public int Height
    {
        get { return height; }
    }

    public void Set(int x, int y, E value)
    {
        EnsureCapacity(x, y);
        grid[x, y] = value;
    }

    public E Get(int x, int y)
    {
        return grid[x, y];
    }

    public bool IsEmpty
    {
        get { return width == 0 || height == 0; }
    }

    private void EnsureCapacity(int x, int y)
    {
        if (x >= grid.GetLength(0))
        {
            GrowWidth(x);
        }
        if (x >= width)
        {
            width = x + 1;
        }

        if (y >= grid.GetLength(1))
        {
            GrowHeight(y);
        }
        if (y >= height)
        {
            height = y + 1;
        }
    }

    private void GrowWidth(int min)
    {
        E[,] newGrid = new E[min * 2, grid.GetLength(1)];
        CopyToNewGrid(grid, newGrid);
        grid = newGrid;
    }

    private void GrowHeight(int min)
    {
        E[,] newGrid = new E[grid.GetLength(0), min * 2];
        CopyToNewGrid(grid, newGrid);
        grid = newGrid;
    }

    private void CopyToNewGrid(E[,] oldGrid, E[,] newGrid)
    {
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                newGrid[i, j] = oldGrid[i, j];
            }
        }
    }
}
