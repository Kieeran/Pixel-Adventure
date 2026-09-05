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

    public Action OnFruitPoolsReady;

    private void Start()
    {
        currentLevel = gameData.currentLevel;
    }
}
