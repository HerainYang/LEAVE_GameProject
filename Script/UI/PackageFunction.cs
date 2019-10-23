using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackageFunction : MonoBehaviour
{
    private const string Buttons = "Buttons";
    private const string Calendar = "Calendar";
    
    private GameObject MainInterface;
    public GameObject Description;
    private UILabel Text;
    private void Start()
    {
        Text = Description.GetComponent<UILabel>();
    }
    public void Back()
    {
        WeakUpMainInterface();
        this.gameObject.SetActive(false);
        Phone_AppearorDis.MissionContent = "Now change";
        
    }
    
    public void Wood_Place()
    {
        if(ResourceController.TakeResource("Wood",1,false)!=-1)
        {
            ResourceController.UIUpdateObject("Wood");
            ResourceController.PlaceWood();
        }
        //放置木头
    }
    public void Pistol()
    {
        Shoot.SetPistolPattern();
        //切换到手枪
    }
    public void Grenade()
    {
        Shoot.SetGrenadePattern();
        //切换到榴弹
    }
    public void Knife()
    {
        //切换到小刀
        Shoot.SetKnife();
    }
    public void Lighter()
    {
        ResourceController.LightTheWood();
        //点燃木材
    }
    public void Water()
    {
        //回复理智
        if (ResourceController.TakeResource("Water", 1, false) != -1)
        {
            ResourceController.UIUpdateObject("Water");
            ResourceController.DrinkWater();
        }
    }
    public void Serum()
    {
        //回复生命力
        ResourceController.UesSerum();
    }
    private void WeakUpMainInterface()
    {
        MainInterface = this.transform.parent.Find("MainInterface").gameObject;
        MainInterface.SetActive(true);
        //MainInterface.transform.Find(Buttons).gameObject.SetActive(true);
        //MainInterface.transform.Find(Calendar).gameObject.SetActive(true);
    }
    private void WakeUpDescription()
    {
        this.transform.parent.Find("ObjectDescription").gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void MoreDetail_Wood()
    {
        Debug.Log(this.name);
        Text.text = "较为常见的资源，空间站有大量需求，可用打火机点燃，回复体力";
        WakeUpDescription();
    }
    public void MoreDetail_Metal()
    {
        Debug.Log(this.name);
        Text.text = "较为稀缺的资源，空间站固定需求";
        WakeUpDescription();
    }
    public void MoreDetail_Stone()
    {
        Debug.Log(this.name);
        Text.text = "较为常见的资源，空间站有大量需求";
        WakeUpDescription();
    }
    public void MoreDetail_Serum()
    {
        Debug.Log(this.name);
        Text.text = "可少量回复健康值，有时空间站会少量寄回，有时可从其他人中获取";
        WakeUpDescription();
    }
    public void MoreDetail_Oil()
    {
        Debug.Log(this.name);
        Text.text = "极为稀缺的资源，空间站有大量需求";
        WakeUpDescription();
    }
    public void MoreDetail_Knife()
    {
        Debug.Log(this.name);
        Text.text = "需要一段时间冷却的防御道具，可击退前来骚扰的动物，没什么伤害";
        WakeUpDescription();
    }
    public void MoreDetail_Pistol()
    {
        Debug.Log(this.name);
        Text.text = "伤害较高的杀伤力武器，射程较远，需要子弹";
        WakeUpDescription();
    }
    public void MoreDetail_Grenade()
    {
        Debug.Log(this.name);
        Text.text = "伤害超高的武器，射程较近，需要子弹";
        WakeUpDescription();
    }
    public void MoreDetail_Light()
    {
        Debug.Log(this.name);
        Text.text = "可用于点燃木材制作篝火回复生命值";
        WakeUpDescription();
    }
    public void MoreDetail_Water()
    {
        Debug.Log(this.name);
        Text.text = "在资源区冒险时用于回复理智，保持清醒";
        WakeUpDescription();
    }
    public void MoreDetail_PistolBullet()
    {
        Debug.Log(this.name);
        Text.text = "粒子枪的子弹";
        WakeUpDescription();
    }
    public void MoreDetail_GrenadeBullet()
    {
        Debug.Log(this.name);
        Text.text = "榴弹枪的子弹";
        WakeUpDescription();
    }
    

}
