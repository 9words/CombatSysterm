using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Combat System/Create a new attack")]//在右键菜单中加入创建这个菜单
public class AttackData : ScriptableObject//为了摆脱硬编码（即将变量在代码里面给赋值的），将动画数据改成一个可编写脚本对象以便于添加
{
    [field :SerializeField] public string AniNmae { get; private set; }

    [field: SerializeField] public float ImpactStartTime { get; private set; }//动画的百分比

    [field: SerializeField] public float ImpactEndTime { get; private set; }//动画的百分比



}
