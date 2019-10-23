using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDialog : MonoBehaviour
{
    private GameObject PlayerDialogs;
    public static bool IsTalking = false;
    public TweenPosition tweenPosition;
    public static string Text;
    private void Start()
    {
        PlayerDialogs = GameObject.Find("PlayerDialog");
    }
    private void Update()
    {
        PlayerDialogs.GetComponent<UILabel>().text = Text;
    }
    public static void SetPlayerDialogText(string Text_in)
    {
        Text = Text_in;
    }
}
