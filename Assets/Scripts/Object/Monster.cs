using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Object
{
    public class Monster : MonoBehaviour
    {
        private System.Random random = new System.Random();

        private int hp;

        public int MaxHp = 2;
        public float MovementSpeed = 3.2f;

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

        public Vector2Int Move(Level.Level level)
        {
            Vector2Int newLocation;
            Cell cell;
            do
            {
                newLocation = new Vector2Int(Location.x + random.Next(-1, 2), Location.y + random.Next(-1, 2));
                cell = level.GetCellAt(newLocation.x, newLocation.y);
            } while (cell == null);
            Location = newLocation;
            return newLocation;
        }
    }
}
