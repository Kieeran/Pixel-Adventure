using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameplayPage : MonoBehaviour
{
    [SerializeField] Transition transition;
    [SerializeField] Button previousLevelButton;
    [SerializeField] Button reloadLevelButton;
    [SerializeField] Button nextLevelButton;
    [Space(10)]
    [SerializeField] MobileHoldButton moveLeftButton;
    [SerializeField] MobileHoldButton moveRightButton;
    [SerializeField] Button jumpButton;

    [SerializeField] Transform deadPopup;
    [SerializeField] Button retryButton;
    [SerializeField] Button homeButton;
    [SerializeField] Transform keyHint;

    public event Action OnJumpButtonPressed;

    public IEnumerator Init()
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

            EventChannel.Instance.OnChangeLevel?.Invoke();
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

            EventChannel.Instance.OnChangeLevel?.Invoke();
        });

        reloadLevelButton.onClick.AddListener(() =>
        {
            EventChannel.Instance.OnChangeLevel?.Invoke();
        });

        retryButton.onClick.AddListener(() =>
        {
            EventChannel.Instance.OnChangeLevel?.Invoke();
        });

        homeButton.onClick.AddListener(() =>
        {
            EventChannel.Instance.OnBackHome?.Invoke();
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

        EventChannel.Instance.OnPlayerDisabled += () =>
        {
            deadPopup.gameObject.SetActive(true);
        };
        EventChannel.Instance.OnChangeLevel += () =>
        {
            deadPopup.gameObject.SetActive(false);
        };
        EventChannel.Instance.OnBackHome += () =>
        {
            deadPopup.gameObject.SetActive(false);
        };

        deadPopup.gameObject.SetActive(false);

        if (PlatformUtil.Current == PlatformType.Mobile)
        {
            keyHint.gameObject.SetActive(false);
        }

        yield break;
    }

    public Vector2 GetMoveButtonSignal()
    {
        float moveX = 0;
        if (moveLeftButton.IsHeld == true && moveRightButton.IsHeld == false) moveX = -1;
        else if (moveLeftButton.IsHeld == false && moveRightButton.IsHeld == true) moveX = 1;

        return new Vector2(moveX, 0);
    }
}
