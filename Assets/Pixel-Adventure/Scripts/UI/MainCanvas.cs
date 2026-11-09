using System;
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
    [Space(10)]
    [SerializeField] MobileHoldButton moveLeftButton;
    [SerializeField] MobileHoldButton moveRightButton;
    [SerializeField] Button jumpButton;

    public event Action OnJumpButtonPressed;

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
        int randomIndex = UnityEngine.Random.Range(0, backGroundSprites.Count - 1);
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

        switch (PlatformUtil.Current)
        {
            case PlatformType.Web:     /* WebGL trên máy tính */
            case PlatformType.Desktop: /* app Windows */
                moveLeftButton.gameObject.SetActive(false);
                moveRightButton.gameObject.SetActive(false);
                jumpButton.gameObject.SetActive(false);
                break;
            case PlatformType.Mobile:  /* Android */
                moveLeftButton.gameObject.SetActive(true);
                moveRightButton.gameObject.SetActive(true);
                jumpButton.gameObject.SetActive(true);
                break;
        }

        jumpButton.onClick.AddListener(() =>
        {
            OnJumpButtonPressed?.Invoke();
        });

        transition.Init();
    }

    public Vector2 GetMoveButtonSignal()
    {
        float moveX = 0;
        if (moveLeftButton.IsHeld == true && moveRightButton.IsHeld == false) moveX = -1;
        else if (moveLeftButton.IsHeld == false && moveRightButton.IsHeld == true) moveX = 1;

        return new Vector2(moveX, 0);
    }
}
