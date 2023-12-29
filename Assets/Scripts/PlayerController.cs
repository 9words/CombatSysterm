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
        float moveAmount =Mathf.Abs( h) + Mathf.Abs(v);//人物移动的量

        var  moveInput= (new Vector3(h, 0, v)).normalized;//向量归一化，不让对角线的移动比单一方向的移动快
        var movedir = cameraController.PlanarRotation * moveInput;
        if (moveAmount > 0)//人物移动的话
        {
            transform.position += movedir * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(movedir);//人物面向移动的方向
        }

        transform.rotation=Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed*Time.deltaTime);//使得人物的旋转变成丝滑旋转不是瞬间转向
    }
}
