using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    private List<Fruits> fruits = new List<Fruits>();
    private List<Boxes> boxes = new List<Boxes>();
    private List<Trap> traps = new List<Trap>();

    public void AddFruit(Fruits fruit)
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