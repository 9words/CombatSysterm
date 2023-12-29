using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 5f;
    [SerializeField]
    float rotationSpeed = 500f;
    Quaternion targetRotation;
    CameraController cameraController;
    Animator animator;
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator=GetComponent<Animator>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount =Mathf.Clamp01(Mathf.Abs( h) + Mathf.Abs(v));//�����ƶ�������Ϊ������������ƶ�������Ҫ������0-1֮��

        var  moveInput= (new Vector3(h, 0, v)).normalized;//������һ�������öԽ��ߵ��ƶ��ȵ�һ������ƶ���
        var movedir = cameraController.PlanarRotation * moveInput;
        if (moveAmount > 0)//�����ƶ��Ļ�
        {
            transform.position += movedir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(movedir);//���������ƶ��ķ���
        }

        transform.rotation=Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed
            *Time.deltaTime);//ʹ���������ת���˿����ת����˲��ת��
        animator.SetFloat("moveAmount",moveAmount,0.2f,Time.deltaTime);//����������������ֵ��ʹ��ͣ�²���˲��ͣ�µģ�����ƽ������
    }
}
