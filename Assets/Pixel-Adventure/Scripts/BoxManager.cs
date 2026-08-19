using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BoxManager : MonoBehaviour
{
    public static BoxManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    Dictionary<string, Queue<Box>> boxPools;
    Dictionary<string, Box> prefabDict;

    [SerializeField] Transform poolContainer;
    [SerializeField] int amount = 4;

    static readonly string LABEL = "Box";

    void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        boxPools = new();
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
                        if (prefab.TryGetComponent<Box>(out var box))
                        {
                            prefabDict[key] = box;
                            boxPools[key] = new Queue<Box>();
                            CreateBoxes(key);
                        }

                        else
                        {
                            Debug.Log("Unvalid box prefab");
                        }
                    }

                    remaining--;
                    if (remaining <= 0)
                    {
                        Debug.Log("Boxes pool is ready!");
                        // Chổ này nên xem xét lại, nên chừa một khoảng thời gian nhỏ để các pool init xong rồi mới vào game
                        // như kiểu loading screen vậy
                        // InGameManager.Instance.OnFruitPoolsReady?.Invoke();
                    }
                });
            }
        });
    }

    public Box GetBoxByID(string boxID)
    {
        if (!boxPools.ContainsKey(boxID))
        {
            Debug.Log("Unvalid boxID");
            return null;
        }

        if (boxPools[boxID].Count <= 0)
        {
            Debug.Log("Create more box has ID:" + boxID);
            CreateBoxes(boxID);
        }

        Box box = boxPools[boxID].Dequeue();
        box.gameObject.SetActive(true);
        box.transform.SetParent(null);
        return box;
    }

    void CreateBoxes(string boxID)
    {
        Debug.Log("Create box with id: " + boxID);
        for (int i = 0; i < amount; i++)
        {
            Box spawnBox = Instantiate(prefabDict[boxID], poolContainer);
            spawnBox.Id = boxID;
            spawnBox.gameObject.SetActive(false);
            boxPools[boxID].Enqueue(spawnBox);
        }
    }

    public void ReturnBox(Box box)
    {
        string boxID = box.Id;
        if (boxPools.ContainsKey(boxID))
        {
            box.gameObject.SetActive(false);
            boxPools[boxID].Enqueue(box);
            box.transform.SetParent(poolContainer);

            Debug.Log("Return box has ID: " + boxID);
        }
        else
        {
            Debug.LogError("Invalid box ID: " + boxID);
        }
    }

    public void Spawn(List<PlacedObjectData> placedObjectDatas)
    {
        foreach (var data in placedObjectDatas)
        {
            if (prefabDict.Keys.Contains(data.addressableKey))
            {
                Box box = GetBoxByID(data.addressableKey);
                box.transform.position = data.position;

                LevelManager.Instance.currentLevel.AddPlacedObject(box);
            }
            else Debug.Log("prefabDict.Keys not contain id: " + data.addressableKey);
        }
    }
}