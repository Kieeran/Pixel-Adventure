using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    Dictionary<string, Queue<PlacedObject>> pools;
    Dictionary<string, PlacedObject> prefabs;
    Dictionary<string, Transform> containers;

    readonly List<string> labels = new() { "Fruit", "Box" };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        Init();
    }

    void Init()
    {
        pools = new();
        prefabs = new();

        LoadAllPrefabs(() =>
        {
            InitContainers();
            InitPools();

            InGameManager.Instance.OnFruitPoolsReady?.Invoke();
        });
    }

    void InitContainers()
    {
        containers = new();
        foreach (var key in prefabs.Keys)
        {
            var container = new GameObject($"[Pool] {key}");
            container.transform.SetParent(transform);
            containers[key] = container.transform;
        }
    }

    void InitPools()
    {
        foreach (var key in pools.Keys)
        {
            CreatePrefabs(key);
        }
    }

    void CreatePrefabs(string id, int amount = 4)
    {
        Debug.Log("Create prefabs with id: " + id);
        for (int i = 0; i < amount; i++)
        {
            PlacedObject spawnObj = Instantiate(prefabs[id], containers[id]);
            spawnObj.Id = id;
            spawnObj.gameObject.SetActive(false);
            pools[id].Enqueue(spawnObj);
        }
    }

    public PlacedObject GetByID(string id)
    {
        if (!pools.ContainsKey(id))
        {
            Debug.Log("Unvalid id");
            return null;
        }

        if (pools[id].Count <= 0)
        {
            Debug.Log("Create more object has ID:" + id);
            CreatePrefabs(id);
        }

        PlacedObject obj = pools[id].Dequeue();
        obj.OnSpawn();

        obj.gameObject.SetActive(true);
        obj.transform.SetParent(null);

        return obj;
    }

    public void Return(PlacedObject obj)
    {
        if (pools.ContainsKey(obj.Id))
        {
            // Đã return rồi nhưng vẫn còn giữ tham chiếu
            if (obj.gameObject.activeSelf == false) return;

            obj.OnDespawn();

            obj.gameObject.SetActive(false);
            pools[obj.Id].Enqueue(obj);
            obj.transform.SetParent(containers[obj.Id]);

            Debug.Log("Return object has id: " + obj.Id);
        }
        else
        {
            Debug.Log("Invalid id: " + obj.Id);
        }
    }

    public void Spawn(List<PlacedObjectData> placedObjectDatas)
    {
        foreach (var data in placedObjectDatas)
        {
            if (pools.Keys.Contains(data.addressableKey))
            {
                PlacedObject obj = GetByID(data.addressableKey);
                obj.transform.position = data.position;

                LevelManager.Instance.currentLevel.AddPlacedObject(obj);
            }
            else Debug.Log("pools not contain id: " + data.addressableKey);
        }
    }

    public void Despawn(List<PlacedObject> placedObjects)
    {
        for (int i = 0; i < placedObjects.Count; i++)
        {
            Return(placedObjects[i]);
        }
    }

    void LoadAllPrefabs(Action onComplete)
    {
        int remaining = labels.Count;
        foreach (string label in labels)
        {
            LoadPrefabsByLabel(label, () =>
            {
                remaining--;
                if (remaining <= 0)
                {
                    Debug.Log("Load all prefabs complete!");
                    onComplete?.Invoke();
                }
            });
        }
    }

    void LoadPrefabsByLabel(string label, Action onComplete)
    {
        AddressableHandler.Instance.GetKeysByLabel(label, keys =>
        {
            if (keys == null || keys.Count == 0)
            {
                Debug.Log("Can't get keys with label " + label);
                onComplete?.Invoke();
                return;
            }

            int remaining = keys.Count;
            foreach (var key in keys)
            {
                AddressableHandler.Instance.LoadPrefab(key, prefab =>
                {
                    if (prefab != null)
                    {
                        if (prefab.TryGetComponent<PlacedObject>(out var obj))
                        {
                            prefabs[key] = obj;
                            pools[key] = new Queue<PlacedObject>();
                        }
                        else
                        {
                            Debug.Log("Unvalid prefab");
                        }
                    }

                    remaining--;
                    if (remaining <= 0)
                    {
                        Debug.Log($"Load complete prefabs with label {label}");
                        onComplete?.Invoke();
                    }
                });
            }
        });
    }
}