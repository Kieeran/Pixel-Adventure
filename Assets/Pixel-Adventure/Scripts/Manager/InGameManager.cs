using System.Collections;
using UnityEngine;

public enum PlatformType { Web, Desktop, Mobile }

public static class PlatformUtil
{
    public static PlatformType Current
    {
        get
        {
#if UNITY_WEBGL
            return PlatformType.Web;
#elif UNITY_ANDROID
            return PlatformType.Mobile;
#else
            return PlatformType.Desktop;   // Windows (và Editor)
#endif
        }
    }
}

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance { get; private set; }
    [SerializeField] MainCanvas mainCanvas;
    [SerializeField] UIManager uIManager;
    [SerializeField] PoolManager poolManager;
    [SerializeField] LevelManager levelManager;
    [SerializeField] InputManager inputManager;
    [SerializeField] Camera mainCamera;

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
        SetCameraProperties();

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
        while (InputManager.Instance == null)
        {
            Instantiate(inputManager);
            yield return null;
        }

        yield return PoolManager.Instance.Init();

        yield return LevelManager.Instance.SetupLevels();

        yield return UIManager.Instance.Init(mainCanvas);

        yield return InputManager.Instance.Init();

        yield return LevelManager.Instance.Init();

        fanGroupSequencer.Init();

        yield break;
    }

    void SetCameraProperties()
    {
        switch (PlatformUtil.Current)
        {
            case PlatformType.Web:     /* WebGL trên máy tính */
            case PlatformType.Desktop: /* app Windows */
                break;
            case PlatformType.Mobile:  /* Android */
                mainCamera.orthographicSize = 11.5f;
                mainCamera.transform.eulerAngles = new Vector3(11.75f, 0, 0);

                Application.targetFrameRate = 60;
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
                break;
        }
    }
}
