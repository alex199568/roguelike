using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{

    public GameObject floor;
    public GameObject wall;
    public int width;
    public int height;

    void Start()
    {
        float cellWidth = floor.GetComponent<Renderer>().bounds.size.x;
        float cellHeight = floor.GetComponent<Renderer>().bounds.size.y;

        float xOffset = width * cellWidth / 2.0f;
        float zOffset = height * cellHeight / 2.0f;

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                GameObject newCell;
                if (Random.value > 0.5f)
                {
                    newCell = floor;
                }
                else
                {
                    newCell = wall;
                }

                Vector3 newPosition = new Vector3(
                    transform.position.x + i * cellWidth - xOffset,
                    0.0f,
                    transform.position.y + j * cellHeight - zOffset
                    );
                Instantiate(newCell, newPosition, transform.rotation);
            }
        }
    }
}
