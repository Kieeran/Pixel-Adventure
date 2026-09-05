using System;
using UnityEngine;

public class EventChannel : MonoBehaviour
{
    public static EventChannel Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Action<Box> OnBoxBroken;
    public Action<Box> OnBoxHit;
}
