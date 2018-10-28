using UnityEngine;

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
