using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int Width;
    public int Height;
    public int MinRoomSize = 2;
    public int MaxRoomSize = 8;
    public int MaxRoomDisplacement = 5;
    public LevelObject.Cell CellPrefab;
    public GameObject BackgroundPrefab;

    private Vector2Int xBounds;
    private Vector2Int yBounds;

    private System.Random random = new System.Random();

    private float cellWidth;
    private float cellHeight;

    void Awake()
    {
        xBounds = new Vector2Int(-Width / 2, Width / 2);
        yBounds = new Vector2Int(-Height / 2, Height / 2);

        Vector3 size = CellPrefab.GetComponent<Renderer>().bounds.size;
        cellWidth = size.x;
        cellHeight = size.z;
    }

    void Update()
    {
    }

    public Vector3 LocationToPosition(Vector2Int location)
    {
        return new Vector3
            (
            transform.position.x + location.x * cellWidth,
            1.0f,
            transform.position.z + location.y * cellHeight
            );
    }

    public Level.Level GenerateLevel()
    {
        Level.Level result = gameObject.AddComponent<Level.Level>();

        var rooms = GenerateRooms();
        result.Rooms = rooms;

        foreach (var room in rooms)
        {
            PlaceRoom(result, room);
        }

        ConnectRooms(result, rooms);

        CreateBackground(result);

        return result;
    }

    private void CreateBackground(Level.Level level)
    {
        GameObject background = Instantiate(BackgroundPrefab);
        var dx = ((level.Cells.XMax + level.Cells.XMin) / 2) * cellWidth;
        var dy = ((level.Cells.YMax + level.Cells.YMin) / 2) * cellHeight;
        background.transform.position += new Vector3(dx, -1f, dy);

        var levelWidth = level.Cells.XMax - level.Cells.XMin;
        var levelHeight = level.Cells.YMax - level.Cells.YMin;
        var backgroundSize = background.GetComponent<Renderer>().bounds.size;
        var sx = levelWidth * cellWidth / backgroundSize.x * 2;
        var sy = levelHeight * cellHeight / backgroundSize.z * 2;
        background.transform.localScale = new Vector3(sx, 1.0f, sy);
    }

    private void ConnectRooms(Level.Level level, List<RectInt> rooms)
    {
        if (rooms.Count == 0)
        {
            return;
        }

        List<RectInt> disconnectedRooms = new List<RectInt>();
        disconnectedRooms.AddRange(rooms);

        List<RectInt> connectedRooms = new List<RectInt>();
        connectedRooms.Add(rooms[random.Next(rooms.Count)]);

        while (disconnectedRooms.Count > 0)
        {
            var disconnectedIndex = random.Next(disconnectedRooms.Count);
            var connectedIndex = random.Next(connectedRooms.Count);

            var disconnectedRoom = disconnectedRooms[disconnectedIndex];
            var connectedRoom = connectedRooms[connectedIndex];

            ConnectWithPassage(level, disconnectedRoom, connectedRoom);

            disconnectedRooms.RemoveAt(disconnectedIndex);
            connectedRooms.Add(disconnectedRoom);
        }
    }

    private List<RectInt> GenerateRooms()
    {
        var result = new List<RectInt>();

        GenerateRoomsRec(new Vector2Int(0, 0), result);

        return result;
    }

    private void GenerateRoomsRec(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var room = GenerateRoom(position, generatedRoom);
        result.Add(room);

        GenerateRoomsRecNorth(position, result);
        GenerateRoomsRecNorthEast(position, result);
        GenerateRoomsRecEast(position, result);
        GenerateRoomsRecSouthEast(position, result);
        GenerateRoomsRecSouth(position, result);
        GenerateRoomsRecSouthWest(position, result);
        GenerateRoomsRecWest(position, result);
        GenerateRoomsRecNorthWest(position, result);
    }

    private void GenerateRoomsRecNorth(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x, position.y + roomSpace);

        var room = GenerateRoom(newPosition, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthWest,
            GenerateRoomsRecWest,
            GenerateRoomsRecSouthWest
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private void GenerateRoomsRecNorthEast(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x + roomSpace, position.y + roomSpace);

        var room = GenerateRoom(position, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthEast,
            GenerateRoomsRecEast,
            GenerateRoomsRecSouthEast
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private void GenerateRoomsRecEast(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x + roomSpace, position.y);

        var room = GenerateRoom(newPosition, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthEast,
            GenerateRoomsRecEast,
            GenerateRoomsRecSouthEast
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private void GenerateRoomsRecSouthEast(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x + roomSpace, position.y - roomSpace);

        var room = GenerateRoom(newPosition, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthEast,
            GenerateRoomsRecEast,
            GenerateRoomsRecSouthEast
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private void GenerateRoomsRecSouth(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x, position.y - roomSpace);

        var room = GenerateRoom(newPosition, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthWest,
            GenerateRoomsRecWest,
            GenerateRoomsRecSouthWest
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private void GenerateRoomsRecSouthWest(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x - roomSpace, position.y - roomSpace);

        var room = GenerateRoom(newPosition, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthWest,
            GenerateRoomsRecWest,
            GenerateRoomsRecSouthWest
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private void GenerateRoomsRecWest(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x - roomSpace, position.y);

        var room = GenerateRoom(newPosition, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthWest,
            GenerateRoomsRecWest,
            GenerateRoomsRecSouthWest
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private void GenerateRoomsRecNorthWest(Vector2Int position, List<RectInt> result)
    {
        if (!CheckPosition(position))
        {
            return;
        }

        var generatedRoom = PreGenerateRoom();
        var roomSpace = NextRoomDistance(generatedRoom);
        var newPosition = new Vector2Int(position.x - roomSpace, position.y + roomSpace);

        var room = GenerateRoom(newPosition, generatedRoom);
        result.Add(room);

        var nextRoomsMethods = new List<Action<Vector2Int, List<RectInt>>>()
        {
            GenerateRoomsRecNorthWest,
            GenerateRoomsRecWest,
            GenerateRoomsRecSouthWest
        };

        foreach (var generationMethod in nextRoomsMethods)
        {
            GenerateRoomsInDirection(newPosition, result, generationMethod);
        }
    }

    private int NextRoomDistance(GeneratedRoom generatedRoom)
    {
        return Math.Max(generatedRoom.width, generatedRoom.height) * 2;
    }

    private void GenerateRoomsInDirection
    (
        Vector2Int position,
        List<RectInt> result,
        Action<Vector2Int, List<RectInt>> generationMethod
         )
    {
        if (random.Next(2) == 1)
        {
            generationMethod(position, result);
        }
    }

    private bool CheckPosition(Vector2Int position)
    {
        if (
            position.x < xBounds.x || position.x > xBounds.y ||
            position.y < yBounds.x || position.y > yBounds.y
            )
        {
            return false;
        }
        return true;
    }

    private RectInt GenerateRoom(Vector2Int position, GeneratedRoom from)
    {
        return new RectInt
        (
            position.x + from.xDisplacement - from.width / 2,
            position.y + from.yDisplacement - from.height / 2,
            from.width,
            from.height
            );
    }

    private GeneratedRoom PreGenerateRoom()
    {
        return new GeneratedRoom(
            random.Next(MinRoomSize, MaxRoomSize),
            random.Next(MinRoomSize, MaxRoomSize),
            random.Next(-MaxRoomDisplacement, MaxRoomDisplacement),
            random.Next(-MaxRoomDisplacement, MaxRoomDisplacement)
        );
    }

    private void ConnectWithPassage(Level.Level level, RectInt room1, RectInt room2)
    {
        int startX = random.Next(room1.xMin + 1, room1.xMax - 1);
        int startY = random.Next(room1.yMin + 1, room1.yMax - 1);
        int endX = random.Next(room2.xMin + 1, room2.xMax - 1);
        int endY = random.Next(room2.yMin + 1, room2.yMax - 1);

        int stepX = startX < endX ? 1 : -1;
        int stepY = startY < endY ? 1 : -1;

        int i = startX;
        int j = startY;
        while (true)
        {
            bool xChanged = false;
            if (Math.Abs(startX - i) < Math.Abs(startX - endX))
            {
                i += stepX;
                xChanged = true;
            }

            bool yChanged = false;
            if (Math.Abs(startY - j) < Math.Abs(startY - endY))
            {
                j += stepY;
                yChanged = true;
            }

            if (xChanged && yChanged)
            {
                CreateCellAt(level, i, j);
                CreateCellAt(level, i - stepX, j);
                CreateCellAt(level, i, j - stepY);
            }
            else if (xChanged || yChanged)
            {
                CreateCellAt(level, i, j);
            }
            else
            {
                break;
            }
        }
    }

    private void PlaceRoom(Level.Level level, RectInt position)
    {
        for (int i = position.xMin + 1; i < position.xMax; ++i)
        {
            for (int j = position.yMin + 1; j < position.yMax; ++j)
            {
                CreateCellAt(level, i, j);
            }
        }
    }

    private void CreateCellAt(Level.Level level, int x, int y)
    {
        var existingCell = level.GetCellAt(x, y);
        if (existingCell != null)
        {
            return;
        }
        Vector3 position = new Vector3
            (
            transform.position.x + x * cellWidth,
            0.0f,
            transform.position.y + y * cellHeight
            );
        LevelObject.Cell cell = Instantiate(CellPrefab, position, transform.rotation);
        foreach (Transform child in cell.transform)
        {
            if (child.CompareTag("Minimap"))
            {
                child.gameObject.SetActive(false);
                break;
            }
        }

        cell.Location = new Vector2Int(x, y);
        level.AddCell(cell);
    }

    private struct GeneratedRoom
    {
        public int width;
        public int height;
        public int xDisplacement;
        public int yDisplacement;

        public GeneratedRoom
        (
            int width,
            int height,
            int xDisplacement,
            int yDisplacement
        )
        {
            this.width = width;
            this.height = height;
            this.xDisplacement = xDisplacement;
            this.yDisplacement = yDisplacement;
        }
    }
}
