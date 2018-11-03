using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Level
    {
        private System.Random random = new System.Random();

        private CustomGrid<Cell> cells = new CustomGrid<Cell>();

        public List<RectInt> Rooms { set; get; } = new List<RectInt>();

        public RectInt RandomRoom
        {
            get { return Rooms[random.Next(Rooms.Count)]; }
        }

        public bool IsEmpty
        {
            get { return cells.IsEmpty; }
        }

        public int XMin
        {
            get { return cells.XMin; }
        }

        public int XMax
        {
            get { return cells.XMax; }
        }

        public int YMin
        {
            get { return cells.YMin; }
        }

        public int YMax
        {
            get { return cells.YMax; }
        }

        public void AddRoom(RectInt room)
        {
            Rooms.Add(room);
        }

        public void AddCell(int x, int y, Cell cell)
        {
            cells.Add(x, y, cell);
        }

        public Cell GetCellAt(int x, int y)
        {
            return cells.GetAt(x, y);
        }
    }
}
