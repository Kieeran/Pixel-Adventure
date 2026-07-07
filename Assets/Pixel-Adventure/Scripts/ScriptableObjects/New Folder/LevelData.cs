using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlacedObjectData
{
    public GameObject prefab;
    public Vector2 position;
}

// Data này lưu tất cả dữ liệu trong level (vị trí fruits, boxes, ...)
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public List<PlacedObjectData> placedObjectDatas;
    public Vector3 playerStartPosition;
}