using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="Combat System/Create a new attack")]//���Ҽ��˵��м��봴������˵�
public class AttackData : ScriptableObject//Ϊ�˰���Ӳ���루���������ڴ����������ֵ�ģ������������ݸĳ�һ���ɱ�д�ű������Ա������
{
    [field :SerializeField] public string AniNmae { get; private set; }

    [field: SerializeField] public float ImpactStartTime { get; private set; }//�����İٷֱ�

    [field: SerializeField] public float ImpactEndTime { get; private set; }//�����İٷֱ�



}
