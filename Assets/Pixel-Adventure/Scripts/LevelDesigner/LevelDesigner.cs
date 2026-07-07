using System.Collections.Generic;
using UnityEngine;

public class LevelDesigner : MonoBehaviour
{
    public List<Fruit> prefabFruits;
    public Dictionary<FruitID, Fruit> fruits;
    public Level level;

    [ContextMenu("Save level")]
    public void SaveLevel()
    {
        if (level == null)
        {
            Debug.Log("Level not found");
            return;
        }
        level.levelData.fruitsData.fruitDatas.Clear();

        level.transform.position = Vector2.zero;
        foreach (Transform tf in level.placedObjectsHolder)
        {
            if (tf.TryGetComponent<Fruit>(out var fruit))
            {
                level.levelData.fruitsData.fruitDatas.Add(new FruitData()
                {
                    fruitID = fruit.GetFruitID(),
                    fruitPosition = tf.position,
                    isStatic = true // fix cứng chổ này về sau sửa lại
                });
            }
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
        foreach (FruitData data in level.levelData.fruitsData.fruitDatas)
        {
            Fruit fruit = Instantiate(fruits[data.fruitID]);
            fruit.transform.parent = level.placedObjectsHolder;
            fruit.transform.position = data.fruitPosition;
        }
        Debug.Log("Reopen level level");
    }

    void OnValidate()
    {
        if (prefabFruits == null || prefabFruits.Count == 0)
        {
            Debug.Log("PrefabFruits list is null or empty!");
            return;
        }

        fruits = new();
        foreach (Fruit fruit in prefabFruits)
        {
            fruits[fruit.GetFruitID()] = fruit;
        }
    }
}