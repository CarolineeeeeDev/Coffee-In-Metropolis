using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeeMachine : MonoBehaviour
{
    public ICup EnterCup;
    public Transform FillPos;
    public Animator Animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cup")
        {
            EnterCup = other.GetComponent<ICup>();
            EnterCup.EndInteraction.AddListener(FillCup);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cup")
        {
            EnterCup.EndInteraction.RemoveListener(FillCup);
            EnterCup = null;
        }
    }


    private void FillCup()
    {
        //EnterCup.FillCup();
        EnterCup.transform.SetParent(FillPos, false);
        EnterCup.transform.localPosition = Vector3.zero;
        EnterCup.transform.localRotation = Quaternion.identity;

        Animator.SetTrigger("Fill");



    }
}
