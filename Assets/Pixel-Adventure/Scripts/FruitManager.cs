using System.Collections.Generic;
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

    public FruitsData fruitData;
    private Dictionary<FruitID, Queue<Fruit>> fruitPools;
    [SerializeField] int amount = 4;

    #region Pooling
    [SerializeField] List<Fruit> prefabList;
    Dictionary<FruitID, Fruit> prefabDict;

    [SerializeField] Transform poolContainer;

    void Start()
    {
        InitializePools();
    }

    void InitializePools()
    {
        fruitPools = new();
        prefabDict = new();

        if (prefabList.Count == 0 || prefabList == null)
        {
            Debug.Log("Fruit prefabList is null or empty!");
        }

        foreach (Fruit fruit in prefabList)
        {
            prefabDict.Add(fruit.GetFruitID(), fruit);
            fruitPools.Add(fruit.GetFruitID(), new Queue<Fruit>());

            CreateFruits(fruit.GetFruitID());
        }
    }

    public Fruit GetFruitByID(FruitID fruitID)
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
        return fruit;
    }

    void CreateFruits(FruitID fruitID)
    {
        for (int i = 0; i < amount; i++)
        {
            Fruit spawnFruit = Instantiate(prefabDict[fruitID], poolContainer);
            spawnFruit.gameObject.SetActive(false);
            fruitPools[fruitID].Enqueue(spawnFruit);
        }
    }

    public void ReturnFruit(FruitID fruitID, Fruit fruit)
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
        FruitID randomFruitID = prefabList[Random.Range(0, prefabList.Count)].GetFruitID();
        return GetFruitByID(randomFruitID);
    }

    public int GetRandomPower()
    {
        List<int> power = new() { -1, 1 };
        return power[Random.Range(0, 2)];
    }

    public void Spawn(FruitsData data)
    {
        // LevelManager level = LevelManager.Instance;

        // //Debug.Log(data);
        // if (data == null)
        // {
        //     Debug.Log("No fruit data");
        //     return;
        // }

        // if (level == null)
        // {
        //     Debug.Log("No level data");
        // }

        // Fruit fruit;

        // // Apple
        // for (int i = 0; i < data.applePosition.Count; i++)
        // {
        //     fruit = GetFruitByID(data.appleID);
        //     fruit.transform.position = data.applePosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        // // Cherry
        // for (int i = 0; i < data.cherryPosition.Count; i++)
        // {
        //     fruit = GetFruitByID(data.cherryID);
        //     fruit.transform.position = data.cherryPosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        // // Bananas
        // for (int i = 0; i < data.bananasPosition.Count; i++)
        // {
        //     fruit = GetFruitByID(data.bananaID);
        //     fruit.transform.position = data.bananasPosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        // // Kiwi
        // for (int i = 0; i < data.kiwiPosition.Count; i++)
        // {
        //     fruit = GetFruitByID(data.kiwiID);
        //     fruit.transform.position = data.kiwiPosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        // // Melon
        // for (int i = 0; i < data.melonPosition.Count; i++)
        // {
        //     fruit = GetFruitByID(data.melonID);
        //     fruit.transform.position = data.melonPosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        // // Orange
        // for (int i = 0; i < data.orangePosition.Count; i++)
        // {
        //     fruit = GetFruitByID(data.orangeID);
        //     fruit.transform.position = data.orangePosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        // // Pineapple
        // for (int i = 0; i < data.pineapplePosition.Count; i++)
        // {
        //     fruit = (Pineapple)GetFruitByID(data.pineappleID);
        //     fruit.transform.position = data.pineapplePosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        // // Strawberry
        // for (int i = 0; i < data.strawberryPosition.Count; i++)
        // {
        //     fruit = GetFruitByID(data.strawberryID);
        //     fruit.transform.position = data.strawberryPosition[i];
        //     fruit.SetIsTrigger(data._isTrigger);
        //     fruit.SetGravityScale(data._gravityScale);

        //     level.GetCurrentLevel().AddFruit(fruit);
        // }

        //// Apple
        //for (int i = 0; i < data.applePosition.Count; i++)
        //{
        //    Apple apple = (Apple)GetFruitByID(data.appleID);
        //    apple.transform.position = data.applePosition[i];
        //    apple.SetIsTrigger(data._isTrigger);
        //    apple.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(apple);
        //}

        //// Cherry
        //for (int i = 0; i < data.cherryPosition.Count; i++)
        //{
        //    Cherry cherry = (Cherry)GetFruitByID(data.cherryID);
        //    cherry.transform.position = data.cherryPosition[i];
        //    cherry.SetIsTrigger(data._isTrigger);
        //    cherry.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(cherry);
        //}

        //// Bananas
        //for (int i = 0; i < data.bananasPosition.Count; i++)
        //{
        //    Bananas banana = (Bananas)GetFruitByID(data.bananaID);
        //    banana.transform.position = data.bananasPosition[i];
        //    banana.SetIsTrigger(data._isTrigger);
        //    banana.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(banana);
        //}

        //// Kiwi
        //for (int i = 0; i < data.kiwiPosition.Count; i++)
        //{
        //    Kiwi kiwi = (Kiwi)GetFruitByID(data.kiwiID);
        //    kiwi.transform.position = data.kiwiPosition[i];
        //    kiwi.SetIsTrigger(data._isTrigger);
        //    kiwi.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(kiwi);
        //}

        //// Melon
        //for (int i = 0; i < data.melonPosition.Count; i++)
        //{
        //    Melon melon = (Melon)GetFruitByID(data.melonID);
        //    melon.transform.position = data.melonPosition[i];
        //    melon.SetIsTrigger(data._isTrigger);
        //    melon.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(melon);
        //}

        //// Orange
        //for (int i = 0; i < data.orangePosition.Count; i++)
        //{
        //    Orange orange = (Orange)GetFruitByID(data.orangeID);
        //    orange.transform.position = data.orangePosition[i];
        //    orange.SetIsTrigger(data._isTrigger);
        //    orange.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(orange);
        //}

        //// Pineapple
        //for (int i = 0; i < data.pineapplePosition.Count; i++)
        //{
        //    Pineapple pineapple = (Pineapple)GetFruitByID(data.pineappleID);
        //    pineapple.transform.position = data.pineapplePosition[i];
        //    pineapple.SetIsTrigger(data._isTrigger);
        //    pineapple.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(pineapple);
        //}

        //// Strawberry
        //for (int i = 0; i < data.strawberryPosition.Count; i++)
        //{
        //    Strawberry strawberry = (Strawberry)GetFruitByID(data.strawberryID);
        //    strawberry.transform.position = data.strawberryPosition[i];
        //    strawberry.SetIsTrigger(data._isTrigger);
        //    strawberry.SetGravityScale(data._gravityScale);

        //    level.GetCurrentLevel().AddFruit(strawberry);
        //}
    }
}