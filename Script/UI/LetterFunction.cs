using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterFunction : MonoBehaviour
{
    private const string Buttons = "Buttons";
    private const string Calendar = "Calendar";

    private GameObject MainInterface;
    
    public void Back()
    {
        WeakUpMainInterface();
        this.gameObject.SetActive(false);
        

    }

    private void WeakUpMainInterface()
    {
        MainInterface = this.transform.parent.Find("MainInterface").gameObject;
        MainInterface.SetActive(true);
        
    }

    public void test()
    {
        Debug.Log(this.name);
    }
}
