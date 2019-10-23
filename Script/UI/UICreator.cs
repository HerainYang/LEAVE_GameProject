using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICreator : MonoBehaviour
{
    private UISprite sprite;
    public UICreator(GameObject Parent, UIAtlas atlas, Font f, int Order)
    {
        
        sprite = NGUITools.AddSprite(Parent, atlas, "UI_AppsIcon_Phone");
        sprite.MakePixelPerfect();
        sprite.name = System.Convert.ToString(Order);
        sprite.transform.localPosition = new Vector3(-7, 341 - 75 * Order, 0);
        sprite.height = 51;
        sprite.width = 325;

        sprite.gameObject.AddComponent<BoxCollider>();
        sprite.gameObject.GetComponent<BoxCollider>().isTrigger = true;
        sprite.gameObject.GetComponent<BoxCollider>().size = new Vector3(325, 51, 0);
        sprite.gameObject.AddComponent<UIButton>();
        sprite.gameObject.AddComponent<TweenColor>();
        

        UIButton icon = sprite.gameObject.GetComponent<UIButton>();
        icon.tweenTarget = sprite.gameObject;
        icon.normalSprite = "UI_AppsIcon_Phone";
        icon.hoverSprite = "UI_AppsIcon_Phone_PressDown";
        icon.pressedSprite = "UI_AppsIcon_Phone";
        icon.hover = Color.white;
        icon.pressed = Color.white;
        icon.onClick.Add(new EventDelegate(SetFunction));
        

        UILabel label = NGUITools.AddChild<UILabel>(sprite.gameObject);
        label.trueTypeFont = f;
        label.depth = sprite.depth + 1;
        label.name = "Label";
        label.text = "Day" + Order;
        label.height = 34;
        label.width = 282;
        label.fontSize = 30;
        label.applyGradient = true;
        label.gradientBottom = Color.black;
        label.gradientTop = Color.black;
    }
    public void SetFunction()
    {
        GameObject BackGround = sprite.parent.parent.parent.gameObject;
        BackGround.transform.Find("LetterInterface").gameObject.SetActive(false);
        GameObject LetterDisplay = BackGround.transform.Find("LetterDisplay").gameObject;
        LetterDisplay.SetActive(true);
        UILabel Date = LetterDisplay.transform.Find("Date").gameObject.GetComponent<UILabel>();
        Date.text = "Day" + sprite.name;
        UILabel SendContent = LetterDisplay.transform.Find("SenderContent").gameObject.transform.Find("Label").gameObject.GetComponent<UILabel>();
        UILabel ReceivedContent = LetterDisplay.transform.Find("ReceivedContent").gameObject.transform.Find("Label").gameObject.GetComponent<UILabel>();
        try
        {
            SendContent.text = GetSendContent();
            //Debug.Log(SendContent.text);
        }
        catch(System.Exception)
        {
            SendContent.text = "暂未收到来信";
        }
        try
        {
            ReceivedContent.text = GetReceivedContent();
        }
        catch (System.Exception)
        {
            ReceivedContent.text = "暂未收到来信";
        }
        Letter_DB.CloseTheDataBase();
    }
    private string GetSendContent()
    {
        return Letter_DB.GetContent(int.Parse(sprite.name), "灰");
    }
    private string GetReceivedContent()
    {
        return Letter_DB.GetContent(int.Parse(sprite.name), "青");
    }
}
