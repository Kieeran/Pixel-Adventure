using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public event System.Action OnChangeLevelButtonPressed;

    [SerializeField] BackgroundScroll backgroundScroll;
    [SerializeField] Transition transition;
    [SerializeField] Button preButton;
    [SerializeField] Button nextButton;

    List<Sprite> backGroundSprites;

    public void Init()
    {
        // Tắt mở hai nút Previous hoặc Next tùy vào level hiện tại lúc mới vào game
        int currentLevel = InGameManager.Instance.GetCurrentLevel();
        if (currentLevel <= 0)
        {
            preButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(true);
        }
        if (currentLevel >= LevelManager.Instance.preFabLevels.Count - 1)
        {
            preButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(false);
        }

        backGroundSprites = UIManager.Instance.backGroundSprites;
        int randomIndex = Random.Range(0, backGroundSprites.Count - 1);
        backgroundScroll.SetBackgroundSprite(backGroundSprites[randomIndex]);

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

            OnChangeLevelButtonPressed?.Invoke();
        });

        nextButton.onClick.AddListener(() =>
        {
            int currentLevel = InGameManager.Instance.GetCurrentLevel() + 1;
            if (currentLevel >= LevelManager.Instance.preFabLevels.Count) return;

            InGameManager.Instance.SetCurrentLevel(currentLevel);

            if (currentLevel == LevelManager.Instance.preFabLevels.Count - 1)
            {
                nextButton.gameObject.SetActive(false);
            }
            else
            {
                preButton.gameObject.SetActive(true);
            }

            OnChangeLevelButtonPressed?.Invoke();
        });

        transition.Init();
    }
}
