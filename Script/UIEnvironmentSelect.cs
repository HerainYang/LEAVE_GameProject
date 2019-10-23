using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnvironmentSelect : MonoBehaviour
{
    public GameObject ResourceUI;
    public GameObject LabUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(BasicInformation.NowWhere == 0)
        {
            ResourceUI.SetActive(false);
            LabUI.SetActive(true);
        }
        else
        {
            ResourceUI.SetActive(true);
            LabUI.SetActive(false);
        }
    }
}
