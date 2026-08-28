using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlacedObjectData
{
    public string addressableKey;
    public Vector2 position;
    public float rotation;
}

// Data này lưu tất cả dữ liệu trong level (vị trí fruits, boxes, ...)
[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public List<PlacedObjectData> placedObjectDatas;
    public Vector3 playerStartPosition;
}