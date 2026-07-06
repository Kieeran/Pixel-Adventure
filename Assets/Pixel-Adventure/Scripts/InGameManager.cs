using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    #region Singleton

    public static InGameManager _instance;
    public static InGameManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }

    #endregion Singleton

    private string currentLevel;
    public GameData gameData;
    public string GetCurrentLevel() { return currentLevel; }
    public void SetCurrentLevel(string i) { currentLevel = i; }

    private void Start()
    {
        currentLevel = gameData.currentLevel;
    }
}
