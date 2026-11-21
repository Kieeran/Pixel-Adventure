using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public HomePage homePage;
    public GameplayPage gameplayPage;

    [SerializeField] BackgroundScroll backgroundScroll;

    List<Sprite> backGroundSprites;

    void Awake()
    {
        homePage.gameObject.SetActive(true);
        gameplayPage.gameObject.SetActive(false);

        EventChannel.Instance.OnStartGameplay += () =>
        {
            homePage.gameObject.SetActive(false);
            gameplayPage.gameObject.SetActive(true);
        };

        EventChannel.Instance.OnBackHome += () =>
        {
            homePage.gameObject.SetActive(true);
            gameplayPage.gameObject.SetActive(false);
        };
    }

    public void Init()
    {
        backGroundSprites = UIManager.Instance.backGroundSprites;
        int randomIndex = Random.Range(0, backGroundSprites.Count - 1);
        backgroundScroll.SetBackgroundSprite(backGroundSprites[randomIndex]);
    }
}
