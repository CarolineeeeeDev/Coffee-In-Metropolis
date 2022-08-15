using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : MonoBehaviour
{
    public ICup EnterCup;
    public Transform EmptyPos;
    public Animator Animator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Cup")
        {
            EnterCup = other.GetComponent<ICup>();
            EnterCup.EndInteraction.AddListener(EmptyCup);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Cup")
        {
            EnterCup.EndInteraction.RemoveListener(EmptyCup);
            EnterCup=null;
        }
    }


    private void EmptyCup()
    {
        EnterCup.EmptyCup();
        EnterCup.transform .SetParent(EmptyPos, false);
        EnterCup.transform.localPosition = Vector3.zero;
        EnterCup.transform.localRotation = Quaternion.identity;

        Animator.SetTrigger("Empty");
    }

}
