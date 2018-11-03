using System;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public LevelBuilder LevelBuilderPrefab;
    public LevelGenerator LevelGeneratorPrefab;

    public GameObject Player;

    public Monster MonsterPrefab;

    public float MovementSpeed = 3.2f;

    private LevelBuilder levelBuilderInstance;
    private LevelGenerator levelGeneratorInstance;

    private Level.Level level;
    private Vector2Int playerLocation = new Vector2Int(0, 0);
    private Vector3 playerTargetPosition;

    private System.Random random = new System.Random();

    private void Awake()
    {
        levelBuilderInstance = Instantiate(LevelBuilderPrefab);
        levelGeneratorInstance = Instantiate(LevelGeneratorPrefab);
    }

    void Start()
    {
        level = levelGeneratorInstance.GenerateLevel();
        levelBuilderInstance.Build(level);

        FindStartingPosition();

        MovePlayerToLocation(false);

        PlaceMonsters();
    }

    void Update()
    {
        var playerMovement = CheckPlayerMovement();
        if ((playerMovement.x != 0 || playerMovement.y != 0))
        {
            var nextLocation = new Vector2Int(playerLocation.x + playerMovement.x, playerLocation.y + playerMovement.y);
            var nextCell = level.GetCellAt(nextLocation.x, nextLocation.y);

            if (nextCell != null)
            {
                var monster = level.GetMonsterAt(nextLocation.x, nextLocation.y);
                if (monster == null)
                {
                    playerLocation = nextLocation;
                }
                else
                {
                    monster.TakeDamage(1);
                    if (monster.IsDead)
                    {
                        level.RemoveMonster(monster);
                    }
                }
            }

            UpdateMonsters();
        }

        MoveMonstersToPositions();
        MovePlayerToLocation();
    }

    private void MoveMonstersToPositions()
    {
        foreach (var monster in level.Monsters)
        {
            var newPosition = monster.TargetPosition;
            monster.transform.position = Vector3.Lerp(monster.transform.position, newPosition, Time.deltaTime * MovementSpeed);
        }
    }

    private void UpdateMonsters()
    {
        foreach (var monster in level.Monsters)
        {
            var oldLocation = monster.Location;
            var nextLocation = monster.Move(level);
            level.MoveMonster(oldLocation, monster);

            var nextPosition = levelBuilderInstance.LevelLocationToWorldPosition(nextLocation);
            monster.TargetPosition = nextPosition;
        }
    }

    private void PlaceMonsters()
    {
        foreach (var room in level.Rooms)
        {
            var randomX = random.Next(room.xMin + 1, room.xMax);
            var randomY = random.Next(room.yMin + 1, room.yMax);
            var location = new Vector2Int(randomX, randomY);
            var position = levelBuilderInstance.LevelLocationToWorldPosition(location);
            var monster = Instantiate<Monster>(MonsterPrefab, position, transform.rotation);
            monster.TargetPosition = position;
            monster.Location = location;
            level.AddMonster(monster);
        }
    }

    private void FindStartingPosition()
    {
        var randomRoom = level.RandomRoom;
        playerLocation.x = random.Next(randomRoom.xMin + 1, randomRoom.xMax);
        playerLocation.y = random.Next(randomRoom.yMin + 1, randomRoom.yMax);
    }

    private Vector2Int CheckPlayerMovement()
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

        return new Vector2Int(0, 0);
    }

    private void MovePlayerToLocation(bool smooth = true)
    {
        var newPosition = levelBuilderInstance.LevelLocationToWorldPosition(playerLocation);
        if (smooth)
        {
            Player.transform.position = Vector3.Lerp(Player.transform.position, newPosition, Time.deltaTime * MovementSpeed);
        }
        else
        {
            Player.transform.position = newPosition;
        }
    }
}
