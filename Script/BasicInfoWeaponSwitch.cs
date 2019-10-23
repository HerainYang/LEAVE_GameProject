using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicInfoWeaponSwitch : MonoBehaviour
{
    public GameObject GetPistolBullet;
    public GameObject GetGrenadeBullet;

    public GameObject PistolBullet;
    public GameObject GrenadeBullet;


    private void Update()
    {
        PistolBullet.GetComponent<UILabel>().text = GetPistolBullet.GetComponent<UILabel>().text;
        GrenadeBullet.GetComponent<UILabel>().text = GetGrenadeBullet.GetComponent<UILabel>().text;
        if (PistolBullet.GetComponent<UILabel>().text == "0")
        {
            PistolBullet.GetComponent<UILabel>().color = Color.red;
        }
        else
        {
            PistolBullet.GetComponent<UILabel>().color = Color.black;
        }
        if (GrenadeBullet.GetComponent<UILabel>().text == "0")
        {
            GrenadeBullet.GetComponent<UILabel>().color = Color.red;
        }
        else
        {
            GrenadeBullet.GetComponent<UILabel>().color = Color.black;
        }
    }
    public void Wood_Place()
    {
        if (ResourceController.TakeResource("Wood", 1, false) != -1)
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
        if(BasicInformation.Health<7)
        {
            if (ResourceController.TakeResource("Serum", 1, false) != -1)
            {
                BasicInformation.Health += 1;
                BasicInformation.HealthIsAdd = true;
            }
        }
    }
}
