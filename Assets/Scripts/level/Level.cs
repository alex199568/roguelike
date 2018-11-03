using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Level
    {
        private System.Random random = new System.Random();

        public List<RectInt> Rooms { set; get; } = new List<RectInt>();

        public CustomGrid<Cell> Cells { get; } = new CustomGrid<Cell>();

        public RectInt RandomRoom
        {
            get { return Rooms[random.Next(Rooms.Count)]; }
        }

        public bool IsEmpty
        {
            get { return Cells.IsEmpty; }
        }

        public void AddRoom(RectInt room)
        {
            Rooms.Add(room);
        }

        public void AddCell(Cell cell)
        {
            Cells.Add(cell.Location.x, cell.Location.y, cell);
        }

        public Cell GetCellAt(int x, int y)
        {
            return Cells.GetAt(x, y);
        }
    }
}
