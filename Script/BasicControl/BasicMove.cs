using UnityEngine;
using System.Collections;
using System.Linq;
using System;
using System.Collections.Generic;

public class BasicMove : MonoBehaviour
{
    private int State;//角色状态
    private int oldState = 0;//前一次角色的状态
    private int UP = 0;//角色状态向前
    private int RIGHT = 1;//角色状态向右
    private int DOWN = 2;//角色状态向后
    private int LEFT = 3;//角色状态向左
    private int JUMP = 4;

    public int JumpForce;
    public Rigidbody rigidbody;
    public float speed = 8;
    private bool JumpOK = true;
    private float InteractRange;
    private bool Controllable = true;
    private bool UIOut = false;
    public List<string> Package = new List<string>();
    public Animator ani;

    public AudioSource Walk;

    private float GetThirdLength(float FirstLength, float SecondLength)
    {
        return Mathf.Sqrt(FirstLength * FirstLength + SecondLength * SecondLength);
    }

    private void OnCollisionEnter(Collision collision)//让角色只能一段跳
    {
        //Debug.Log(collision.collider.name);
        if (collision.collider.name == "ground")
            JumpOK = true;
    }
    
    void Start()
    {
        InteractRange = GetThirdLength(this.GetComponent<Collider>().bounds.size.y, GetThirdLength(this.GetComponent<Collider>().bounds.size.x, this.GetComponent<Collider>().bounds.size.z)) / 2;
        
    }
    void Update()
    {
        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
        RaycastHit hit;
        Physics.Raycast(transform.position,-transform.up,out hit);
        //Debug.Log(hit.normal);

        if(BasicInformation.NowWhere!=0)
        {
            UpperAnimatorSelect();
        }
        else
        {
            UpperAnimatorDone();
        }

        if (Controllable&&!UIOut)
        {
            if (Input.GetKey("w"))
            {
                setState(UP);
                ani.SetBool("IdleToWalk", true);
                Walk.enabled = true;
                UpperAnimatorSelect();
            }
            else if(Input.GetKeyUp("w"))
            {
                ani.SetBool("IdleToWalk", false);
                Walk.enabled = false;
                UpperAnimatorDone();
            }
            else if (Input.GetKey("s"))
            {
                setState(DOWN);
                ani.SetBool("IdleToWalk", true);
                ani.SetFloat("RePlay", -1);
                Walk.enabled = true;
                UpperAnimatorSelect();
            }
            else if(Input.GetKeyUp("s"))
            {
                ani.SetBool("IdleToWalk", false);
                ani.SetFloat("RePlay", 1);
                Walk.enabled = false;
                UpperAnimatorDone();
            }
            if (Input.GetKey("a"))
            {
                setState(LEFT);
            }
            else if (Input.GetKey("d"))
            {
                setState(RIGHT);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                setState(JUMP);
            }
        }
    }


    void setState(int currState)
    {
        Vector3 transformValue = new Vector3();//定义平移向量
        //int rotateValue = (currState - State) * 90;
        float rotateValue = 0;
        //transform.animation.Play("walk");//播放角色行走动画
        Quaternion targetRotation;
        switch (currState)
        {
            case 0://角色状态向前时，角色不断向前缓慢移动
                transformValue = Vector3.forward * Time.deltaTime * speed;
                break;
            case 1://角色状态向右时。角色不断向右缓慢移动
                rotateValue = 2.5f;

                break;
            case 2://角色状态向后时。角色不断向后缓慢移动
                transformValue = Vector3.back * Time.deltaTime * speed;
                break;
            case 3://角色状态向左时，角色不断向左缓慢移动
                rotateValue = -2.5f;

                break;
            case 4:
                if(JumpOK)
                {
                    //this.rigidbody.AddForce(transform.up * JumpForce);//实测200比较好
                    JumpOK = false;
                }
                break;
        }
        transform.Rotate(Vector3.up, rotateValue);//旋转角色
        
        transform.Translate(transformValue, Space.Self);//平移角色
        oldState = State;//赋值，方便下一次计算
        State = currState;//赋值，方便下一次计算
    }
    public void SetControllable(bool state)
    {
        Controllable = state;
    }
    public bool GetControllable()
    {
        return Controllable;
    }
    public void SetUIState(bool state)
    {
        UIOut = state;
    }
    void UpperAnimatorSelect()
    {
        if (BasicInformation.NowWhere == 0)
        {
            ani.SetBool("Up_IdleToWalk", true);
        }
        else
        {
            if(Shoot.WeaponState == 1)
            {
                ani.SetBool("Up_IdleToShoot", true);
            }
            else if(Shoot.WeaponState == 2)
            {
                ani.SetBool("Up_IdleToShoot_Grenade", true);
            }
            else if (Shoot.WeaponState == 0)
            {
                ani.SetBool("Up_IdleToKnife", true);
            }
        }
    }
    void UpperAnimatorDone()
    {
        if (BasicInformation.NowWhere == 0)
        {
            ani.SetBool("Up_IdleToWalk", false);
        }
        ani.SetBool("Up_IdleToShoot", false);
        ani.SetBool("Up_IdleToShoot_Grenade", false);
        ani.SetBool("Up_IdleToKnife", false);
    }
}
