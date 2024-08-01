using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    public static UIManager _instance;
    public static UIManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }
    #endregion Singleton

    public Button preButton;
    public Button nextButton;

    public void SetActivePreButton(bool b)
    {
        preButton.gameObject.SetActive(b);
    }

    public void SetActiveNextButton(bool b)
    {
        nextButton.gameObject.SetActive(b);
    }

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
