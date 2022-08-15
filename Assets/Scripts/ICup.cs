using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ICup : MonoBehaviour
{

    public UnityEvent EndInteraction;
    public UnityEvent StartInteraction;
    public GameObject Cream;
    public GameObject Jellybeans;
    public GameObject CoffeePlane;
    public GameObject CottonCandy;
    public string selfname="EmptyCup";

    public void EmptyCup()
    {
        Cream?.SetActive(false);
        CottonCandy?.SetActive(false);
        CoffeePlane?.SetActive(false);
        Jellybeans?.SetActive(false);
        selfname = "EmptyCup";
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Bin")
        {
            EmptyCup();
        }
        if (CoffeePlane.activeSelf != true)
        {
            if (other.tag=="Coffee")
            {
                CoffeePlane.SetActive(true);
                //Destroy(other);
                selfname = "Blackcoffee";
            }
        }
        else
        {
            if (Jellybeans.activeSelf != true)
            {
                if(other.tag=="Jellybeans"&&CottonCandy.activeSelf == false && Cream.activeSelf == false)
                {
                    Jellybeans.SetActive(true);
                    Destroy(other.gameObject);
                    selfname = "JellybeansCoffee";
                }
            }
            if (Cream.activeSelf != true)
            {
                if(other.tag=="Cream"&&CottonCandy.activeSelf==false&&Jellybeans.activeSelf==false)
                {
                    Cream.SetActive(true);
                    Destroy(other.gameObject);
                    selfname = "CreamCoffee";
                }
            }
            if (CottonCandy.activeSelf != true)
            {
                if (other.tag == "CottonCandy"&&Cream.activeSelf == false && Jellybeans.activeSelf == false)
                {
                    CottonCandy.SetActive(true);
                    Destroy(other.gameObject);
                    selfname = "CottonCandyCoffee";
                }
            }
        }

        
    }

    public void TriggerEndInteraction()
    {
        EndInteraction?.Invoke();
    }

    public void TriggerStartInteraction() => StartInteraction?.Invoke();


    /*
    //杯子内的材料
    public Dictionary<RawMaterial, float> CoffeeMaterials = new Dictionary<RawMaterial, float>();


    public float L;

    /// <summary>
    /// 添加材料进入杯子  由机器调用
    /// </summary>
    /// <param name="coffeeMaterial"></param>
    /// <param name="_v"></param>
    public void AddCMaterial(RawMaterial coffeeMaterial, float _v)
    {
       if (!CoffeeMaterials.ContainsKey(coffeeMaterial)) //判断是否存在该KEY
        {
            CoffeeMaterials.Add(coffeeMaterial, _v);//添加KEy
            return;//不做下面内容
        }
        CoffeeMaterials[coffeeMaterial] += _v;
        L += _v;
    }

    public void SubCMaterial(RawMaterial coffeeMaterial, float _v)
    {
        CoffeeMaterials[coffeeMaterial] -= _v;
        L -= _v;
    }

    public void EmptyCup()
    {
        CoffeeMaterials.Clear ();
      

        Debug.Log("Empty");
    }

    public void FillCup()
    {
        CoffeeMaterials.Clear();
        Debug.Log("Fill");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TriggerStartInteraction();
        }
        if (Input.GetKeyUp(KeyCode.R))
        {
            TriggerEndInteraction();
        }
    }
    */
    
}
