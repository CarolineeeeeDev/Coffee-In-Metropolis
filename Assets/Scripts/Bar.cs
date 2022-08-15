using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bar : MonoBehaviour
{
    
    public static Vector3 pos;
    public static Bar _in;
    public Transform par;
    public Transform StartPos;
    public Customers inCust;

    public List<Customers> OnCust=new List<Customers>();
    // Start is called before the first frame update
    private void Awake()
    {
        if (_in==null)
        {
            _in=this;
        }
    }
    void Start()
    {
        if(pos == Vector3.zero)
        {
            pos = transform.position;
        }
    }

    // Update is called once per frame
    public Vector3 GetPos(Customers custItem)
    {
        Vector3 tar;
        if (inCust==null & OnCust.Count==0)
        {
            tar = StartPos.position;
        }
        else
        {

            tar = StartPos.position;
            tar.x += OnCust.Count*2;
        }
        OnCust.Add(custItem);
        return tar;
    }

    void Update()
    {
        
    }
    
    //更新排队位置
    private void UpdateCustPos()
    {
        for (int i = 0; i < OnCust.Count; i++)
        {

            Vector3 tar = StartPos.position;
            tar.x += i * 2;
            OnCust[i].Navigate(tar);
        }
    }

    public GameObject ordermodel;
    public void SpawnOrder(Customers customers,OrderData or)
    {
        if (customers.name=="Customer1(Clone)")
        {
            ordermodel = Resources.Load<GameObject>("order1");
        }
        else if (customers.name == "Customer2(Clone)")
        {
            ordermodel = Resources.Load<GameObject>("order2");
        }
        else if (customers.name == "Customer3(Clone)")
        {
            ordermodel = Resources.Load<GameObject>("order3");
        }
        else if (customers.name == "Customer4(Clone)")
        {
            ordermodel = Resources.Load<GameObject>("order4");
        }

        ordermodel = GameObject.Instantiate(ordermodel,par,false);

        OrderInfo tempInfo  = ordermodel.GetComponent<OrderInfo>();
        tempInfo.Init(this,customers, or);

        //StartCoroutine(BTest(customers));
    }
    //模拟点单耗时
    private IEnumerator BTest(Customers customers)
    {
        yield return new WaitForSeconds(3);
        //customers.BTest();
    }
    //玩家提交咖啡
    public void PlayerEnd(Dictionary<RawMaterial,float> mat,OrderInfo order)
    {
        var temp = order.End(mat);
        order.m_Customers.End(temp);
    }

    public OrderInfo order;
    public ICup m_cup;

    /*private void OnGUI()
    {
        if(GUILayout.Button("提交订单"))
        {
            PlayerEnd(m_cup.CoffeeMaterials, order);
        }
    }*/

    public void OrdEnd(OrderInfo order)
    {
        order.m_Customers.End(100);
        Destroy(order.gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            if (inCust==null)
            {
                inCust = other.GetComponent<Customers>();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (inCust != null && other.GetComponent<Customers>()==inCust)
            {
                NewMethod(); 
            }
        }
    }

    private void NewMethod()
    {
        OnCust.Remove(inCust);
        inCust = null;
        UpdateCustPos();
    }
}
