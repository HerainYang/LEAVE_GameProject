using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phone_AppearorDis : MonoBehaviour
{
    public UILabel Missiontext;
    public static string MissionContent;
    public TweenPosition tweenPosition;
    public static bool isCallOut = false;
    public GameObject Player;
    public void Controller()
    {
        if(isCallOut)
        {
            CallBack();
            isCallOut = false;
        }
        else if(!isCallOut&&Player.GetComponent<BasicMove>().GetControllable())
        {
            CallOut();
            
            isCallOut = true;
        }
    }
    public void CallOut()
    {
        //GameObject.Find("player").GetComponent<BasicMove>().SetUIState(true);
        ResourceController.UIUpdate();
        tweenPosition.PlayForward();
    }
    public void CallBack()
    {
        //GameObject.Find("player").GetComponent<BasicMove>().SetUIState(false);
        tweenPosition.PlayReverse();
        
    }
    
}
