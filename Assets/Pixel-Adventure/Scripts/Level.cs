using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    public LevelData levelData;
    public Transform placedObjectsHolder;

    private List<Fruit> fruits = new List<Fruit>();
    private List<Boxes> boxes = new List<Boxes>();
    private List<Trap> traps = new List<Trap>();

    public void AddFruit(Fruit fruit)
    {
        if (fruits != null)
            fruits.Add(fruit);
    }

    public void AddBox(Boxes box)
    {
        if (boxes != null)
            boxes.Add(box);
    }

    public void AddTrap(Trap trap)
    {
        if (traps != null)
            traps.Add(trap);
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
        // FruitManager.Instance.Spawn(levelData.fruitsData);
        // BoxesManager.Instance.Spawn(levelData.boxesData);
        // TrapsManager.Instance.Spawn(levelData.trapsData);
    }

    public void UnloadLevel()
    {
        for (int i = 0; i < fruits.Count; i++)
        {
            FruitManager.Instance.ReturnFruit(fruits[i].GetFruitID(), fruits[i]);
        }

        for (int i = 0; i < boxes.Count; i++)
        {
            BoxesManager.Instance.ReturnBox(boxes[i].GetBoxID(), boxes[i]);
        }

        for (int i = 0; i < traps.Count; i++)
        {
            TrapsManager.Instance.ReturnTrap(traps[i].GetTrapID(), traps[i]);
        }
    }
}