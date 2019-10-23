
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{
    bool Display = true;
    float TimeCounter = 0;
    void Update()
    {
        if(Display&&TimeCounter<1)
        {
            GetComponent<UILabel>().text = "2088/8/"+BasicInformation.Day+"  "+System.DateTime.Now.ToString("hh:mm:ss");
        }
        else if(Display && TimeCounter >= 1)
        {
            GetComponent<UILabel>().enabled = false;
            Display = false;
            TimeCounter = 0;
        }
        else if(!Display && TimeCounter < 1)
        {

        }
        else if(!Display && TimeCounter >= 1)
        {
            GetComponent<UILabel>().enabled = true;
            Display = true;
            TimeCounter = 0;
        }
        TimeCounter += Time.deltaTime;
    }
}
