using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BoxesManager : MonoBehaviour
{
    public static BoxesManager _instance;
    public static BoxesManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }

    public BoxesData boxesData;
    private int amount;

    public Box1 _prefabBox1;
    public Box2 _prefabBox2;
    public Box3 _prefabBox3;

    private Dictionary<int, Queue<Boxes>> boxes;

    private void Start()
    {
        amount = 4;
        InitializePools();
    }

    private void InitializePools()
    {
        boxes = new Dictionary<int, Queue<Boxes>>
        {
            { boxesData.box1ID, new Queue<Boxes>() },
            { boxesData.box2ID, new Queue<Boxes>() },
            { boxesData.box3ID, new Queue<Boxes>() }
        };

        Box1 box1;
        Box2 box2;
        Box3 box3;

        for (int i = 0; i < amount; i++)
        {
            box1 = Instantiate(_prefabBox1);
            box1.gameObject.SetActive(false);
            boxes[boxesData.box1ID].Enqueue(box1);

            box2 = Instantiate(_prefabBox2);
            box2.gameObject.SetActive(false);
            boxes[boxesData.box2ID].Enqueue(box2);

            box3 = Instantiate(_prefabBox3);
            box3.gameObject.SetActive(false);
            boxes[boxesData.box3ID].Enqueue(box3);
        }
    }

    public Boxes GetBoxByID(int boxID)
    {
        //Debug.Log(boxes);
        Boxes box;
        if (boxes.ContainsKey(boxID) && boxes[boxID].Count > 0)
        {
            box = boxes[boxID].Dequeue();
            box.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Create more box has ID:" + boxID);
            CreateMoreBoxes(boxID);
            box = boxes[boxID].Dequeue();
            box.gameObject.SetActive(true);
        }
        return box;
    }

    public void ReturnBox(int boxID, Boxes box)
    {
        if (boxes.ContainsKey(boxID))
        {
            box.gameObject.SetActive(false);
            boxes[boxID].Enqueue(box);

            Debug.Log("Return box has ID: " + boxID);
        }
        else
        {
            Debug.LogError("Invalid box ID: " + boxID);
        }
    }

    private void CreateMoreBoxes(int boxID)
    {
        Boxes _prefab;
        switch (boxID)
        {
            case 0:
                _prefab = _prefabBox1;
                Box1 box1;
                for (int i = 0; i < amount; i++)
                {
                    box1 = (Box1)Instantiate(_prefab);
                    box1.gameObject.SetActive(false);
                    boxes[boxID].Enqueue(box1);
                }
                break;

            case 1:
                _prefab = _prefabBox2;
                Box2 box2;
                for (int i = 0; i < amount; i++)
                {
                    box2 = (Box2)Instantiate(_prefab);
                    box2.gameObject.SetActive(false);
                    boxes[boxID].Enqueue(box2);
                }
                break;

            case 2:
                _prefab = _prefabBox1;
                Box3 box3;
                for (int i = 0; i < amount; i++)
                {
                    box3 = (Box3)Instantiate(_prefab);
                    box3.gameObject.SetActive(false);
                    boxes[boxID].Enqueue(box3);
                }
                break;
        }
    }
    public void Spawn(BoxesData data)
    {
        LevelManager level = LevelManager.Instance;

        //Debug.Log(boxes);
        if (data == null)
        {
            Debug.Log("No box data");
            return;
        }

        if (level == null)
        {
            Debug.Log("No level data");
        }

        Boxes box;

        // Box1
        for (int i = 0; i < data.box1Position.Count; i++)
        {
            box = GetBoxByID(data.box1ID);
            box.transform.position = data.box1Position[i];
            level.GetCurrentLevel().AddBox(box);
        }

        // Box2
        for (int i = 0; i < data.box2Position.Count; i++)
        {
            box = GetBoxByID(data.box2ID);
            box.transform.position = data.box2Position[i];
            level.GetCurrentLevel().AddBox(box);
        }

        // Box3
        for (int i = 0; i < data.box3Position.Count; i++)
        {
            box = GetBoxByID(data.box3ID);
            box.transform.position = data.box3Position[i];
            level.GetCurrentLevel().AddBox(box);
        }
    }
}