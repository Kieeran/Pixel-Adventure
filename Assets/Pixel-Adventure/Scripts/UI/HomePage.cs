using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum HomePageScreen { Main, Skins, Levels, Settings }

public class HomePage : MonoBehaviour
{
    [SerializeField] Transform mainScreen;
    [SerializeField] Transform skinsScreen;
    [SerializeField] Transform levelsScreen;
    [SerializeField] Transform settingsScreen;
    [Space(10)]
    [SerializeField] Button levelsButton;
    [SerializeField] Button skinsButton;
    [SerializeField] Button settingsButton;
    [Space(10)]
    [SerializeField] Button levelsBackButton;
    [SerializeField] Button skinsBackButton;
    [SerializeField] Button settingsBackButton;
    [Space(10)]
    [SerializeField] Transform homePlayer;
    [Space(10)]
    [SerializeField] Button playButton;

    void Awake()
    {
        levelsButton.onClick.AddListener(() => NavigatetoScreen(HomePageScreen.Levels));
        skinsButton.onClick.AddListener(() => NavigatetoScreen(HomePageScreen.Skins));
        settingsButton.onClick.AddListener(() => NavigatetoScreen(HomePageScreen.Settings));

        levelsBackButton.onClick.AddListener(() => NavigatetoScreen(HomePageScreen.Main));
        skinsBackButton.onClick.AddListener(() => NavigatetoScreen(HomePageScreen.Main));
        settingsBackButton.onClick.AddListener(() => NavigatetoScreen(HomePageScreen.Main));

        playButton.onClick.AddListener(() =>
        {
            EventChannel.Instance.OnStartGameplay?.Invoke();
        });
    }

    public IEnumerator Init()
    {
        yield break;
    }

    void NavigatetoScreen(HomePageScreen s)
    {
        mainScreen.gameObject.SetActive(s == HomePageScreen.Main);
        skinsScreen.gameObject.SetActive(s == HomePageScreen.Skins);
        levelsScreen.gameObject.SetActive(s == HomePageScreen.Levels);
        settingsScreen.gameObject.SetActive(s == HomePageScreen.Settings);

        homePlayer.gameObject.SetActive(s == HomePageScreen.Main);
    }
}
