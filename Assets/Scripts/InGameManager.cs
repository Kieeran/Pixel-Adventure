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

    private int currentLevel;
    public GameData gameData;
    public int GetCurrentLevel() { return currentLevel; }
    public void SetCurrentLevel(int i) { currentLevel = i; }

    private void Start()
    {
        currentLevel = gameData.currentLevel;
        //Physics.IgnoreLayerCollision(6, 6);
        //Physics.IgnoreLayerCollision(6, 7);
    }
}
