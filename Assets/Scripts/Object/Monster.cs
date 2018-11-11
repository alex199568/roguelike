using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Object
{
    public class Monster : MonoBehaviour
    {
        private System.Random random = new System.Random();

        private int hp;

        public int MaxHp = 2;
        public float MovementSpeed = 3.2f;
        public int PlayerAwarenessDistance = 10;

        public Vector3 TargetPosition { get; set; }

        public Vector2Int Location { get; set; }

        public bool IsDead
        {
            get { return hp <= 0; }
        }

        private void Awake()
        {
            hp = MaxHp;
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * MovementSpeed);
        }

        public void TakeDamage(int damage)
        {
            hp -= damage;
        }

        public Vector2Int? Move(Level.Level level, Object.Player player)
        {
            Vector2Int? direction = null;
            if (CalculateDistance(player.Location) > PlayerAwarenessDistance)
            {
                direction = RandomMove(level);
            }
            else
            {
                direction = MoveTowards(level, player.Location);
                if (direction == null)
                {
                    direction = RandomMove(level);
                }
            }

            if (direction == null)
            {
                return null;
            }

            var dir = (Vector2Int)direction;
            return new Vector2Int(Location.x + dir.x, Location.y + dir.y);
        }

        private Vector2Int? MoveTowards(Level.Level level, Vector2Int to)
        {
            Vector2Int? direction = null;
            if (to.x == Location.x)
            {
                if (to.y < Location.y)
                {
                    direction = new Vector2Int(0, -1);
                }
                else
                {
                    direction = new Vector2Int(0, 1);
                }
            }
            else if (to.y == Location.y)
            {
                if (to.x < Location.x)
                {
                    direction = new Vector2Int(-1, 0);
                }
                else
                {
                    direction = new Vector2Int(1, 0);
                }
            }
            else
            {
                int dx;
                if (to.x < Location.x)
                {
                    dx = -1;
                }
                else
                {
                    dx = 1;
                }

                int dy;
                if (to.y < Location.y)
                {
                    dy = -1;
                }
                else
                {
                    dy = 1;
                }
                direction = new Vector2Int(dx, dy);
            }
            
            if (direction == null)
            {
                return null;
            }

            var dir = (Vector2Int)direction;
            var cell = level.GetCellAt(dir.x + Location.x, dir.y + Location.y);
            var other = level.GetMonsterAt(dir.x + Location.x, dir.y + Location.y);

            if (cell == null || other != null)
            {
                return null;
            }

            return direction;
        }

        private Vector2Int? RandomMove(Level.Level level)
        {
            Cell cell;
            Monster other;
            var tries = 100;

            while (true)
            {
                var direction = new Vector2Int(random.Next(-1, 2), random.Next(-1, 2));
                cell = level.GetCellAt(direction.x + Location.x, direction.y + Location.y);
                other = level.GetMonsterAt(direction.x + Location.x, direction.y + Location.y);

                if (cell != null && other == null)
                {
                    return direction;
                }

                if (tries-- == 0)
                {
                    return null;
                }
            }
        }

        private int CalculateDistance(Vector2Int to)
        {
            int dx;
            if (to.x > Location.x)
            {
                dx = to.x - Location.x;
            }
            else
            {
                dx = Location.x - to.x;
            }

            int dy;
            if (to.y > Location.y)
            {
                dy = to.y - Location.y;
            }
            else
            {
                dy = Location.y - to.y;
            }

            return (int)Math.Sqrt(dx * dx + dy * dy);
        }
    }
}
