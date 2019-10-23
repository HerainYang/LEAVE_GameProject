using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionFunction : MonoBehaviour
{
    //public UILabel MissionDiscribe;
    public MissionFunction(){
        
        //Debug.Log("here is" + this.GetType().Assembly.Location);
    }
    
    private GameObject MainInterface;
    public void Back()
    {
        //MissionDiscribe.text = "Hello, here writing the discription";
        WeakUpMainInterface();
        this.gameObject.SetActive(false);
    }
    
    private void WeakUpMainInterface()
    {
        MainInterface = this.transform.parent.Find("MainInterface").gameObject;
        MainInterface.SetActive(true);
        
    }
}
