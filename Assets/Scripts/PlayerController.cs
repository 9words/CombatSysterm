using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [Header("Ground Check Setting")]//做地面检测是为了给player重力，在地上给点重力，在空中给更多的重力
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    float yspeed;//追踪y的变化值，因为用于给player在摔落的时候给的y方向上的速度

    bool isgrounded;

    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController charactercontroller;
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator=GetComponent<Animator>();
        charactercontroller = GetComponent<CharacterController>();//角色碰撞器的中心的y值一般为角色碰撞器高度的一半+蒙皮宽度（skin width）
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float moveAmount =Mathf.Clamp01(Mathf.Abs( h) + Mathf.Abs(v));//人物移动的量做为动画混合树的移动量，需要限制在0-1之间

        var  moveInput= (new Vector3(h, 0, v)).normalized;//向量归一化，不让对角线的移动比单一方向的移动快
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

        if (moveAmount > 0)//人物移动的话
        {
           
            targetRotation = Quaternion.LookRotation(movedir);//人物面向移动的方向
        }

        transform.rotation=Quaternion.RotateTowards(transform.rotation,targetRotation,rotationSpeed
            *Time.deltaTime);//使得人物的旋转变成丝滑旋转不是瞬间转向
        animator.SetFloat("moveAmount",moveAmount,0.2f,Time.deltaTime);//第三个参数是阻尼值，使得停下不是瞬间停下的，可以平滑过度
    }
    void GroundCheck()//判断player是否站在地面上
    {
       isgrounded= Physics.CheckSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius,groundLayer);
    }
    private void OnDrawGizmosSelected()//绘制groundcheck球
    {
        Gizmos.color = new Color(0,1,0,0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset),groundCheckRadius);   
    }
}
