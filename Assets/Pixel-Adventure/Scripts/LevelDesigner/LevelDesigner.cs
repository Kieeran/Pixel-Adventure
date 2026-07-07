using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class LevelDesigner : MonoBehaviour
{
    public Level level;

    [ContextMenu("Save level")]
    public void SaveLevel()
    {
        if (level == null)
        {
            Debug.Log("Level not found");
            return;
        }
        level.levelData.placedObjectDatas.Clear();

        level.transform.position = Vector2.zero;
        foreach (Transform tf in level.placedObjectsHolder)
        {
            GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(tf.gameObject);
            level.levelData.placedObjectDatas.Add(new PlacedObjectData
            {
                prefab = prefab,
                position = tf.position
            });
        }
        Debug.Log("Save level complete!");
    }

    [ContextMenu("Edit level")]
    public void EditLevel()
    {
        if (level == null)
        {
            Debug.Log("Level not found");
            return;
        }

        // Clear object cũ trước khi đặt object mới load từ SO lên
        for (int i = level.placedObjectsHolder.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(level.placedObjectsHolder.GetChild(i).gameObject);
        }

        level.transform.position = Vector2.zero;
        foreach (PlacedObjectData data in level.levelData.placedObjectDatas)
        {
            GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(data.prefab);
            obj.transform.parent = level.placedObjectsHolder;
            obj.transform.position = data.position;
        }
        Debug.Log("Reopen level level");
    }
}
#endif