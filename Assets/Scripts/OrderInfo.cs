using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderInfo : MonoBehaviour
{
    public Customers m_Customers;
    public OrderData m_Data;
    private Bar m_Bar;
    public Slider slider;
    private float AtTime;
    

    public void Init(Bar bar,Customers customers,OrderData order)
    {
        m_Bar = bar;
        m_Customers = customers;
        m_Data= order;
    }

    private void Update()
    {
        if (m_Data.time<AtTime)
        {
            m_Bar.OrdEnd(this);
            
        }
        AtTime +=Time.deltaTime;
        slider.value = AtTime / m_Data.time;
    }

    public int End(Dictionary<RawMaterial, float> matdata)
    {
        int v = 100;
        foreach(var item in m_Data.OrderMat.Keys)
        {
            if(!matdata.ContainsKey(item))
            {
                v -= 10;
            }
            else
            {
                Debug.Log($"订单{item.name}需要的值{m_Data.OrderMat[item]}_____杯子{item}有{matdata[item]}");
                v -= (int)(m_Data.OrderMat[item] - matdata[item]);
            }
        }
        return v;
    }
}
