using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�����ֽ׶εĺô��������������������ȵĴ���Ҳ���Խ����ײ���Ͳ����ܻ��Ķ�������Ϊ��ϸ�ڵĿ������ܻ�
public enum AttackState {Idle,Windup,Impact,Cooldown }//����״̬��ö�٣����У���ʼ���������ȴ��ָ���ǹ�����ҡ��

public class MeeleFighter : MonoBehaviour
{
    [SerializeField] List<AttackData> attacks;//�����б�
    [SerializeField] GameObject sword;
    BoxCollider swordCollider;

    Animator animator;
    AttackState attackState;
    bool doCombo;
    int ComboCount = 0;

    public bool InAction { get; private set; } = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        if(sword!=null)
        {
            swordCollider = sword.GetComponent<BoxCollider>();
            swordCollider.enabled = false;
        }
    }
    public void TryToAttack()
    {
        if(!InAction)
        {
            StartCoroutine("Attack");
        }
        else if(attackState==AttackState.Impact||attackState==AttackState.Cooldown)
        {
            doCombo = true;
        }
    }

    IEnumerator Attack()
    {
        InAction = true;
        attackState = AttackState.Windup;
      /*  float impactStartTime = 0.33f;//�����ʼ�İٷֱ�
        float impactEndTime = 0.55f;//��������İٷֱ�*/

        animator.CrossFade(attacks[ComboCount].AniNmae, 0.2f);//�������浭�����룬�����0.2f����0.2�룬�ڽ��浭����������0.2fֵ�����õ�ǰ�Ķ�����20%ʱ�������ȵ���Ķ�����
                                          //��������������ڵ�ǰ֮֡��������
        yield return null;//��ΪCrossFade�������ڵ�ǰ֮֡���������������������ִ����Ҫ��һ֡

        var animState= animator.GetNextAnimatorStateInfo(1);//��ȡ��һ��Ҫ���ŵĶ���״̬��Ϣ
        float timer = 0f;
        while(timer<=animState.length) //�ڹ����Ķ���������ǰ�������飬timer���ڼ��㶯����ʱ��
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;
            if(attackState==AttackState.Windup)//������������ʱ
            {
                if(normalizedTime >= attacks[ComboCount].ImpactStartTime)//�������������ʱ�䵽�˻ӿ���ʱ���
                {
                    attackState = AttackState.Impact;//״̬��Ϊ�ӿ�
                    //������ײ
                    swordCollider.enabled = true;
                }
            }
            else if(attackState==AttackState.Impact)//���ڹ����Ļӿ��׶�
            {
                if(normalizedTime>= attacks[ComboCount].ImpactEndTime)//�������������ʱ����˻ӿ���ʱ���
                {
                    attackState = AttackState.Cooldown;//״̬��Ϊ��ȴ
                    //������ײ
                    swordCollider.enabled = false;
                }
            }
            else if(attackState==AttackState.Cooldown)//������ȴ�׶ε�
            {
                if(doCombo)//�Ƿ��������
                {
                    doCombo = false;
                    ComboCount = (ComboCount + 1) % attacks.Count;
                    /*�������õ�ͬ����
                     * ++ComboCount;
                     if(ComboCount>=attacks.Count)
                     {
                         ComboCount = 0;
                     }*/
                    StartCoroutine("Attack");
                    yield break;
                }
            }
            yield return null;
        }
        attackState = AttackState.Idle;
        ComboCount = 0;
        InAction = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="HitBox"&&!InAction)
        {
            StartCoroutine("PlayHitReaction");
        }
    }

    IEnumerator PlayHitReaction()//�ܵ������ķ�Ӧ
    {
        InAction = true;
        animator.CrossFade("SwordImpact", 0.2f);//�������浭�����룬�����0.2f����0.2�룬�ڽ��浭����������0.2fֵ�����õ�ǰ�Ķ�����20%ʱ�������ȵ���Ķ�����
                                          //��������������ڵ�ǰ֮֡��������
        yield return null;//��ΪCrossFade�������ڵ�ǰ֮֡���������������������ִ����Ҫ��һ֡

        var animState = animator.GetNextAnimatorStateInfo(1);//��ȡ��һ��Ҫ���ŵĶ���״̬��Ϣ

        yield return new WaitForSeconds(animState.length*0.8f);//�ȵ������Ķ�����80%������
        InAction = false;
    }

}
