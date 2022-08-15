using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CustomerType
{
    Child,Intern,Lady,OldWoman
}
/// <summary>
/// �˿�������
/// </summary>
public class CustomerSpawner : MonoBehaviour
{
    List<Customers> customersinscene = new List<Customers>();

    int customernum;

    List<string> customertypes = new List<string>() { "Customer1" , "Customer2" , "Customer3" , "Customer4" };
   


    void Start()
    {
        Invoke("Spawn",2);
    }
    private void Awake()
    {
        OrderFactory.InitFactory();
    }

    void Update()
    {
        //ÿ��һ��ʱ�����ɹ˿�
    }

    void Spawn()
    {
        if (customersinscene.Count >3)
        {
            return;
        }
        customernum = Random.Range(0, 4);
        string newcustomer = customertypes[customernum];
        GameObject customer= Resources.Load<GameObject>(newcustomer);//������ɹ˿�
        customer=GameObject.Instantiate(customer,transform.position,transform.rotation);
        
        var tempCustInfo = customer.GetComponent<Customers>();
        //tempCustInfo.order = OrderFactory.Matdict(tempCustInfo.cusType);
        customersinscene.Add(tempCustInfo);
        Invoke("Spawn", Random.Range(1, 3));
        //���ʲ��м��ع˿ͣ�ʵ�����˿ͣ�����˿���Ϣ
    }

    public void Delete(Customers cus)
    {
        //�Ƴ��˿�
    }
}
