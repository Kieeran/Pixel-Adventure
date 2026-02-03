using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Burst.Intrinsics.Arm;
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
    private Dictionary<int, Queue<Fruits>> fruitPools;
    private int amount;

    #region Pooling
    public Apple _prefabApple;
    public Bananas _prefabBananas;
    public Cherry _prefabCherry;
    public Kiwi _prefabKiwi;
    public Melon _prefabMelon;
    public Orange _prefabOrange;
    public Pineapple _prefabPineapple;
    public Strawberry _prefabStrawberry;
    private void Start()
    {
        amount = 6;
        InitializePools();
    }

    private void InitializePools()
    {
        fruitPools = new Dictionary<int, Queue<Fruits>>
        {
            { fruitData.appleID, new Queue<Fruits>() },
            { fruitData.bananaID, new Queue<Fruits>() },
            { fruitData.cherryID, new Queue<Fruits>() },
            { fruitData.kiwiID, new Queue<Fruits>() },
            { fruitData.melonID, new Queue<Fruits>() },
            { fruitData.orangeID, new Queue<Fruits>() },
            { fruitData.pineappleID, new Queue<Fruits>() },
            { fruitData.strawberryID, new Queue<Fruits>() }
        };

        Apple apple;
        Bananas banana;
        Cherry cherry;
        Kiwi kiwi;
        Melon melon;
        Orange orange;
        Pineapple pineapple;
        Strawberry strawberry;

        for (int i = 0; i < amount; i++)
        {
            apple = Instantiate(_prefabApple);
            apple.gameObject.SetActive(false);
            fruitPools[fruitData.appleID].Enqueue(apple);

            banana = Instantiate(_prefabBananas);
            banana.gameObject.SetActive(false);
            fruitPools[fruitData.bananaID].Enqueue(banana);

            cherry = Instantiate(_prefabCherry);
            cherry.gameObject.SetActive(false);
            fruitPools[fruitData.cherryID].Enqueue(cherry);

            kiwi = Instantiate(_prefabKiwi);
            kiwi.gameObject.SetActive(false);
            fruitPools[fruitData.kiwiID].Enqueue(kiwi);

            melon = Instantiate(_prefabMelon);
            melon.gameObject.SetActive(false);
            fruitPools[fruitData.melonID].Enqueue(melon);

            orange = Instantiate(_prefabOrange);
            orange.gameObject.SetActive(false);
            fruitPools[fruitData.orangeID].Enqueue(orange);

            pineapple = Instantiate(_prefabPineapple);
            pineapple.gameObject.SetActive(false);
            fruitPools[fruitData.pineappleID].Enqueue(pineapple);

            strawberry = Instantiate(_prefabStrawberry);
            strawberry.gameObject.SetActive(false);
            fruitPools[fruitData.strawberryID].Enqueue(strawberry);
        }
    }

    public Fruits GetFruitByID(int fruitID)
    {
        //Debug.Log(fruitPools);
        Fruits fruit;
        if (fruitPools.ContainsKey(fruitID) && fruitPools[fruitID].Count > 0)
        {
            fruit = fruitPools[fruitID].Dequeue();
            fruit.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Create more food has ID:" + fruitID);
            CreateMoreFruits(fruitID);
            fruit = fruitPools[fruitID].Dequeue();
            fruit.gameObject.SetActive(true);
        }
        return fruit;
    }

    private void CreateMoreFruits(int fruitID)
    {
        Fruits _prefab;
        switch (fruitID)
        {
            case 0:
                _prefab = _prefabApple;
                Apple apple;
                for (int i = 0; i < amount; i++)
                {
                    apple = (Apple)Instantiate(_prefab);
                    apple.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(apple);
                }
                break;
            case 1:
                _prefab = _prefabBananas;
                Bananas banana;
                for (int i = 0; i < amount; i++)
                {
                    banana = (Bananas)Instantiate(_prefab);
                    banana.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(banana);
                }
                break;
            case 2:
                _prefab = _prefabCherry;
                Cherry cherry;
                for (int i = 0; i < amount; i++)
                {
                    cherry = (Cherry)Instantiate(_prefab);
                    cherry.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(cherry);
                }
                break;
            case 3:
                _prefab = _prefabKiwi;
                Kiwi kiwi;
                for (int i = 0; i < amount; i++)
                {
                    kiwi = (Kiwi)Instantiate(_prefab);
                    kiwi.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(kiwi);
                }
                break;
            case 4:
                _prefab = _prefabMelon;
                Melon melon;
                for (int i = 0; i < amount; i++)
                {
                    melon = (Melon)Instantiate(_prefab);
                    melon.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(melon);
                }
                break;
            case 5:
                _prefab = _prefabOrange;
                Orange orange;
                for (int i = 0; i < amount; i++)
                {
                    orange = (Orange)Instantiate(_prefab);
                    orange.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(orange);
                }
                break;
            case 6:
                _prefab = _prefabPineapple;
                Pineapple pineapple;
                for (int i = 0; i < amount; i++)
                {
                    pineapple = (Pineapple)Instantiate(_prefab);
                    pineapple.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(pineapple);
                }
                break;
            case 7:
                _prefab = _prefabStrawberry;
                Strawberry strawberry;
                for (int i = 0; i < amount; i++)
                {
                    strawberry = (Strawberry)Instantiate(_prefab);
                    strawberry.gameObject.SetActive(false);
                    fruitPools[fruitID].Enqueue(strawberry);
                }
                break;
        }
    }

    public void ReturnFruit(int fruitID, Fruits fruit)
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

    public Fruits GetRandomFruit()
    {
        List<int> fruitID = new()
        { fruitData.appleID,
        fruitData.bananaID,
        fruitData.cherryID,
        fruitData.kiwiID,
        fruitData.melonID,
        fruitData.orangeID,
        fruitData.pineappleID,
        fruitData.strawberryID};

        return GetFruitByID(fruitID[Random.Range(0, 8)]);
    }

    public int GetRandomPower()
    {
        List<int> power = new() { -1, 1 };
        return power[Random.Range(0, 2)];
    }

    public void Spawn(FruitsData data)
    {
        LevelManager level = LevelManager.Instance;

        //Debug.Log(data);
        if (data == null)
        {
            Debug.Log("No fruit data");
            return;
        }

        if (level == null)
        {
            Debug.Log("No level data");
        }

        Fruits fruit;

        // Apple
        for (int i = 0; i < data.applePosition.Count; i++)
        {
            fruit = GetFruitByID(data.appleID);
            fruit.transform.position = data.applePosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

        // Cherry
        for (int i = 0; i < data.cherryPosition.Count; i++)
        {
            fruit = GetFruitByID(data.cherryID);
            fruit.transform.position = data.cherryPosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

        // Bananas
        for (int i = 0; i < data.bananasPosition.Count; i++)
        {
            fruit = GetFruitByID(data.bananaID);
            fruit.transform.position = data.bananasPosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

        // Kiwi
        for (int i = 0; i < data.kiwiPosition.Count; i++)
        {
            fruit = GetFruitByID(data.kiwiID);
            fruit.transform.position = data.kiwiPosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

        // Melon
        for (int i = 0; i < data.melonPosition.Count; i++)
        {
            fruit = GetFruitByID(data.melonID);
            fruit.transform.position = data.melonPosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

        // Orange
        for (int i = 0; i < data.orangePosition.Count; i++)
        {
            fruit = GetFruitByID(data.orangeID);
            fruit.transform.position = data.orangePosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

        // Pineapple
        for (int i = 0; i < data.pineapplePosition.Count; i++)
        {
            fruit = (Pineapple)GetFruitByID(data.pineappleID);
            fruit.transform.position = data.pineapplePosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

        // Strawberry
        for (int i = 0; i < data.strawberryPosition.Count; i++)
        {
            fruit = GetFruitByID(data.strawberryID);
            fruit.transform.position = data.strawberryPosition[i];
            fruit.SetIsTrigger(data._isTrigger);
            fruit.SetGravityScale(data._gravityScale);

            level.GetCurrentLevel().AddFruit(fruit);
        }

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