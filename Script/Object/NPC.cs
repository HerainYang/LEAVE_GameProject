
using UnityEngine;
using System;

public class NPC : MonoBehaviour
{
    private float InteractRange;
    private Collider[] Hits;
    public TweenPosition tweenPosition;
    private GameObject NPCDialog;
    public static string Text;
    private void Start()
    {
        InteractRange = GetThirdLength(this.GetComponent<Collider>().bounds.size.y, GetThirdLength(this.GetComponent<Collider>().bounds.size.x, this.GetComponent<Collider>().bounds.size.z)) / 2;
        NPCDialog = GameObject.Find("NPCDialog");
    }

    // Update is called once per frame
    void Update()
    {
        NPCDialog.GetComponent<UILabel>().text = Text;
        Hits = Physics.OverlapSphere(this.transform.position, InteractRange);
        foreach (Collider Hit in Hits)
        {
            if (Hit.name == "player")
            {
                
                if (!Phone_AppearorDis.isCallOut&&Input.GetKeyDown("f"))
                {
                    transform.LookAt(Hit.transform.position);
                    CallOut();
                    int FinishMission = MissionController.FindFinishMission(this.name);
                    if (FinishMission!=0)
                    {
                        DialogController.SetNPCName(this.name);
                        DialogController.SetFinishedMissionID(FinishMission);
                        Hit.GetComponent<BasicMove>().SetControllable(false);
                    }
                    else
                    {
                        Tuple<string, int> MissionInfo = MissionController.FindNPCDialog(this.name);
                        if (MissionInfo.Item2 != 0)
                        {
                            Debug.Log("任务中对话");
                        }
                        else
                        {
                            Debug.Log("列表中没有这个任务");

                            switch (MissionController.AddItemToList(this.name, 100))
                            {
                                case 0:
                                    {
                                        Debug.Log("闲聊");
                                        break;
                                    }
                                case 1:
                                    {
                                        Debug.Log("任务对话");
                                        break;
                                    }
                                case 2:
                                    {
                                        Debug.Log("任务已满");
                                        break;
                                    }
                            }
                            //如果可以添加任务，就开始任务对话
                            //如果拒绝任务，就从列表中移除
                            //如果接受任务，修改任务状态
                            //如果添加失败，就开始闲聊
                        }
                        //Debug.Log("NPC.cs:NPC name is " + this.name);
                        DialogController.SetNPCName(this.name);
                        Hit.GetComponent<BasicMove>().SetControllable(false);
                    }
                }
                else if(Input.GetKeyDown("e"))
                {
                    //Debug.Log("对话结束");
                    CallBack();
                    Hit.GetComponent<BasicMove>().SetControllable(true);
                }
                if(DialogController.IsFinish == true)
                {
                    CallBack();
                    Hit.GetComponent<BasicMove>().SetControllable(true);
                    DialogController.IsFinish = false;
                }
            }
        }
    }
    public void CallOut()
    {

        tweenPosition.PlayForward();
    }
    public void CallBack()
    {
        tweenPosition.PlayReverse();
    }
    public static void SetNPCDialogText(string Text_in)
    {
        Text = Text_in;
    }
    
    private float GetThirdLength(float FirstLength, float SecondLength)
    {
        return Mathf.Sqrt(FirstLength * FirstLength + SecondLength * SecondLength);
    }
    
    
}
