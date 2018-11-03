public class ArrayGrid<E>
{
    private const int InitialWidth = 12;
    private const int InitialHeight = 12;
    private E[,] grid = new E[InitialWidth, InitialHeight];

    public int Width { get; private set; } = 0;

    public int Height { get; private set; } = 0;

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
        get { return Width == 0 || Height == 0; }
    }

    private void EnsureCapacity(int x, int y)
    {
        if (x >= grid.GetLength(0))
        {
            GrowWidth(x);
        }
        if (x >= Width)
        {
            Width = x + 1;
        }

        if (y >= grid.GetLength(1))
        {
            GrowHeight(y);
        }
        if (y >= Height)
        {
            Height = y + 1;
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
        for (int i = 0; i < Width; ++i)
        {
            for (int j = 0; j < Height; ++j)
            {
                newGrid[i, j] = oldGrid[i, j];
            }
        }
    }
}
