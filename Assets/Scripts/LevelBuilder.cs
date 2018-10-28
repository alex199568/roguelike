using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public GameObject floor;
    public int width;
    public int height;
    public int MIN_ROOM_SIZE = 2;
    public int MAX_ROOM_SIZE = 8;
    public int MAX_ROOM_DISPLACEMENT = 5;

    private float cellWidth;
    private float cellHeight;

    private Vector2Int xBounds;
    private Vector2Int yBounds;

    private System.Random random = new System.Random();

    void Start()
    {
        Vector3 size = floor.GetComponent<Renderer>().bounds.size;
        cellWidth = size.x;
        cellHeight = size.z;

        xBounds = new Vector2Int(-width / 2, width / 2);
        yBounds = new Vector2Int(-height / 2, height / 2);

        Level level = GenerateLevel();
        BuildLevel(level);
    }

    private void BuildLevel(Level level)
    {
        if (level.IsEmpty)
        {
            return;
        }

        for (int i = level.XMin; i < level.XMax; ++i)
        {
            for (int j = level.YMin; j < level.YMax; ++j)
            {
                Cell cell = level.GetCellAt(i, j);
                if (cell != null)
                {
                    GameObject newCell = cell.CellObject;
                    Vector3 newPosition = new Vector3
                    (
                    transform.position.x + i * cellWidth,
                    0.0f,
                    transform.position.y + j * cellHeight
                    );
                    Instantiate(newCell, newPosition, transform.rotation);
                }
            }
        }
    }

    private Level GenerateLevel()
    {
        Level result = new Level();

        var rooms = GenerateRooms();

        foreach (var room in rooms)
        {
            PlaceRoom(result, room);
        }

        return result;
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
            random.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE),
            random.Next(MIN_ROOM_SIZE, MAX_ROOM_SIZE),
            random.Next(-MAX_ROOM_DISPLACEMENT, MAX_ROOM_DISPLACEMENT),
            random.Next(-MAX_ROOM_DISPLACEMENT, MAX_ROOM_DISPLACEMENT)
        );
    }

    private void ConnectWithPassage(Level level, RectInt room1, RectInt room2)
    {
        Cell floorCell = new Cell(floor);

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
                level.AddCell(i, j, floorCell);
                level.AddCell(i - stepX, j, floorCell);
                level.AddCell(i, j - stepY, floorCell);
            }
            else if (xChanged || yChanged)
            {
                level.AddCell(i, j, floorCell);
            }
            else
            {
                break;
            }
        }
    }

    private void PlaceRoom(Level level, RectInt position)
    {
        Cell floorCell = new Cell(floor);

        for (int i = position.xMin + 1; i < position.xMax; ++i)
        {
            for (int j = position.yMin + 1; j < position.yMax; ++j)
            {
                level.AddCell(i, j, floorCell);
            }
        }
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
