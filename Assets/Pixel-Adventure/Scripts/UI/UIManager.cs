using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public static UIManager Instance => _instance;

    void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }

    public MainCanvas mainCanvas;
    public List<Sprite> backGroundSprites;

    public IEnumerator Init(MainCanvas mainCanvas)
    {
        this.mainCanvas = mainCanvas;
        this.mainCanvas.Init();

        yield break;
    }
}
