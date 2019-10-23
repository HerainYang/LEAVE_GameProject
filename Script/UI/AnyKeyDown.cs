using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyDown : MonoBehaviour
{
    private bool isAnyKeyDown = false;
    private GameObject ButtonContainer;
    // Update is called once per frame
    private void Start()
    {
        ButtonContainer = this.transform.parent.Find("Container").gameObject;
    }
    void Update()
    {
        if(isAnyKeyDown == false)
        {
            if(Input.anyKey)
            {
                ShowButton();
            }
        }
    }
    void ShowButton()
    {
        ButtonContainer.SetActive(true);
        this.gameObject.SetActive(false);
        isAnyKeyDown = true;
    }
}
