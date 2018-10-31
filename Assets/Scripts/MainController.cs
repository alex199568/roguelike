using UnityEngine;

public class MainController : MonoBehaviour
{
    public LevelBuilder LevelBuilderPrefab;
    public LevelGenerator LevelGeneratorPrefab;

    public GameObject Player;

    public float PlayerMovementSpeed = 3.2f;

    private LevelBuilder levelBuilderInstance;
    private LevelGenerator levelGeneratorInstance;

    private Level level;
    private Vector2Int playerLocation = new Vector2Int(0, 0);
    private Vector3 playerTargetPosition;

    private void Awake()
    {
        levelBuilderInstance = Instantiate(LevelBuilderPrefab);
        levelGeneratorInstance = Instantiate(LevelGeneratorPrefab);
    }

    void Start ()
    {
        level = levelGeneratorInstance.GenerateLevel();
        levelBuilderInstance.Build(level);

        FindStartingPosition();

        MovePlayerToLocation(false);
	}
	
	void Update ()
    {
        var playerMovement = CheckPlayerMovement();
        if ((playerMovement.x != 0 || playerMovement.y != 0) && CheckNextMove(playerMovement))
        {
            playerLocation.x += playerMovement.x;
            playerLocation.y += playerMovement.y;
        }

        MovePlayerToLocation();
    }

    // TODO: come up with a proper way to find a starting location
    private void FindStartingPosition()
    {
        while (level.GetCellAt(playerLocation.x, playerLocation.y) == null)
        {
            playerLocation.y += 1;
        }
    }

    private bool CheckNextMove(Vector2Int move)
    {
        return level.GetCellAt(playerLocation.x + move.x, playerLocation.y + move.y) != null;
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
            Player.transform.position = Vector3.Lerp(Player.transform.position, newPosition, Time.deltaTime * PlayerMovementSpeed);
        }
        else
        {
            Player.transform.position = newPosition;
        }
    }
}
