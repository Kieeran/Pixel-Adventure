using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    private float delayTime = 0.1f;
    private float counter = 0f;

    //public Bananas _bananas;
    //public Kiwi kiwi;

    void Update()
    {
        counter += Time.deltaTime;
        if (counter > delayTime)
        {
            Spawn();
            counter = -0.2f;
        }
    }
    private void Spawn()
    {
        int fruit1ID = Random.Range(0, 6);
        int fruit2ID = Random.Range(0, 6);

        Fruits fruit1 = null;
        Fruits fruit2 = null;

        if (FruitManager.Instance != null)
        {
            //fruit1 = FruitManager.Instance.GetFruitByID(0);
            //fruit2 = FruitManager.Instance.GetFruitByID(1);
            fruit1 = FruitManager.Instance.GetFruitByID(fruit1ID);
            fruit2 = FruitManager.Instance.GetFruitByID(fruit2ID);
        }
        else
        {
            Debug.Log("FruitManager instance is null");
        }

        if (fruit1 != null && fruit2 != null)
        {
            fruit1.transform.position = this.transform.position + new Vector3(0.001f, 0, 0);
            fruit2.transform.position = this.transform.position + new Vector3(-0.001f, 0, 0);
        }
        else
        {
            Debug.Log("NULLLLL");
        }
    }

    //private GameObject SpawnFruit(int fruitID)
    //{
    //    GameObject fruit = new();
    //    //switch (fruitID)
    //    //{
    //    //    case 0:
    //    //        fruit = 
    //    //        break;
    //    //    case 1:
    //    //        break;
    //    //    case 2:
    //    //        break;
    //    //    case 3:
    //    //        break;
    //    //}

    //    //FruitsData _data = new();
    //    //if (fruitID == _data.appleID)
    //    //{
    //    //    return fruitManager1.Instance.GetApple();
    //    //}

    //    return fruit;
    //}
}