using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    //public PlayerController _prefabCharacter;

    //private Vector3 spawnPosition;

    private void Start()
    {
        //spawnPosition = new Vector3(0, 2, 0);
        //Instantiate(_prefabCharacter, spawnPosition, Quaternion.identity);
    }
}