using System.Collections;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }
    [SerializeField] MainCanvas mainCanvas;
    [SerializeField] UIManager uIManager;
    [SerializeField] PoolManager poolManager;
    [SerializeField] LevelManager levelManager;

    [Space(10)]
    [SerializeField] FanGroupSequencer fanGroupSequencer;

    [Space(10)]
    public GameData gameData;

    private int currentLevel;
    public int GetCurrentLevel() { return currentLevel; }
    public void SetCurrentLevel(int i) { currentLevel = i; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        while (PoolManager.Instance == null)
        {
            Instantiate(poolManager);
            yield return null;
        }
        while (LevelManager.Instance == null)
        {
            Instantiate(levelManager);
            yield return null;
        }
        while (UIManager.Instance == null)
        {
            Instantiate(uIManager);
            yield return null;
        }

        yield return PoolManager.Instance.Init();

        yield return LevelManager.Instance.SetupLevels();

        yield return LevelManager.Instance.Init();

        yield return UIManager.Instance.Init(mainCanvas);

        fanGroupSequencer.Init();

        yield break;
    }
}
