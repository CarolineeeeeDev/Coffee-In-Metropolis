using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OrderFactory
{
    public static RawMaterial BlackCoffee;

    
    public static void InitFactory()
    {
        BlackCoffee = Resources.Load<RawMaterial>("CoffeeMaterial/BlackCoffeeMaterial");
    }
    public static OrderData Matdict(CustomerType custype)
    {
        //switch()
        OrderData tempdata = new OrderData();
        tempdata.time = 50;
        tempdata.OrderMat.Add(BlackCoffee, 10);
        tempdata.sprite = null;
        //
        return tempdata;
        
    }
}

public class OrderData
{
    public float time;
    public Dictionary<RawMaterial, float> OrderMat = new Dictionary<RawMaterial, float>();
    public Sprite sprite;
}
