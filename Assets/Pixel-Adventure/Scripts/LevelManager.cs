using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentLevelID;
    public Level currentLevel;
    public List<Level> _preFabLevels;
    public Dictionary<int, Level> levels;

    public Level GetCurrentLevel() { return currentLevel; }

    public PlayerController _prefabCharacter;
    private PlayerController player;

    public event Action CurrentLevelLoaded;

    public void SetIsReadyToLoad()
    {
        LoadLevel();
    }

    public static LevelManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        SetupLevels();
    }

    void SetupLevels()
    {
        levels = new();
        foreach (var prefab in _preFabLevels)
        {
            levels[prefab.levelData.levelID] = prefab;
        }
    }

    void Start()
    {
        InGameManager.Instance.OnFruitPoolsReady += () =>
        {
            Debug.Log("Start to load level");
            player = Instantiate(_prefabCharacter);

            LoadLevel();
        };
    }

    void LoadLevel()
    {
        if (currentLevel != null)
        {
            currentLevel.Unload();
            Destroy(currentLevel.gameObject);
        }

        currentLevelID = InGameManager.Instance.GetCurrentLevel();
        currentLevel = Instantiate(levels[currentLevelID]);

        player.transform.SetPositionAndRotation(currentLevel.levelData.playerStartPosition, Quaternion.identity);
        currentLevel.Load(() =>
        {
            CurrentLevelLoaded?.Invoke();
        });
    }
}