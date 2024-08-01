using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TrapsManager : MonoBehaviour
{
    #region Singleton
    public static TrapsManager _instance;
    public static TrapsManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
            Destroy(_instance);
        else
            _instance = this;
    }
    #endregion Singleton

    public TrapData trapData;

    public Trampoline _prefabTrampoline;
    public Block _prefabBlock;
    public Fan _prefabFan;

    private Dictionary<int, Queue<Trap>> traps;

    public virtual TrapData GetTrapData() { return trapData; }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        traps = new Dictionary<int, Queue<Trap>>
        {
            { trapData.trampolineID, new Queue<Trap>() },
            { trapData.blockID, new Queue<Trap>() },
            { trapData.fanID, new Queue<Trap>() }
        };

        Trampoline trampoline;
        Block block;
        Fan fan;

        for (int i = 0; i < 2; i++)
        {
            trampoline = Instantiate(_prefabTrampoline);
            trampoline.gameObject.SetActive(false);
            traps[trapData.trampolineID].Enqueue(trampoline);
        }

        for (int i = 0; i < 50; i++)
        {
            block = Instantiate(_prefabBlock);
            block.gameObject.SetActive(false);
            traps[trapData.blockID].Enqueue(block);
        }

        for (int i = 0; i < 4; i++)
        {
            fan = Instantiate(_prefabFan);
            fan.gameObject.SetActive(false);
            traps[trapData.fanID].Enqueue(fan);
        }
    }

    private void CreateMoreTrap(int trapID)
    {
        Block block;

        for (int i = 0; i < 25; i++)
        {
            block = Instantiate(_prefabBlock);
            block.gameObject.SetActive(false);
            traps[trapID].Enqueue(block);
        }
    }

    public Trap GetTrapByID(int trapID)
    {
        Trap trap;
        if (traps.ContainsKey(trapID) && traps[trapID].Count > 0)
        {
            trap = traps[trapID].Dequeue();
            trap.gameObject.SetActive(true);
        }
        else
        {
            Debug.Log("Create more trap has ID:" + trapID);
            CreateMoreTrap(trapID);
            trap = traps[trapID].Dequeue();
            trap.gameObject.SetActive(true);
        }
        return trap;
    }

    public void ReturnTrap(int trapID, Trap trap)
    {
        if (traps.ContainsKey(trapID))
        {
            trap.gameObject.SetActive(false);
            traps[trapID].Enqueue(trap);

            Debug.Log("Return trap has ID: " + trapID);
        }
        else
        {
            Debug.LogError("Invalid trap ID: " + trapID);
        }
    }

    public void Spawn(TrapData data)
    {
        LevelManager level = LevelManager.Instance;

        //Debug.Log(boxes);
        if (data == null)
        {
            Debug.Log("No trap data");
            return;
        }

        if (level == null)
        {
            Debug.Log("No level data");
        }

        Trap trap;

        // Trampoline
        for (int i = 0; i < data.trampolinePosition.Count; i++)
        {
            trap = GetTrapByID(data.trampolineID);
            trap.transform.position = data.trampolinePosition[i];

            level.GetCurrentLevel().AddTrap(trap);
        }

        // Block
        for (int i = 0; i < data.blockPosition.Count; i++)
        {
            trap = GetTrapByID(data.blockID);
            trap.transform.position = data.blockPosition[i];

            level.GetCurrentLevel().AddTrap(trap);
        }

        // Fan
        for (int i = 0; i < data.fanPosition.Count; i++)
        {
            trap = GetTrapByID(data.fanID);
            trap.transform.position = data.fanPosition[i];

            if (i == data.fanPosition.Count - 1)
            {
                trap.transform.rotation = Quaternion.Euler(0, 0, -90);
                trap.SetToggle(true);
            }
            else
            {
                trap.SetToggle(false);
            }

            level.GetCurrentLevel().AddTrap(trap);
        }
    }
}