using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager _instance;
    public static LevelManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }
    #endregion Singleton

    private int currentLevelID;
    public Level currentLevel;
    public List<LevelData> levelData;
    public List<Level> _preFabLevels;

    public Level GetCurrentLevel() { return currentLevel; }
    public int GetCurrentLevelID() { return currentLevelID; }

    public PlayerController _prefabCharacter;
    private PlayerController player;

    private bool IsReadyToLoad = false;
    private bool _LoadNextLevel;
    private bool _LoadPreLevel;

    public void SetIsReadyToLoad() { IsReadyToLoad = true; }
    public void SetLoadNextLevel() { _LoadNextLevel = true; }
    public void SetLoadPreLevel() { _LoadPreLevel = true; }

    //public void SetLoadNextLevel(bool b) { _LoadNextLevel = b; }
    //public void SetLoadPreLevel(bool b) { _LoadPreLevel = b; }
    //public int GetLevelID() { return currentLevelID; }
    //public void SetLevelID(int i) { currentLevelID = i; }

    private void Start()
    {
        currentLevelID = InGameManager.Instance.GetCurrentLevel();
        player = Instantiate(_prefabCharacter, levelData[currentLevelID].playerPosition, Quaternion.identity);
        Invoke(nameof(LateStart), 0.00001f);
    }

    private void Update()
    {
        //Debug.Log(currentLevelID);
        //Debug.Log(IsReadyToLoad);
        if (IsReadyToLoad)
        {
            if (_LoadNextLevel)
            {
                LoadNextLevel();
            }
            else if (_LoadPreLevel)
            {
                LoadPreviousLevel();
            }
            IsReadyToLoad = false;

            if (currentLevelID == 0)
            {
                UIManager.Instance.SetActivePreButton(false);
            }
            else
            {
                UIManager.Instance.SetActivePreButton(true);
            }

            if (currentLevelID == 4)
            {
                UIManager.Instance.SetActiveNextButton(false);
            }
            else
            {
                UIManager.Instance.SetActiveNextButton(true);
            }
        }
    }

    void LateStart()
    {
        LoadLevel(currentLevelID);
    }

    public void LoadNextLevel()
    {
        if (currentLevelID < 4)
        {
            currentLevel.UnloadLevel();
            Destroy(currentLevel.gameObject);
            currentLevel = null;
            currentLevelID = InGameManager.Instance.GetCurrentLevel() + 1;
            InGameManager.Instance.SetCurrentLevel(currentLevelID);
            LoadLevel(currentLevelID);

            Debug.Log(currentLevelID);
        }
        else
        {
            Debug.Log("This is the last scene");
        }

        _LoadNextLevel = false;
        //currentLevel.UnloadLevel();
        //Destroy(currentLevel.gameObject);
        //currentLevel = null;
        //currentLevelID = InGameManager.Instance.GetCurrentLevel() + 1;
        //InGameManager.Instance.SetCurrentLevel(currentLevelID);
        //LoadLevel(currentLevelID);
    }

    public void LoadPreviousLevel()
    {
        if (currentLevelID > 0)
        {
            currentLevel.UnloadLevel();
            Destroy(currentLevel.gameObject);
            currentLevel = null;
            currentLevelID = InGameManager.Instance.GetCurrentLevel() - 1;
            InGameManager.Instance.SetCurrentLevel(currentLevelID);
            LoadLevel(currentLevelID);

            Debug.Log(currentLevelID);
        }
        else
        {
            Debug.Log("There are no scenes behind this scene");
        }

        _LoadPreLevel = false;

        //currentLevel.UnloadLevel();
        //Destroy(currentLevel.gameObject);
        //currentLevel = null;
        //currentLevelID = InGameManager.Instance.GetCurrentLevel() - 1;
        //InGameManager.Instance.SetCurrentLevel(currentLevelID);
        //LoadLevel(currentLevelID);
    }

    private void LoadLevel(int levelID)
    {
        currentLevel = Instantiate(_preFabLevels[levelID]);
        FruitManager.Instance.Spawn(levelData[levelID].fruitsData);
        BoxesManager.Instance.Spawn(levelData[levelID].boxesData);
        TrapsManager.Instance.Spawn(levelData[levelID].trapsData);

        player.transform.position = levelData[levelID].playerPosition;
        player.ChangeRandomSkin();
        //currentLevel.AddPlayer(_prefabCharacter);
        //Debug.Log(levelData[levelID].boxesData);
    }
}