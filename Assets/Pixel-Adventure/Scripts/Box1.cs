using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box1 : Boxes
{
    private void Start()
    {
        this.SetBoxHP(1);
        this.SetBoxID(BoxesManager.Instance.boxesData.box1ID);
    }
    public override void DoneHitting()
    {
        base.DoneHitting();
        Debug.Log("Box hit have ID: " + boxID);

        BoxesManager.Instance.ReturnBox(this.GetBoxID(), this);

        Fruits fruit1 = FruitManager.Instance.GetRandomFruit();
        Fruits fruit2 = FruitManager.Instance.GetRandomFruit();
        fruit1.transform.position = transform.position;
        fruit2.transform.position = transform.position;
    }

    //public override void DropFruits(int number)
    //{
    //    base.DropFruits(number);
    //}
}