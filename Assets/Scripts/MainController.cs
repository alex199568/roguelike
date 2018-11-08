﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    public LevelGenerator LevelGeneratorPrefab;
    public Object.Monster MonsterPrefab;

    public Object.Player Player;
    public Text KillCountText;

    public Canvas PauseCanvas;
    public Button ResumeButton;
    public Button QuitButton;

    private LevelGenerator levelGeneratorInstance;

    private Level.Level level;

    private System.Random random = new System.Random();

    private int kills = 0;
    private bool isPaused = false;

    private void Awake()
    {
        levelGeneratorInstance = Instantiate(LevelGeneratorPrefab);
    }

    void Start()
    {
        PauseCanvas.enabled = false;
        ResumeButton.onClick.AddListener(OnResume);
        QuitButton.onClick.AddListener(OnQuit);

        level = levelGeneratorInstance.GenerateLevel();

        PlacePlayer();
        UpdateCellsVisibility();

        PlaceMonsters();
    }

    void Update()
    {
        CheckEscape();

        if (isPaused)
        {
            return;
        }

        var playerMovement = Player.CheckMovement();
        if (playerMovement != null)
        {
            var movement = (Vector2Int)playerMovement;
            var nextLocation = new Vector2Int(Player.Location.x + movement.x, Player.Location.y + movement.y);
            var nextCell = level.GetCellAt(nextLocation.x, nextLocation.y);

            if (nextCell != null)
            {
                var monster = level.GetMonsterAt(nextLocation.x, nextLocation.y);
                if (monster == null)
                {
                    Player.Location = nextLocation;
                    Player.TargetPosition = levelGeneratorInstance.LocationToPosition(Player.Location);
                    UpdateCellsVisibility();
                }
                else
                {
                    monster.TakeDamage(1);
                    if (monster.IsDead)
                    {
                        ++kills;
                        level.RemoveMonster(monster);
                        if (level.MonsterCount == 0)
                        {
                            LoadScene("Win");
                        }
                    }
                }
            }

            UpdateMonsters();
        }
    }

    private void UpdateCellsVisibility()
    {
        var cells = level.FindCellsInRange(Player.Location, Player.VisionRange);
        foreach (var cell in cells)
        {
            foreach (Transform child in cell.transform)
            {
                if (child.CompareTag("Minimap"))
                {
                    child.gameObject.SetActive(true);
                    break;
                }
            }
        }
    }

    private void LateUpdate()
    {
        KillCountText.text = $"Kills {kills}";
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
                if (location.x == Player.Location.x && location.y == Player.Location.y)
                {
                    Player.TakeDamage(1);
                    if (Player.IsDead)
                    {
                        LoadScene("Death");
                    }
                }
                else if (level.MoveMonster(monster, location))
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

    private void CheckEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PauseCanvas.enabled)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    private void Resume()
    {
        isPaused = false;
        PauseCanvas.enabled = false;
        Time.timeScale = 1f;
    }

    private void Pause()
    {
        isPaused = true;
        PauseCanvas.enabled = true;
        Time.timeScale = 0f;
    }

    private void LoadScene(string name)
    {
#if UNITY_EDITOR
        Debug.Log($"Loading {name}");
#else
        SceneManager.LoadScene(name);
#endif
    }

    void OnResume()
    {
        Resume();
    }

    void OnQuit()
    {
        Resume();
#if UNITY_EDITOR
        Debug.Log("Quit");
#else
        SceneManager.LoadScene("MainMenu");
#endif
    }
}
