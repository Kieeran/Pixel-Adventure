using System;
using System.Collections.Generic;
using UnityEngine;

// Kiểu dữ liệu custom cho một fruit
[Serializable]
public struct FruitData
{
    public FruitID fruitID;
    public Vector2 fruitPosition;
    public bool isStatic;
}

// Data này lưu vị trí của các fruit trong level
[CreateAssetMenu(fileName = "FruitsData", menuName = "ScriptableObjects/ItemData/FruitsData")]
public class FruitsData : ScriptableObject
{
    public List<FruitData> fruitDatas;

    public int _gravityScale;
    public bool _isTrigger;
}