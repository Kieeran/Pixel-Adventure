using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    #region Singleton
    public static FruitManager _instance;
    public static FruitManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }
    #endregion Singleton

    private Dictionary<string, Queue<Fruit>> fruitPools;
    [SerializeField] int amount = 4;

    #region Pooling
    [SerializeField] Transform poolContainer;
    Dictionary<string, Fruit> prefabDict;

    static readonly string LABEL = "Fruit";

    void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        fruitPools = new();
        prefabDict = new();

        AddressableHandler.Instance.GetKeysByLabel(LABEL, keys =>
        {
            if (keys == null || keys.Count == 0)
            {
                Debug.Log("Không lấy được key nào với label " + LABEL);
                return;
            }

            int remaining = keys.Count;
            foreach (var key in keys)
            {
                AddressableHandler.Instance.LoadPrefab(key, prefab =>
                {
                    if (prefab != null)
                    {
                        if (prefab.TryGetComponent<Fruit>(out var fruit))
                        {
                            prefabDict[key] = fruit;
                            fruitPools[key] = new Queue<Fruit>();
                            CreateFruits(key);
                        }

                        else
                        {
                            Debug.Log("Unvalid fruit prefab");
                        }
                    }

                    remaining--;
                    if (remaining <= 0)
                    {
                        Debug.Log("Fruits pool is ready!");
                        InGameManager.Instance.OnFruitPoolsReady?.Invoke();
                    }
                });
            }
        });
    }

    public Fruit GetFruitByID(string fruitID)
    {
        if (!fruitPools.ContainsKey(fruitID))
        {
            Debug.Log("Unvalid fruitID");
            return null;
        }

        if (fruitPools[fruitID].Count <= 0)
        {
            Debug.Log("Create more food has ID:" + fruitID);
            CreateFruits(fruitID);
        }

        Fruit fruit = fruitPools[fruitID].Dequeue();
        fruit.gameObject.SetActive(true);
        fruit.transform.SetParent(null);
        return fruit;
    }

    void CreateFruits(string fruitID)
    {
        Debug.Log("Create fruit with id: " + fruitID);
        for (int i = 0; i < amount; i++)
        {
            Fruit spawnFruit = Instantiate(prefabDict[fruitID], poolContainer);
            spawnFruit.gameObject.SetActive(false);
            fruitPools[fruitID].Enqueue(spawnFruit);
        }
    }

    public void ReturnFruit(string fruitID, Fruit fruit)
    {
        if (fruitPools.ContainsKey(fruitID))
        {
            fruit.SetIsTrigger(false);
            fruit.SetGravityScale(5);
            fruit.gameObject.SetActive(false);
            fruitPools[fruitID].Enqueue(fruit);

            Debug.Log("Return fruit has ID: " + fruitID);
        }
        else
        {
            Debug.LogError("Invalid fruit ID: " + fruitID);
        }
    }

    #endregion Pooling

    public Fruit GetRandomFruit()
    {
        string randomFruitID = prefabDict.Keys.ToList()[Random.Range(0, prefabDict.Keys.Count)];
        return GetFruitByID(randomFruitID);
    }

    public int GetRandomPower()
    {
        List<int> power = new() { -1, 1 };
        return power[Random.Range(0, 2)];
    }

    public void Spawn(List<PlacedObjectData> placedObjectDatas)
    {
        foreach (var data in placedObjectDatas)
        {
            if (prefabDict.Keys.Contains(data.addressableKey))
            {
                Fruit fruit = GetFruitByID(data.addressableKey);
                fruit.transform.position = data.position;
            }
            else Debug.Log("prefabDict.Keys not contain id: " + data.addressableKey);
        }
    }
}