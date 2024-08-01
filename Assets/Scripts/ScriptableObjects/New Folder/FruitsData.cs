using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitsData", menuName = "ScriptableObjects/ItemData/FruitsData")]
public class FruitsData : ScriptableObject
{
    // Fruits's ID
    public int appleID;
    public int bananaID;
    public int cherryID;
    public int kiwiID;
    public int melonID;
    public int orangeID;
    public int pineappleID;
    public int strawberryID;

    // Fruit's Positions
    public List<Vector3> applePosition;
    public List<Vector3> bananasPosition;
    public List<Vector3> cherryPosition;
    public List<Vector3> kiwiPosition;
    public List<Vector3> melonPosition;
    public List<Vector3> orangePosition;
    public List<Vector3> pineapplePosition;
    public List<Vector3> strawberryPosition;

    public int _gravityScale;
    public bool _isTrigger;
}