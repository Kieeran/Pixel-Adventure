using System;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public LevelData levelData;
    public Transform placedObjectsHolder;

    List<PlacedObject> placedObjects = new();

    public void AddPlacedObject(PlacedObject placedObject)
    {
        if (placedObject != null)
        {
            placedObjects.Add(placedObject);
            placedObject.transform.SetParent(placedObjectsHolder);
        }
    }

    void OnValidate()
    {
        foreach (Transform tf in transform)
        {
            if (tf.gameObject.CompareTag("PlacedObjectsHolder"))
            {
                placedObjectsHolder = tf;
            }
        }
    }

    public void Load(Action onComplete)
    {
        PoolManager.Instance.Spawn(levelData.placedObjectDatas, onComplete);
    }

    public void Unload()
    {
        PoolManager.Instance.Despawn(placedObjects);
        placedObjects.Clear();
    }
}