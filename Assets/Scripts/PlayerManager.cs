using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager _instance;
    public static PlayerManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }
    #endregion Singleton

    //public PlayerController _prefabCharacter;

    //private Vector3 spawnPosition;

    private void Start()
    {
        //spawnPosition = new Vector3(0, 2, 0);
        //Instantiate(_prefabCharacter, spawnPosition, Quaternion.identity);
    }
}