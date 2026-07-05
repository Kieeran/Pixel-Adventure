using System;
using System.Collections.Generic;
using UnityEngine;

// Kiểu dữ liệu custom cho một box
[Serializable]
public struct BoxData
{
    public string boxID;
    public Vector2 boxPosition;
}

// Data này lưu vị trí của các box trong level
[CreateAssetMenu(fileName = "BoxesData", menuName = "ScriptableObjects/ItemData/BoxesData")]
public class BoxesData : ScriptableObject
{
    public List<BoxData> boxDatas;

    // Fruit drop number
    public int fruitDropNum_Box1 = 2;
    public int fruitDropNum_Box2 = 3;
    public int fruitDropNum_Box3 = 5;
}