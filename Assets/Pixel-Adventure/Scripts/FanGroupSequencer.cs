using System.Collections.Generic;
using UnityEngine;

public class FanGroupSequencer : MonoBehaviour
{
    void Start()
    {
        LevelManager.Instance.CurrentLevelLoaded += OnCurrentLevelLoaded;
    }

    void OnCurrentLevelLoaded()
    {
        List<PlacedObject> activeObjects = PoolManager.Instance.activeObjects;
    }
}
