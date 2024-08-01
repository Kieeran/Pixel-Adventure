using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box3 : Boxes
{
    private void Start()
    {
        this.SetBoxHP(5);
        this.SetBoxID(BoxesManager.Instance.boxesData.box3ID);
    }

    public override void DoneHitting()
    {
        base.DoneHitting();
        Debug.Log("Box hit have ID: " + boxID);

        if (this.GetBoxHP() <= 0)
        {
            BoxesManager.Instance.ReturnBox(this.GetBoxID(), this);

            Fruits fruit1 = FruitManager.Instance.GetRandomFruit();
            Fruits fruit2 = FruitManager.Instance.GetRandomFruit();
            Fruits fruit3 = FruitManager.Instance.GetRandomFruit();
            Fruits fruit4 = FruitManager.Instance.GetRandomFruit();
            Fruits fruit5 = FruitManager.Instance.GetRandomFruit();

            fruit1.transform.position = transform.position;
            fruit2.transform.position = transform.position;
            fruit3.transform.position = transform.position;
            fruit4.transform.position = transform.position;
            fruit5.transform.position = transform.position;
        }
    }
    //public override void DropFruits(int number)
    //{
    //    base.DropFruits(number);
    //}
}