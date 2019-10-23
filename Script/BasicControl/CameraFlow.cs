using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraFlow : MonoBehaviour
{

    public Transform CirclePoint;
    public float speed;
    public float radius;
    private float angled;
    public float movepoint, uppoint;//movepoint一定大于radius才有预期效果
    public float smoothSpeed;
    private float posY, posZ;

    public static GameObject Moniter_1;
    public static GameObject Moniter_2;
    public static GameObject Moniter_3;



    public static int NowMoniter = 1;
    //用于计算差集的集合
    List<GameObject> Data_Current = new List<GameObject>();
    List<GameObject> Data_New = new List<GameObject>();
    List<GameObject> Data_CurrentExceptNew = new List<GameObject>();//用于存放Current有，New没有的元素
    List<GameObject> Data_NewExceptCurrent = new List<GameObject>();//用于存放New有，Current没有的元素

    List<Vector3> CameraShakeFix = new List<Vector3>();

    void Start()
    {
        Moniter_1 = transform.parent.Find("Monitor_1").gameObject;
        Moniter_2 = transform.parent.Find("Monitor_2").gameObject;
        Moniter_3 = transform.parent.Find("Monitor_3").gameObject;
        //Moniter_1 = GameObject.Find("Camera").gameObject.transform.Find("Monitor_1").gameObject;
        //Moniter_2 = GameObject.Find("Camera").gameObject.transform.Find("Monitor_2").gameObject;
        //Moniter_3 = GameObject.Find("Camera").gameObject.transform.Find("Monitor_3").gameObject;
        Vector3 startposition = CirclePoint.rotation * Vector3.forward * radius;
        transform.position = new Vector3(startposition.x, CirclePoint.position.y, startposition.z + movepoint);
    }

    public static int GetMoniterInfo()
    {
        return NowMoniter;
    }
    void Update()
    {
        if (BasicInformation.NowWhere == 0)
        {
            switch (NowMoniter)
            {
                case 1:
                    {
                        transform.position = Moniter_1.transform.position;
                        break;
                    }
                case 2:
                    {
                        transform.position = Moniter_2.transform.position;
                        break;
                    }
                case 3:
                    {
                        transform.position = Moniter_3.transform.position;
                        break;
                    }
            }
            transform.LookAt(CirclePoint);
        }
        else
        {
            InResourceArea();
        }
    }
    public void InLabArea()
    {

    }
    public void InResourceArea()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y > CirclePoint.position.y)
        {
            speed = -System.Math.Abs(speed);
            angled += (speed * Time.deltaTime) % 360;//累加已经转过的角度
            posY = radius * Mathf.Sin(angled * Mathf.Deg2Rad);
            posZ = radius * Mathf.Cos(angled * Mathf.Deg2Rad);//计算y位置

        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && transform.position.y > CirclePoint.position.y)
        {
            speed = System.Math.Abs(speed);
            angled += (speed * Time.deltaTime) % 360;//累加已经转过的角度
            posY = radius * Mathf.Sin(angled * Mathf.Deg2Rad);
            posZ = radius * Mathf.Cos(angled * Mathf.Deg2Rad);//计算y位置

        }
        else if (Input.GetMouseButton(1))
        {
            float nor = Input.GetAxis("Mouse X");//获取鼠标的偏移量
            transform.RotateAround(CirclePoint.position, Vector3.up, Time.deltaTime * speed * nor);//每帧旋转空物体，相机也跟随旋转
        }
        /*
        RaycastHit[] Hit;
        Hit = Physics.RaycastAll(transform.position, CirclePoint.position - transform.position, (transform.position - CirclePoint.position).magnitude);
        foreach (RaycastHit Hitelement in Hit)
        {
            GameObject Object;
            Object = GameObject.Find(Hitelement.collider.name);
            if (Object.tag == "UnmoveableWall")
                Data_New.Add(Object);
        }

        Data_NewExceptCurrent = Data_New.Except(Data_Current).ToList();
        foreach (GameObject Object in Data_NewExceptCurrent)
        {
            Object.GetComponent<Renderer>().material.color = new Color(Object.GetComponent<Renderer>().material.color.r, Object.GetComponent<Renderer>().material.color.g, Object.GetComponent<Renderer>().material.color.b, 0.4f);
        }
        Data_CurrentExceptNew = Data_Current.Except(Data_New).ToList();
        foreach (GameObject Object in Data_CurrentExceptNew)
        {
            Object.GetComponent<Renderer>().material.color = new Color(Object.GetComponent<Renderer>().material.color.r, Object.GetComponent<Renderer>().material.color.g, Object.GetComponent<Renderer>().material.color.b, 1.0f);
        }
        Data_Current.Clear();
        foreach (GameObject Object in Data_New)
        {
            Data_Current.Add(Object);
        }
        Data_New.Clear();
        */
        if (transform.position.y < CirclePoint.position.y+2)
        {
            float autospeed = speed * 10;
            //Debug.Log((transform.position - CirclePoint.position).magnitude);
            if ((transform.position - CirclePoint.position).magnitude < movepoint)
            {
                autospeed = -System.Math.Abs(speed);
                angled += (autospeed * Time.deltaTime) % 360;//累加已经转过的角度
                posY = radius * Mathf.Sin(angled * Mathf.Deg2Rad);
                posZ = radius * Mathf.Cos(angled * Mathf.Deg2Rad);//计算y位置
            }
            else
            {
                autospeed = System.Math.Abs(autospeed);
                angled += (autospeed * Time.deltaTime) % 360;//累加已经转过的角度
                posY = radius * Mathf.Sin(angled * Mathf.Deg2Rad);
                posZ = radius * Mathf.Cos(angled * Mathf.Deg2Rad);//计算y位置
            }
            transform.LookAt(CirclePoint);
            Vector3 nextposition = CirclePoint.forward * -1 * (movepoint + posZ) + CirclePoint.up * posY + CirclePoint.position;
            transform.position = Vector3.Lerp(this.transform.position, nextposition, smoothSpeed * Time.deltaTime);

        }


        transform.LookAt(CirclePoint);
        Vector3 nextpos = CirclePoint.forward * -1 * (movepoint + posZ) + CirclePoint.up * posY + CirclePoint.position;

        transform.position = Vector3.Lerp(this.transform.position, nextpos, smoothSpeed * Time.deltaTime);
    }
}
