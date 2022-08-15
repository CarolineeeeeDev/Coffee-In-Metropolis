using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
/// <summary>
/// �����˿͵���Ϊ
/// </summary>
public class Customers : MonoBehaviour
{
   // bool con = true;
    bool ordend = false;
    int tempv;
    public CustomerType cusType;//�˿�����
    public OrderData order;//�˿͵Ķ���
    private NavMeshAgent nav;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        order = OrderFactory.Matdict(cusType);//ͨ���˿��������ɶ���
        ToBar(() =>
        {
            //��ҵ���̨��
            animator.SetTrigger("Wave");
            //�����궯����{ Bar._in.SpawnOrder }
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
            //��ҵ���̨��
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
        //�ڹ˿�ͷ����ʾ���㵥��
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
    /// <param name="pos">Ŀ���</param>
    /// <param name="NavEnd">�����ص�</param>
    /// <param name="a">��ʱ�ص�</param>
    /// <param name="di"></param>
    /// <param name="time"></param>
    public void Navigate(Vector3 pos, Action NavEnd = null, Action a = null, float di = 0.5f, float time = -1)//��������
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
   
    public void LeaveBar()//ȡ��&�뿪
    {

    }


    public void ToBar(Action NavEnd = null)//ǰ����̨
    {
        Vector3 tar = Bar._in.GetPos(this);
        Navigate(tar, NavEnd);
    }



    private void BTestEnd()
    {
        //����˳���̨λ�ý���
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
                Debug.Log($"�����������:{animName}");
                break;
            }
            yield return new WaitForSeconds(0.02f);
        }
        //act?.Invoke();
    }
}
