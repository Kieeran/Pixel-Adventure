using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private int currentLevel;
    public GameData gameData;
    public int GetCurrentLevel() { return currentLevel; }
    public void SetCurrentLevel(int i) { currentLevel = i; }

    // Dơ: sẽ quay lại refactor khi hoàn thành full levels
    public Action OnGameReady;
    public Action OnPoolsReady;
    public Action OnLevelsReady;
    bool assetsLoaded = false;
    bool levelsLoaded = false;

    void Start()
    {
        currentLevel = gameData.currentLevel;

        OnPoolsReady += () =>
        {
            assetsLoaded = true;
            CheckGameReady();
        };

        OnLevelsReady += () =>
        {
            levelsLoaded = true;
            CheckGameReady();
        };
    }

    void CheckGameReady()
    {
        if (assetsLoaded && levelsLoaded)
        {
            OnGameReady?.Invoke();
        }
    }
}
