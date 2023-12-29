using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [Header("Ground Check Setting")]//����������Ϊ�˸�player�������ڵ��ϸ����������ڿ��и����������
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    float yspeed;//׷��y�ı仯ֵ����Ϊ���ڸ�player��ˤ���ʱ�����y�����ϵ��ٶ�

    bool isgrounded;

    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController charactercontroller;
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator=GetComponent<Animator>();
        charactercontroller = GetComponent<CharacterController>();//��ɫ��ײ�������ĵ�yֵһ��Ϊ��ɫ��ײ���߶ȵ�һ��+��Ƥ��ȣ�skin width��
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount =Mathf.Clamp01(Mathf.Abs( h) + Mathf.Abs(v));//�����ƶ�������Ϊ������������ƶ�������Ҫ������0-1֮��

        var  moveInput= (new Vector3(h, 0, v)).normalized;//������һ�������öԽ��ߵ��ƶ��ȵ�һ������ƶ���
        var movedir = cameraController.PlanarRotation * moveInput;
        GroundCheck();
        //Debug.Log("IsGround =" + isgrounded);
        if(isgrounded)
        {
            yspeed = -0.5f;
        }
        else
        {
            yspeed += Physics.gravity.y*Time.deltaTime;
        }
        var velocity = movedir * moveSpeed;
        velocity.y = yspeed;
        charactercontroller.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)//�����ƶ��Ļ�
        {
           
            targetRotation = Quaternion.LookRotation(movedir);//���������ƶ��ķ���
        }

        transform.rotation=Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed
            *Time.deltaTime);//ʹ���������ת���˿����ת����˲��ת��
        animator.SetFloat("moveAmount",moveAmount,0.2f,Time.deltaTime);//����������������ֵ��ʹ��ͣ�²���˲��ͣ�µģ�����ƽ������
    }
    void GroundCheck()//�ж�player�Ƿ�վ�ڵ�����
    {
       isgrounded= Physics.CheckSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius,groundLayer);
    }
    private void OnDrawGizmosSelected()//����groundcheck��
    {
        Gizmos.color = new Color(0,1,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius);   
    }
}
