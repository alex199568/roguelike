using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private System.Random random = new System.Random();

    private int hp;

    public int MaxHp = 2;

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
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
    }

    public Vector2Int Move(Level level)
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
