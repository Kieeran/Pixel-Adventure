using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class BackgroundManager : MonoBehaviour
{
    #region Singleton
    public static BackgroundManager _instance;
    public static BackgroundManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }
    #endregion Singleton

    public GameObject BG;
    public Tilemap Tilemap_BG1;
    public Tilemap Tilemap_BG2;
    public List<Tile> tiles;

    //private float speed;
    //private float size;

    //public float GetBackgroundSpeed() { return speed; }
    //public float GetBackgroundSize() { return size; }

    private void Start()
    {
        //speed = 3f;
        //size = 24f;
        ChangeBackground();
    }

    private Tile GetRandomBackground()
    {
        return tiles[Random.Range(0, tiles.Count)];
    }

    public void ChangeBackground()
    {
        Tile newTile = GetRandomBackground();

        for (int y = -3; y < 3; y++)
        {
            for (int x = -4; x < 4; x++)
            {
                Tilemap_BG1.SetTile(new Vector3Int(x, y), newTile);
                Tilemap_BG2.SetTile(new Vector3Int(x, y), newTile);
            }
        }
    }
}