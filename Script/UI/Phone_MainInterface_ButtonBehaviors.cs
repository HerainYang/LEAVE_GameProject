using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Phone_MainInterface_ButtonBehaviors : MonoBehaviour
{
    public GameObject Parent;
    public UIAtlas atlas;
    public Font f;
    public GameObject ExitInterface;
    private UISprite sprite;
    UICreator creator;

    public void SetActive_Bag()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.transform.parent.parent.Find("PackageInterface").gameObject.SetActive(true);
    }
    public void SetActive_Mission()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.transform.parent.parent.Find("MissionInterface").gameObject.SetActive(true);
    }
    public void SetActive_Letter()
    {
        for(int i = 1;i<= BasicInformation.Day; i++)//Letter_DB.GetAddressableLetter()
        {
            creator = new UICreator(Parent, atlas, f, i);
        }
        
        this.transform.parent.gameObject.SetActive(false);
        this.transform.parent.parent.Find("LetterInterface").gameObject.SetActive(true);
    }
    public void Enter_ResourceArea()
    {
        if(BasicInformation.Day>0&&BasicInformation.FirstEnterResourceArea&&BasicInformation.NowWhere == 0)
        {
            BasicInformation.EnterResourceArea();
            BasicInformation.SetPlace(1);
        }
    }
    public void Enter_LabArea()
    {
        if(BasicInformation.NowWhere != 0)
        {
            BasicInformation.EnterLabArea();
            BasicInformation.SetPlace(0);
        }
    }
   public void ExitGame()
    {
        ExitInterface.SetActive(true);
    }
}
