using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleFighter : MonoBehaviour
{
    Animator animator;
    public bool InAction { get; private set; } = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void TryToAttack()
    {
        if(!InAction)
        {
            StartCoroutine("Attack");
       }
    }

    IEnumerator Attack()
    {
        InAction = true;
        animator.CrossFade("Slash", 0.2f);//�������浭�����룬�����0.2f����0.2�룬�ڽ��浭����������0.2fֵ�����õ�ǰ�Ķ�����20%ʱ�������ȵ���Ķ�����
                                          //��������������ڵ�ǰ֮֡��������
        yield return null;//��ΪCrossFade�������ڵ�ǰ֮֡���������������������ִ����Ҫ��һ֡

        var animState= animator.GetNextAnimatorStateInfo(1);//��ȡ��һ��Ҫ���ŵĶ���״̬��Ϣ

        yield return  new WaitForSeconds(animState.length);//�ȵ������Ķ���������
        InAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="HitBox"&&!InAction)
        {
            StartCoroutine("PlayHitReaction");
        }
    }

    IEnumerator PlayHitReaction()
    {
        InAction = true;
        animator.CrossFade("SwordImpact", 0.2f);//�������浭�����룬�����0.2f����0.2�룬�ڽ��浭����������0.2fֵ�����õ�ǰ�Ķ�����20%ʱ�������ȵ���Ķ�����
                                          //��������������ڵ�ǰ֮֡��������
        yield return null;//��ΪCrossFade�������ڵ�ǰ֮֡���������������������ִ����Ҫ��һ֡

        var animState = animator.GetNextAnimatorStateInfo(1);//��ȡ��һ��Ҫ���ŵĶ���״̬��Ϣ

        yield return new WaitForSeconds(animState.length);//�ȵ������Ķ���������
        InAction = false;
    }

}
