using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public LevelBuilder LevelBuilderPrefab;
    public LevelGenerator LevelGeneratorPrefab;

    private LevelBuilder levelBuilderInstance;
    private LevelGenerator levelGeneratorInstance;

    private void Awake()
    {
        levelBuilderInstance = Instantiate(LevelBuilderPrefab);
        levelGeneratorInstance = Instantiate(LevelGeneratorPrefab);
    }

    void Start ()
    {
        var level = levelGeneratorInstance.GenerateLevel();
        levelBuilderInstance.Build(level);
	}
	
	void Update () {
		
	}
}
