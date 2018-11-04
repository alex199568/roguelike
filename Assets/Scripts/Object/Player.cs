using UnityEngine;
using System.Collections;

namespace Object
{
    public class Player : MonoBehaviour
    {
        public Vector2Int Location { get; set; } = new Vector2Int(0, 0);

        public float MovementSpeed = 3.2f;
        public int InitialHp = 10;

        private int hp;

        public Vector3 TargetPosition { get; set; }

        void Awake()
        {
            hp = InitialHp;
        }

        public void TakeDamage(int damage)
        {
            hp -= damage;
            Debug.Log($"Player took damage {damage}, hp is {hp}");
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * MovementSpeed);
        }

        public Vector2Int? CheckMovement()
        {
            if (Input.GetKeyUp("w"))
            {
                return new Vector2Int(0, 1);
            }

            if (Input.GetKeyUp("a"))
            {
                return new Vector2Int(-1, 0);
            }

            if (Input.GetKeyUp("s"))
            {
                return new Vector2Int(0, -1);
            }

            if (Input.GetKeyUp("d"))
            {
                return new Vector2Int(1, 0);
            }

            if (Input.GetKeyUp("q"))
            {
                return new Vector2Int(-1, 1);
            }

            if (Input.GetKeyUp("e"))
            {
                return new Vector2Int(1, 1);
            }

            if (Input.GetKeyUp("z"))
            {
                return new Vector2Int(-1, -1);
            }

            if (Input.GetKeyUp("c"))
            {
                return new Vector2Int(1, -1);
            }

            if (Input.GetKeyUp("space"))
            {
                return new Vector2Int(0, 0);
            }

            return null;
        }
    }
}
