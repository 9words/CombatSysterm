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
        animator.CrossFade("Slash", 0.2f);//动画交叉淡出淡入，这里的0.2f不是0.2秒，在交叉淡出淡入这里0.2f值得是用当前的动画的20%时长来过度到别的动画，
                                          //并且这个函数是在当前帧之后启动的
        yield return null;//因为CrossFade函数是在当前帧之后启动的所以这里下面的执行需要等一帧

        var animState= animator.GetNextAnimatorStateInfo(1);//获取下一个要播放的动画状态信息

        yield return  new WaitForSeconds(animState.length);//等到攻击的动画播放完
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
        animator.CrossFade("SwordImpact", 0.2f);//动画交叉淡出淡入，这里的0.2f不是0.2秒，在交叉淡出淡入这里0.2f值得是用当前的动画的20%时长来过度到别的动画，
                                          //并且这个函数是在当前帧之后启动的
        yield return null;//因为CrossFade函数是在当前帧之后启动的所以这里下面的执行需要等一帧

        var animState = animator.GetNextAnimatorStateInfo(1);//获取下一个要播放的动画状态信息

        yield return new WaitForSeconds(animState.length);//等到攻击的动画播放完
        InAction = false;
    }

}
