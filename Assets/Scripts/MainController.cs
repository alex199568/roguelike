using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainController : MonoBehaviour
{
    private const string HP_KEY = "hp";
    private const string KILLS_KEY = "kills";
    private const string LEVEL_KEY = "level";

    public LevelGenerator LevelGeneratorPrefab;
    public LevelObject.Monster[] MonsterPrefabs;
    public LevelObject.NextLevel NextLevelPrefab;

    public LevelObject.Player Player;
    public Text KillCountText;
    public Text LevelText;

    public Canvas PauseCanvas;
    public Button ResumeButton;
    public Button QuitButton;

    private GameState gameState;

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
        gameState = GameObject.FindObjectsOfType<GameState>()[0];
        kills = gameState.GetInt(KILLS_KEY, 0);
        Player.Hp = gameState.GetInt(HP_KEY, Player.Hp);

        LevelText.text = $"Level: {gameState.GetInt(LEVEL_KEY, 1)}";

        PauseCanvas.enabled = false;
        ResumeButton.onClick.AddListener(OnResume);
        QuitButton.onClick.AddListener(OnQuit);

        level = levelGeneratorInstance.GenerateLevel();

        PlacePlayer();
        UpdateCellsVisibility();

        PlaceMonsters();

        PlaceNextLevel();
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

                    var nextLevel = level.NextLevel;
                    if (nextLevel.Location.x == nextLocation.x && nextLevel.Location.y == nextLocation.y)
                    {
                        LoadNextLevel();
                    }
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

    private void PlaceNextLevel()
    {
        var room = level.RandomRoom;
        while (true)
        {
            var location = new Vector2Int(random.Next(room.xMin + 1, room.xMax), random.Next(room.yMin + 1, room.yMax));
            if (level.GetMonsterAt(location.x, location.y) == null)
            {
                LevelObject.NextLevel nextLevel = Instantiate(NextLevelPrefab, levelGeneratorInstance.LocationToPosition(location), transform.rotation);
                nextLevel.Location = location;
                level.NextLevel = nextLevel;
                break;
            }
        }
    }

    private void LoadNextLevel()
    {
        gameState.SetInt(HP_KEY, Player.Hp);
        gameState.SetInt(KILLS_KEY, kills);
        gameState.SetInt(LEVEL_KEY, gameState.GetInt(LEVEL_KEY, 1) + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
                    Player.TakeDamage(monster.Attack);
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
            var randomMonster = MonsterPrefabs[random.Next(MonsterPrefabs.Length)];
            LevelObject.Monster monster = Instantiate(randomMonster, position, transform.rotation);
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
        GameState.Reset();
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
