using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public string currentLevelID;
    public Level currentLevel;
    public List<Level> _preFabLevels;
    public Dictionary<string, Level> levels;

    public Level GetCurrentLevel() { return currentLevel; }

    public PlayerController _prefabCharacter;
    private PlayerController player;

    private bool IsReadyToLoad = false;
    private bool _LoadNextLevel;
    private bool _LoadPreLevel;

    public void SetIsReadyToLoad() { IsReadyToLoad = true; }
    public void SetLoadNextLevel() { _LoadNextLevel = true; }
    public void SetLoadPreLevel() { _LoadPreLevel = true; }

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
        LoadLevel();
        player = Instantiate(_prefabCharacter, currentLevel.levelData.playerStartPosition, Quaternion.identity);
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