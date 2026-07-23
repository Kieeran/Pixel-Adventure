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

    public void SetIsReadyToLoad()
    {
        LoadLevel();
    }

    public static LevelManager _instance;
    public static LevelManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;

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
            LoadLevel();
            player = Instantiate(_prefabCharacter, currentLevel.levelData.playerStartPosition, Quaternion.identity);
        };
    }

    void LoadLevel()
    {
        if (currentLevel != null)
        {
            currentLevel.UnloadLevel();
            Destroy(currentLevel.gameObject);
        }

        currentLevelID = InGameManager.Instance.GetCurrentLevel();
        currentLevel = Instantiate(levels[currentLevelID]);
        currentLevel.LoadLevel();
    }
}