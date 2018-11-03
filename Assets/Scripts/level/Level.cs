using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        private System.Random random = new System.Random();

        public List<RectInt> Rooms { set; get; } = new List<RectInt>();

        public CustomGrid<Cell> Cells { get; } = new CustomGrid<Cell>();

        public CustomGrid<Monster> Monsters { get; } = new CustomGrid<Monster>();

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

        public bool AddCell(Cell cell)
        {
            var existingCell = GetCellAt(cell.Location.x, cell.Location.y);
            if (existingCell != null)
            {
                return false;
            }

            try
            {
                Cells.Add(cell.Location.x, cell.Location.y, cell);
                return true;
            }
            catch (IndexOutOfRangeException)
            {
                return false;
            }
        }

        public Cell GetCellAt(int x, int y)
        {
            return Cells.GetAt(x, y);
        }

        public bool AddMonster(Monster monster)
        {
            var cell = GetCellAt(monster.Location.x, monster.Location.y);
            var existingMonster = GetMonsterAt(monster.Location.x, monster.Location.y);
            if (cell != null && existingMonster == null)
            {
                try
                {
                    Monsters.Add(monster.Location.x, monster.Location.y, monster);
                    return true;
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
                }
            }
            return false;
        }

        public Monster GetMonsterAt(int x, int y)
        {
            return Monsters.GetAt(x, y);
        }

        public void RemoveMonster(Monster monster)
        {
            Monsters.Remove(monster.Location.x, monster.Location.y, monster);
            Destroy(monster.gameObject);
            Destroy(monster);
        }

        public void MoveMonster(Vector2Int from, Monster monster)
        {
            Monsters.Remove(from.x, from.y);
            Monsters.AddExisting(monster.Location.x, monster.Location.y, monster);
        }
    }
}
