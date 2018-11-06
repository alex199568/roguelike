using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        private System.Random random = new System.Random();

        public List<RectInt> Rooms { set; get; } = new List<RectInt>();

        public CustomGrid<Object.Cell> Cells { get; } = new CustomGrid<Object.Cell>();

        public CustomGrid<Object.Monster> Monsters { get; } = new CustomGrid<Object.Monster>();

        public RectInt RandomRoom
        {
            get { return Rooms[random.Next(Rooms.Count)]; }
        }

        public bool IsEmpty
        {
            get { return Cells.IsEmpty; }
        }

        public int MonsterCount
        {
            get { return Monsters.Count; }
        }

        public void AddRoom(RectInt room)
        {
            Rooms.Add(room);
        }

        public bool AddCell(Object.Cell cell)
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

        public Object.Cell GetCellAt(int x, int y)
        {
            return Cells.GetAt(x, y);
        }

        public List<Object.Cell> FindCellsInRange(Vector2Int origin, int range)
        {
            var result = new List<Object.Cell>();
            for (int i = origin.x - range; i <= origin.x + range; ++i)
            {
                for (int j = origin.y - range; j <= origin.y + range; ++j)
                {
                    var cell = GetCellAt(i, j);
                    if (cell != null)
                    {
                        result.Add(cell);
                    }
                }
            }
            return result;
        }

        public bool AddMonster(Object.Monster monster)
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

        public Object.Monster GetMonsterAt(int x, int y)
        {
            return Monsters.GetAt(x, y);
        }

        public void RemoveMonster(Object.Monster monster)
        {
            Monsters.Remove(monster.Location.x, monster.Location.y, monster);
            Destroy(monster.gameObject);
            Destroy(monster);
        }

        public bool MoveMonster(Object.Monster monster, Vector2Int to)
        {
            var cell = GetCellAt(to.x, to.y);
            if (cell == null)
            {
                return false;
            }

            var other = GetMonsterAt(to.x, to.y);
            if (other != null)
            {
                return false;
            }

            Monsters.Remove(monster.Location.x, monster.Location.y);
            Monsters.AddExisting(to.x, to.y, monster);
            monster.Location = to;
            return true;
        }
    }
}
