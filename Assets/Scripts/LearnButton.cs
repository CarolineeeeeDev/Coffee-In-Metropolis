using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
public class LearnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject image1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 鼠标移入事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        image1.SetActive(true);
    }

    /// <summary>
    /// 鼠标移出事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        image1.SetActive(false);
    }

}
