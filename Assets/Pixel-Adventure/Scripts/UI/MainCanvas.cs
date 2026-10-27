using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    public event System.Action OnChangeLevelButtonPressed;

    [SerializeField] BackgroundScroll backgroundScroll;
    [SerializeField] Transition transition;
    [SerializeField] Button previousLevelButton;
    [SerializeField] Button reloadLevelButton;
    [SerializeField] Button nextLevelButton;

    List<Sprite> backGroundSprites;

    public void Init()
    {
        // Tắt mở hai nút Previous hoặc Next tùy vào level hiện tại lúc mới vào game
        int currentLevel = InGameManager.Instance.GetCurrentLevel();
        if (currentLevel <= 0)
        {
            previousLevelButton.image.enabled = false;
            nextLevelButton.image.enabled = true;
        }
        if (currentLevel >= LevelManager.Instance.preFabLevels.Count - 1)
        {
            previousLevelButton.image.enabled = true;
            nextLevelButton.image.enabled = false;
        }

        backGroundSprites = UIManager.Instance.backGroundSprites;
        int randomIndex = Random.Range(0, backGroundSprites.Count - 1);
        backgroundScroll.SetBackgroundSprite(backGroundSprites[randomIndex]);

        previousLevelButton.onClick.AddListener(() =>
        {
            int currentLevel = InGameManager.Instance.GetCurrentLevel() - 1;
            if (currentLevel < 0) return;

            InGameManager.Instance.SetCurrentLevel(currentLevel);

            if (currentLevel == 0)
            {
                previousLevelButton.image.enabled = false;
            }
            else
            {
                nextLevelButton.image.enabled = true;
            }

            OnChangeLevelButtonPressed?.Invoke();
        });

        nextLevelButton.onClick.AddListener(() =>
        {
            int currentLevel = InGameManager.Instance.GetCurrentLevel() + 1;
            if (currentLevel >= LevelManager.Instance.preFabLevels.Count) return;

            InGameManager.Instance.SetCurrentLevel(currentLevel);

            if (currentLevel == LevelManager.Instance.preFabLevels.Count - 1)
            {
                nextLevelButton.image.enabled = false;
            }
            else
            {
                previousLevelButton.image.enabled = true;
            }

            OnChangeLevelButtonPressed?.Invoke();
        });

        reloadLevelButton.onClick.AddListener(() =>
        {
            OnChangeLevelButtonPressed?.Invoke();
        });

        transition.Init();
    }
}
