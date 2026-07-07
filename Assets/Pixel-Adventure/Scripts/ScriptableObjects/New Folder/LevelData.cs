using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


// Data này lưu tất cả dữ liệu trong level (vị trí fruits, boxes, ...)
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
public class LevelData : ScriptableObject
{
    public int levelID;
    public FruitsData fruitsData;
    public BoxesData boxesData;
    public TrapData trapsData;

    public Vector3 playerStartPosition;
}