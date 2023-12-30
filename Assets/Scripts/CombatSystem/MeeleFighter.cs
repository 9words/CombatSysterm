using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//攻击分阶段的好处是有利于连击，反击等的处理，也可以解决碰撞到就播放受击的动画，改为更细节的砍到才受击
public enum AttackState {Idle,Windup,Impact,Cooldown }//攻击状态的枚举：空闲，开始，冲击，冷却（指的是攻击后摇）

public class MeeleFighter : MonoBehaviour
{
    [SerializeField] List<AttackData> attacks;//攻击列表
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
      /*  float impactStartTime = 0.33f;//冲击开始的百分比
        float impactEndTime = 0.55f;//冲击结束的百分比*/

        animator.CrossFade(attacks[ComboCount].AniNmae, 0.2f);//动画交叉淡出淡入，这里的0.2f不是0.2秒，在交叉淡出淡入这里0.2f值得是用当前的动画的20%时长来过度到别的动画，
                                          //并且这个函数是在当前帧之后启动的
        yield return null;//因为CrossFade函数是在当前帧之后启动的所以这里下面的执行需要等一帧

        var animState= animator.GetNextAnimatorStateInfo(1);//获取下一个要播放的动画状态信息
        float timer = 0f;
        while(timer<=animState.length) //在攻击的动画播放完前做的事情，timer用于计算动画的时长
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / animState.length;
            if(attackState==AttackState.Windup)//攻击动作触发时
            {
                if(normalizedTime >= attacks[ComboCount].ImpactStartTime)//如果攻击动画的时间到了挥砍的时间段
                {
                    attackState = AttackState.Impact;//状态改为挥砍
                    //启用碰撞
                    swordCollider.enabled = true;
                }
            }
            else if(attackState==AttackState.Impact)//处于攻击的挥砍阶段
            {
                if(normalizedTime>= attacks[ComboCount].ImpactEndTime)//如果攻击动画的时间过了挥砍的时间段
                {
                    attackState = AttackState.Cooldown;//状态改为冷却
                    //禁用碰撞
                    swordCollider.enabled = false;
                }
            }
            else if(attackState==AttackState.Cooldown)//处于冷却阶段的
            {
                if(doCombo)//是否进行连击
                {
                    doCombo = false;
                    ComboCount = (ComboCount + 1) % attacks.Count;
                    /*以下作用等同上行
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

    IEnumerator PlayHitReaction()//受到攻击的反应
    {
        InAction = true;
        animator.CrossFade("SwordImpact", 0.2f);//动画交叉淡出淡入，这里的0.2f不是0.2秒，在交叉淡出淡入这里0.2f值得是用当前的动画的20%时长来过度到别的动画，
                                          //并且这个函数是在当前帧之后启动的
        yield return null;//因为CrossFade函数是在当前帧之后启动的所以这里下面的执行需要等一帧

        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个要播放的动画状态信息

        yield return new WaitForSeconds(animState.length*0.8f);//等到攻击的动画的80%播放完
        InAction = false;
    }

}
