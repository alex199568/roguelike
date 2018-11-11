using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    private Dictionary<string, int> ints = new Dictionary<string, int>();

    public void Reset()
    {
        ints.Clear();    
    }

    public void SetInt(string key, int value)
    {
        ints[key] = value;
    }

    public int GetInt(string key, int defaultValue)
    {
        try
        {
            return ints[key];
        }
        catch (KeyNotFoundException)
        {
            return defaultValue;
        }
    }

    private void Awake()
    {
        var gameStates = GameObject.FindObjectsOfType<GameState>();
        if (gameStates.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
