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

    public void LoadLevel()
    {
        FruitManager.Instance.Spawn(levelData.placedObjectDatas);
        // BoxesManager.Instance.Spawn(levelData.boxesData);
        // TrapsManager.Instance.Spawn(levelData.trapsData);
    }

    public void UnloadLevel()
    {
        for (int i = 0; i < placedObjects.Count; i++)
        {
            placedObjects[i].UnloadObject();
        }
        placedObjects.Clear();
    }
}