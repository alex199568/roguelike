using System;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour // TODO: refactor into several controllers
{
    public LevelGenerator LevelGeneratorPrefab;

    public Object.Player Player;

    public Object.Monster MonsterPrefab;

    private LevelGenerator levelGeneratorInstance;

    private Level.Level level;

    private System.Random random = new System.Random();

    private void Awake()
    {
        levelGeneratorInstance = Instantiate(LevelGeneratorPrefab);
    }

    void Start()
    {
        level = levelGeneratorInstance.GenerateLevel();

        PlacePlayer();

        PlaceMonsters();
    }

    void Update()
    {
        var playerMovement = Player.CheckMovement();
        if ((playerMovement.x != 0 || playerMovement.y != 0))
        {
            var nextLocation = new Vector2Int(Player.Location.x + playerMovement.x, Player.Location.y + playerMovement.y);
            var nextCell = level.GetCellAt(nextLocation.x, nextLocation.y);

            if (nextCell != null)
            {
                var monster = level.GetMonsterAt(nextLocation.x, nextLocation.y);
                if (monster == null)
                {
                    Player.Location = nextLocation;
                    Player.TargetPosition = levelGeneratorInstance.LocationToPosition(Player.Location);
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
    }

    private void UpdateMonsters()
    {
        foreach (var monster in level.Monsters)
        {
            var oldLocation = monster.Location;
            var nextLocation = monster.Move(level, Player);
            if (nextLocation != null)
            {
                var location = (Vector2Int)nextLocation;
                if (level.MoveMonster(monster, location))
                {
                    var nextPosition = levelGeneratorInstance.LocationToPosition(location);
                    monster.TargetPosition = nextPosition;
                }
            }
        }
    }

    private void PlaceMonsters()
    {
        foreach (var room in level.Rooms)
        {
            var randomX = random.Next(room.xMin + 1, room.xMax);
            var randomY = random.Next(room.yMin + 1, room.yMax);
            var location = new Vector2Int(randomX, randomY);
            var position = levelGeneratorInstance.LocationToPosition(location);
            Object.Monster monster = Instantiate(MonsterPrefab, position, transform.rotation);
            monster.TargetPosition = position;
            monster.Location = location;
            level.AddMonster(monster);
        }
    }

    private void PlacePlayer()
    {
        var randomRoom = level.RandomRoom;
        Player.Location = new Vector2Int
            (
            random.Next(randomRoom.xMin + 1, randomRoom.xMax),
            random.Next(randomRoom.yMin + 1, randomRoom.yMax)
            );
        var position = levelGeneratorInstance.LocationToPosition(Player.Location);
        Player.transform.position = position;
        Player.TargetPosition = position;
    }
}
