using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;

        int randomIndex = Random.Range(0, backGroundSprites.Count - 1);
        mainCanvas.backgroundScroll.SetBackgroundSprite(backGroundSprites[randomIndex]);

        preButton.onClick.AddListener(() =>
        {
            int currentLevel = InGameManager.Instance.GetCurrentLevel() - 1;
            if (currentLevel < 0) return;

            InGameManager.Instance.SetCurrentLevel(currentLevel);

            if (currentLevel == 0)
            {
                preButton.gameObject.SetActive(false);
            }
            else
            {
                nextButton.gameObject.SetActive(true);
            }

            OnPreButtonPressed?.Invoke();
        });

        nextButton.onClick.AddListener(() =>
        {
            int currentLevel = InGameManager.Instance.GetCurrentLevel() + 1;
            if (currentLevel >= LevelManager.Instance._preFabLevels.Count) return;

            InGameManager.Instance.SetCurrentLevel(currentLevel);

            if (currentLevel == LevelManager.Instance._preFabLevels.Count - 1)
            {
                nextButton.gameObject.SetActive(false);
            }
            else
            {
                preButton.gameObject.SetActive(true);
            }

            OnNextButtonPressed?.Invoke();
        });
    }

    public MainCanvas mainCanvas;
    public Button preButton;
    public Button nextButton;

    public event System.Action OnPreButtonPressed;
    public event System.Action OnNextButtonPressed;

    [SerializeField] List<Sprite> backGroundSprites;

    private void Start()
    {
        preButton.gameObject.SetActive(false);
    }

    private void Update()
    {
        //if (LevelManager.Instance.GetCurrentLevelID() == 0)
        //{
        //    preButton.gameObject.SetActive(false);
        //}
        //else
        //{
        //    preButton.gameObject.SetActive(true);
        //}

        //if (LevelManager.Instance.GetCurrentLevelID() == 4)
        //{
        //    nextButton.gameObject.SetActive(false);
        //}
        //else
        //{
        //    nextButton.gameObject.SetActive(true);
        //}
    }
}
