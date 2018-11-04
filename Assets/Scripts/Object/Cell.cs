using UnityEngine;

namespace Object
{
    public class Cell : MonoBehaviour
    {
        public Cell(int x, int y)
        {
            Location = new Vector2Int(x, y);
        }

        public Vector2Int Location { get; set; }
    }
}
