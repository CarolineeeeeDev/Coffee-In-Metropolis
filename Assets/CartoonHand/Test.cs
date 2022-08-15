using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    Animator m_Animator;

    private void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
    }
    public void SetFlex(Single f)
    {
        m_Animator.SetFloat("Flex", f);
    }
    public void SetPose(Single f)
    {
        m_Animator.SetFloat("Pose", f);
    }
    public void SetPinch(Single f)
    {
        m_Animator.SetFloat("Pinch", f);
    }
}
