using System;
using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    public class Level : MonoBehaviour
    {
        private System.Random random = new System.Random();

        public List<RectInt> Rooms { set; get; } = new List<RectInt>();

        public CustomGrid<LevelObject.Cell> Cells { get; } = new CustomGrid<LevelObject.Cell>();

        public CustomGrid<LevelObject.Monster> Monsters { get; } = new CustomGrid<LevelObject.Monster>();

        public RectInt RandomRoom
        {
            get { return Rooms[random.Next(Rooms.Count)]; }
        }

        public LevelObject.NextLevel NextLevel { get; set; }

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

        public bool AddCell(LevelObject.Cell cell)
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

        public LevelObject.Cell GetCellAt(int x, int y)
        {
            return Cells.GetAt(x, y);
        }

        public List<LevelObject.Cell> FindCellsInRange(Vector2Int origin, int range)
        {
            var result = new List<LevelObject.Cell>();
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

        public bool AddMonster(LevelObject.Monster monster)
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

        public LevelObject.Monster GetMonsterAt(int x, int y)
        {
            return Monsters.GetAt(x, y);
        }

        public void RemoveMonster(LevelObject.Monster monster)
        {
            Monsters.Remove(monster.Location.x, monster.Location.y, monster);
            Destroy(monster.gameObject);
            Destroy(monster);
        }

        public bool MoveMonster(LevelObject.Monster monster, Vector2Int to)
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

        public bool MonsterCanMove(Vector2Int to)
        {
            var cell = GetCellAt(to.x, to.y);
            if (cell == null)
            {
                return false;
            }
            var monster = GetMonsterAt(to.x, to.y);
            if (monster != null)
            {
                return false;
            }
            if (to.x == NextLevel.Location.x && to.y == NextLevel.Location.y)
            {
                return false;
            }
            return true;
        }
    }
}
