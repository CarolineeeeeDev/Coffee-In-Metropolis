using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
/// <summary>
/// 单个顾客的行为
/// </summary>
public class Customers : MonoBehaviour
{
   // bool con = true;
    bool ordend = false;
    int tempv;
    public CustomerType cusType;//顾客类型
    public OrderData order;//顾客的订单
    private NavMeshAgent nav;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        order = OrderFactory.Matdict(cusType);//通过顾客类型生成订单
        ToBar(() =>
        {
            //玩家到吧台了
            animator.SetTrigger("Wave");
            //当播完动画后{ Bar._in.SpawnOrder }
            StartCoroutine(WAnimEnd("Wave", ()=> { Bar._in.SpawnOrder(this, order); }));

        }
        );
    }


    public void BTest()
    {
        Navigate(new Vector3(3.05999994f, -2.18188024f, 0.270000011f), BTestEnd);

    }
   

    public void End(int v)
    {
        ToBar(() =>
        {
            //Debug.Log(v);
            //玩家到吧台了
            animator.SetTrigger("Wave");

            StartCoroutine(WAnimEnd("Wave", () => {
                ordend = true;
                tempv = v;
                PlayAnim(tempv);
                BTest();
            }));
        }
        );
      
     
     //   Debug.Log(tempv);
        //在顾客头上显示来点单的
    }


    public void PlayAnim(int v)
    {
        if (v == 100)
        {
            animator.SetBool("Happy", true);
        }
        else if (v > 80)
        {

        }
        else if (v > 60)
        {

        }
        else
        {
            animator.SetBool("Angry", true);
        }
        Invoke("StopAnim", 3);
    }
    
    private void StopAnim()
    {
        
       /* animator.SetBool("Happy", false);
        animator.SetBool("Angry", false);
        animator.SetBool("Wave", false);
        animator.SetBool("Stop", true);*/
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="pos">目标点</param>
    /// <param name="NavEnd">结束回调</param>
    /// <param name="a">超时回调</param>
    /// <param name="di"></param>
    /// <param name="time"></param>
    public void Navigate(Vector3 pos, Action NavEnd = null, Action a = null, float di = 0.5f, float time = -1)//导航功能
    {
        nav.isStopped = false;
        animator.SetBool("IsMoving", true);
        nav.SetDestination(pos);
        StopCoroutine("Nav");
        StartCoroutine(Nav(pos, () =>
        {
            nav.isStopped = true;
            animator.SetBool("IsMoving", false);
            NavEnd?.Invoke();
        },a, 
        di, time));
    }
    /*

    
    static Vector3 trans1 = new Vector3(3.05999994f, -2.18188024f, 0.270000011f);
    static Vector3 trans2 = new Vector3(1.32000005f, -2.18188024f, -1.27999997f);
    static Vector3 trans3 = new Vector3(4.23999977f, -2.18188024f, -2.03999996f);
    static Vector3 trans4 = new Vector3(4.23999977f, -2.18188024f, 0.910000026f);
    static Vector3 trans5 = new Vector3(0.790000021f, -2.18188024f, 2.3499999f);
    static Vector3 trans6 = new Vector3(-0.374000013f, -2.18188024f, -1.95000005f);
    List<Vector3> Allpos = new List<Vector3>() { trans1,trans2,trans3,trans4,trans5,trans6 };

    private void TestNav()
    {
        Vector3 ranPos = Allpos[UnityEngine.Random.Range(0, Allpos.Count)];
        Navigate(ranPos, () =>
         {
             TestNav();
         });
    }

    */
   
    public void LeaveBar()//取餐&离开
    {

    }


    public void ToBar(Action NavEnd = null)//前往吧台
    {
        Vector3 tar = Bar._in.GetPos(this);
        Navigate(tar, NavEnd);
    }



    private void BTestEnd()
    {
        //玩家退出吧台位置结束
        animator.SetBool("Wave", false);
    }



    private IEnumerator Nav(Vector3 pos, Action NavEnd, Action a, float di, float time)
    {
        while (true)
        {
            float atTime = 0;
            if (atTime > time && time > 0)
            {
                a?.Invoke();
            }
            if ((transform.position - pos).magnitude < di)
            {
                break;
            }
            yield return new WaitForSeconds(0.02f);
            atTime += 0.02f;
        }

        NavEnd?.Invoke();
    }


    private IEnumerator WAnimEnd(string animName ,Action act)
    {
        
        while (true)
        {

            AnimatorStateInfo animatorInfo;
            animatorInfo = animator.GetCurrentAnimatorStateInfo(0);
            //Debug.Log(animator.GetAnimatorTransitionInfo(0).IsName(animName));
            //Debug.Log(animator.GetAnimatorTransitionInfo(0).normalizedTime);
            if (animatorInfo.normalizedTime > 1f&& animatorInfo.IsName(animName))
            {
                act?.Invoke();
                Debug.Log($"动画播放完毕:{animName}");
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        //act?.Invoke();
    }
}
