using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeReceiver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Cup")
        {
            Debug.Log(other.GetComponent<ICup>().selfname);
        }
    }
}
