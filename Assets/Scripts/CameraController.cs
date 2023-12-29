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
        invertXval = (invertX) ? -1 : 1;//��ת������ж�
        //��ͬһ��
        /*if(invertX)
        {
            invertXval = -1;
        }
        else
        {
            invertXval = 1;
        }*/
        invertYval = (invertY) ? -1 : 1;

        rotationX += Input.GetAxis("Camera Y") * invertYval * rotatespeed;//���ݸ˻������������ƶ�������ͷ��X����ת
        rotationX = Mathf.Clamp(rotationX, minVerticalangle, maxVerticalangle);//���ƾ�ͷ�����ƶ��ĽǶ�
       
        rotationY += Input.GetAxis("Camera X") * invertXval * rotatespeed;//���ݸ˻������������ƶ�������ͷ��Y����ת

        var targetrotation = Quaternion.Euler(rotationX, rotationY, 0);
        
        var focuposition = followTraget.position + new Vector3( farmingOffset.x, farmingOffset.y);//����һ����ͷ���㣬��ͷӦ������������ز�����λ��
        
        transform.position = focuposition - targetrotation * new Vector3(0, 0, distance);
        transform.rotation = targetrotation;
    }
    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);//�ṩ�����ƽ����ת
    //��ͬ���º���
   /* public Quaternion PlanarRotation()
    {
        return Quaternion.Euler(0,rotationY,0);
    }*/

}
