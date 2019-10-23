using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMinitorChooser : MonoBehaviour
{
    public Texture Moniter_1;
    public Texture Moniter_2;
    public Texture Moniter_3;

    public void Moniter_Left()
    {
        Texture MoniterUI = transform.Find("Monitor_Left").GetComponent<UITexture>().mainTexture;
        Debug.Log(MoniterUI.name);
        string MoniterName = MoniterUI.name;
        CameraFlow.NowMoniter = 1;
    }
    public void Moniter_Mid()
    {
        Texture MoniterUI = transform.Find("Monitor_Mid").GetComponent<UITexture>().mainTexture;
        Debug.Log(MoniterUI.name);
        string MoniterName = MoniterUI.name;
        CameraFlow.NowMoniter = 2;
    }
    public void Moniter_Right()
    {
        Texture MoniterUI = transform.Find("Monitor_Right").GetComponent<UITexture>().mainTexture;
        Debug.Log(MoniterUI.name);
        string MoniterName = MoniterUI.name;
        CameraFlow.NowMoniter = 3;
    }
}
