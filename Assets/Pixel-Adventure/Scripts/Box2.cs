using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box2 : Boxes
{
    private void Start()
    {
        this.SetBoxHP(3);
        this.SetBoxID(BoxesManager.Instance.boxesData.box2ID);
    }

    public override void DoneHitting()
    {
        base.DoneHitting();
        Debug.Log("Box hit have ID: " + boxID);

        Fruits fruit = FruitManager.Instance.GetRandomFruit();
        fruit.transform.position = transform.position;

        if (this.GetBoxHP() <= 0)
        {
            BoxesManager.Instance.ReturnBox(this.GetBoxID(), this);
        }
    }
    //public override void DropFruits(int number)
    //{
    //    base.DropFruits(number);
    //}
}