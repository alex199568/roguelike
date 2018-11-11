using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Object
{
    public class Player : MonoBehaviour
    {
        public Vector2Int Location { get; set; } = new Vector2Int(0, 0);

        public float MovementSpeed = 3.2f;
        public int InitialHp = 10;
        public Text HpText;
        public int VisionRange = 4;

        public Vector3 TargetPosition { get; set; }

        public int Hp { get; set; }

        void Awake()
        {
            Hp = InitialHp;
        }

        public void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        public bool IsDead
        {
            get { return Hp <= 0; }
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(transform.position, TargetPosition, Time.deltaTime * MovementSpeed);
        }

        private void LateUpdate()
        {
            HpText.text = $"HP: {Hp}";
        }

        public Vector2Int? CheckMovement()
        {
            if (Input.GetKeyDown("w"))
            {
                return new Vector2Int(0, 1);
            }

            if (Input.GetKeyDown("a"))
            {
                return new Vector2Int(-1, 0);
            }

            if (Input.GetKeyDown("s"))
            {
                return new Vector2Int(0, -1);
            }

            if (Input.GetKeyDown("d"))
            {
                return new Vector2Int(1, 0);
            }

            if (Input.GetKeyDown("q"))
            {
                return new Vector2Int(-1, 1);
            }

            if (Input.GetKeyDown("e"))
            {
                return new Vector2Int(1, 1);
            }

            if (Input.GetKeyDown("z"))
            {
                return new Vector2Int(-1, -1);
            }

            if (Input.GetKeyDown("c"))
            {
                return new Vector2Int(1, -1);
            }

            if (Input.GetKeyDown("space"))
            {
                return new Vector2Int(0, 0);
            }

            return null;
        }
    }
}
