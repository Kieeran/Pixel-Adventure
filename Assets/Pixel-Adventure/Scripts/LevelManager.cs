using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public int currentLevelID;
    public Level currentLevel;
    public List<Level> preFabLevels;
    public Dictionary<int, Level> levels;

    public Level GetCurrentLevel() { return currentLevel; }

    public PlayerController _prefabCharacter;
    private PlayerController player;

    public event Action CurrentLevelLoaded;

    public string levelAddressableLabel = "Level";

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
        preFabLevels = new();
        LoadPrefabsByLabel(levelAddressableLabel, () =>
        {
            preFabLevels.Sort((a, b) => a.levelData.levelID.CompareTo(b.levelData.levelID));
            InGameManager.Instance.OnLevelsReady?.Invoke();
        });
    }

    void LoadPrefabsByLabel(string label, Action onComplete)
    {
        Debug.Log("Start to LoadPrefabsByLabel Level");
        AddressableHandler.Instance.GetKeysByLabel(label, keys =>
        {
            if (keys == null || keys.Count == 0)
            {
                Debug.Log("Can't get keys with label " + label);
                onComplete?.Invoke();
                return;
            }

            int remaining = keys.Count;
            foreach (var key in keys)
            {
                AddressableHandler.Instance.LoadPrefab(key, prefab =>
                {
                    if (prefab != null)
                    {
                        if (prefab.TryGetComponent<Level>(out var level))
                        {
                            preFabLevels.Add(level);
                            levels[level.levelData.levelID] = level;
                        }
                        else
                        {
                            Debug.Log("Unvalid prefab");
                        }
                    }

                    remaining--;
                    if (remaining <= 0)
                    {
                        Debug.Log($"Load complete prefabs with label {label}");
                        onComplete?.Invoke();
                    }
                });
            }
        });
    }

    void Start()
    {
        InGameManager.Instance.OnGameReady += () =>
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