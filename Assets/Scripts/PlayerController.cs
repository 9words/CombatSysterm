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
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount =Mathf.Abs( h) + Mathf.Abs(v);//�����ƶ�����

        var  moveInput= (new Vector3(h, 0, v)).normalized;//������һ�������öԽ��ߵ��ƶ��ȵ�һ������ƶ���
        var movedir = cameraController.PlanarRotation * moveInput;
        if (moveAmount > 0)//�����ƶ��Ļ�
        {
            transform.position += movedir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(movedir);//���������ƶ��ķ���
        }

        transform.rotation=Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed*Time.deltaTime);//ʹ���������ת���˿����ת����˲��ת��
    }
}
