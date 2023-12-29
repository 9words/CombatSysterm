using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform followTraget;
    [SerializeField] float distance = 5f;
    [SerializeField] float rotatespeed = 2f;

    [SerializeField] float minVerticalangle = -45f;
    [SerializeField] float maxVerticalangle = 45f;
    [SerializeField] Vector2 farmingOffset ;

    [SerializeField] bool invertX;
    [SerializeField] bool invertY;

    float rotationY;
    float rotationX;

    float invertXval;
    float invertYval;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    private void Update()
    {
        invertXval = (invertX) ? -1 : 1;//倒转相机的判断
        //等同一下
        /*if(invertX)
        {
            invertXval = -1;
        }
        else
        {
            invertXval = 1;
        }*/
        invertYval = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Camera Y") * invertYval * rotatespeed;//操纵杆或者鼠标的上下移动决定镜头绕X轴旋转
        rotationX = Mathf.Clamp(rotationX, minVerticalangle, maxVerticalangle);//限制镜头上下移动的角度
       
        rotationY += Input.GetAxis("Camera X") * invertXval * rotatespeed;//操纵杆或者鼠标的左右移动决定镜头绕Y轴旋转

        var targetrotation = Quaternion.Euler(rotationX, rotationY, 0);
        
        var focuposition = followTraget.position + new Vector3( farmingOffset.x, farmingOffset.y);//设置一个镜头焦点，镜头应该照着人物的胸部以上位置
        
        transform.position = focuposition - targetrotation * new Vector3(0, 0, distance);
        transform.rotation = targetrotation;
    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);//提供相机的平面旋转
    //等同以下函数
   /* public Quaternion PlanarRotation()
    {
        return Quaternion.Euler(0,rotationY,0);
    }*/

}
