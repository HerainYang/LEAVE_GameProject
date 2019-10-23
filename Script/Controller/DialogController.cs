using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public TweenPosition PlayerDialogItemPosition;
    public TweenPosition NPCDialogItemPosition;
    public TweenColor PlayerDialogItemColor;
    public TweenColor NPCDialogItemColor;
    public static bool IsFinish = false;
    private static SQLiteHelper DialogSql;
    private static string NPCName;
    private static int FinishedMissionID;
    private bool TurnToChange = false;//false为NPC对话改变，反之为Player对话改变
    private List<string> NPCDialogList = new List<string>();
    private List<string> PlayerDialogList = new List<string>();
    private GameObject Player;
    private bool FirstTimeToRead = true;
    

    private void Start()
    {
        
        DialogSql = new SQLiteHelper("data source=Dialogs.db");
        Player = GameObject.Find("player");
    }

    private void Update()
    {
        
        if (!Player.GetComponent<BasicMove>().GetControllable())//人物不能动的时候更新对话，因为这时是对话状态
        {
            if (NPCDialogList.Count <= 1 && PlayerDialogList.Count <= 1 && FirstTimeToRead)//NPCDialogList第一个留着用于存储对话id
            {
                ReFreshData();
                Call(0);//升起NPC对话框
                FirstTimeToRead = false;
                IsFinish = false;

                TurnToChange = true;
                //Debug.Log(NPCDialogList.Count);
                //Debug.Log(PlayerDialogList.Count);
                NPC.SetNPCDialogText(NPCDialogList[1]);//更新一次对话
                NPCDialogList.Remove(NPCDialogList[1]);

            }
            if (Input.GetMouseButtonDown(0))
            {
                if (!TurnToChange&&NPCDialogList.Count>1)
                {
                    Call(0);
                    
                    NPC.SetNPCDialogText(NPCDialogList[1]);
                    NPCDialogList.Remove(NPCDialogList[1]);
                    TurnToChange = true;
                    
                }
                else if(TurnToChange && PlayerDialogList.Count > 1)
                {
                    Call(1);
                    
                    PlayerDialog.SetPlayerDialogText(PlayerDialogList[1]);
                    PlayerDialogList.Remove(PlayerDialogList[1]);
                    TurnToChange = false;
                    
                }

            }
            else if (NPCDialogList.Count <= 1 && PlayerDialogList.Count <= 1 && !FirstTimeToRead)
            {
                if (NPCDialogList[0] != "0")
                {
                    ShowMissionButton();
                }
                else
                {
                    CallReset();
                    IsFinish = true;

                    FirstTimeToRead = true;
                }
            }
            
        }
        else
        {
            CallReset();
        }
    }
    private void Call(int state)//将对话的一方的对话框给抬起来，NPC为0，Player为1
    {
        if(state == 0)
        {
            NPCDialogItemPosition.PlayForward();
            PlayerDialogItemPosition.PlayReverse();
            NPCDialogItemColor.PlayReverse();//恢复颜色
            PlayerDialogItemColor.PlayForward();//变灰色
            
        }
        else
        {
            NPCDialogItemPosition.PlayReverse();
            PlayerDialogItemPosition.PlayForward();
            NPCDialogItemColor.PlayForward();
            PlayerDialogItemColor.PlayReverse();
        }
    }
    private void CallReset()//将对话双方的对话框给降下去
    {
        NPCDialogItemColor.PlayReverse();//恢复颜色
        PlayerDialogItemColor.PlayReverse();//变灰色
        NPCDialogItemPosition.PlayReverse();
        PlayerDialogItemPosition.PlayReverse();
    }


    //IsTrigger 为0的时候代表可以触发，为1代表不能触发，为2代表任务进行时触发，为3代表任务结算时触发
    private List<string> GetPlayerContent(string DialogNPC)//更新Player的对话表
    {

        SqliteDataReader reader;
        List<string> PlayerDialogs = new List<string>();
        Tuple<string, int> MissionInfo = MissionController.FindNPCDialog(DialogNPC);

        string MissionID_string = "\"" + MissionInfo.Item2 + "\"";
        string DialogNPC_string = "\'" + DialogNPC + "\'";
        reader = DialogSql.ReadTable("PlayerDialog", new string[] { "Content" }, new string[] { "ConversationID", "NPC", "IsTriggle" }, new string[] { "=", "=", "=" }, new string[] { MissionID_string, DialogNPC_string, "0" });
        
        PlayerDialogs.Add(MissionInfo.Item2.ToString());
        while(reader.Read())
        {
            PlayerDialogs.Add(reader.GetString(reader.GetOrdinal("Content")));
            
        }
        return PlayerDialogs;
    }
    private List<string> GetNPCContent(string DialogNPC)//更新NPC的对话表
    {
        SqliteDataReader reader;
        List<string> NPCDialogs = new List<string>();
        Tuple<string, int> MissionInfo = MissionController.FindNPCDialog(DialogNPC);
        string MissionID_string = "\"" + MissionInfo.Item2 + "\"";
        string DialogNPC_string = "\'" + DialogNPC + "\'";
        
            reader = DialogSql.ReadTable("NPCDialog", new string[] { "Content" }, new string[] { "ConversationID", "NPC", "IsTriggle" }, new string[] { "=", "=", "=" }, new string[] { MissionID_string, DialogNPC_string, "0" });
         
        NPCDialogs.Add(MissionInfo.Item2.ToString());
        while (reader.Read())
        {
            NPCDialogs.Add(reader.GetString(reader.GetOrdinal("Content")));
            //Debug.Log("NPC:" + reader.GetString(reader.GetOrdinal("Content")));
        }
        return NPCDialogs;
    }
    private List<string> GetProcessingPlayerContent(string DialogNPC)//更新任务正在进行中的Player对话表
    {

        SqliteDataReader reader;
        List<string> PlayerDialogs = new List<string>();
        Tuple<string, int> MissionInfo = MissionController.FindNPCDialog(DialogNPC);
        string MissionID_string = "\"" + MissionInfo.Item2 + "\"";
        string DialogNPC_string = "\'" + DialogNPC + "\'";

        reader = DialogSql.ReadTable("PlayerDialog", new string[] { "Content" }, new string[] { "ConversationID", "NPC", "IsTriggle" }, new string[] { "=", "=", "=" }, new string[] { MissionID_string, DialogNPC_string, "2" });

        PlayerDialogs.Add(MissionInfo.Item2.ToString());
        while (reader.Read())
        {
            PlayerDialogs.Add(reader.GetString(reader.GetOrdinal("Content")));

        }
        return PlayerDialogs;
    }
    private List<string> GetProcessingNPCContent(string DialogNPC)//更新任务正在进行中的NPC的对话表
    {
        SqliteDataReader reader;
        List<string> NPCDialogs = new List<string>();
        Tuple<string, int> MissionInfo = MissionController.FindNPCDialog(DialogNPC);
        string MissionID_string = "\"" + MissionInfo.Item2 + "\"";
        string DialogNPC_string = "\'" + DialogNPC + "\'";

        reader = DialogSql.ReadTable("NPCDialog", new string[] { "Content" }, new string[] { "ConversationID", "NPC", "IsTriggle" }, new string[] { "=", "=", "=" }, new string[] { MissionID_string, DialogNPC_string, "2" });

        NPCDialogs.Add(MissionInfo.Item2.ToString());
        while (reader.Read())
        {
            NPCDialogs.Add(reader.GetString(reader.GetOrdinal("Content")));
            //Debug.Log("NPC:" + reader.GetString(reader.GetOrdinal("Content")));
        }
        return NPCDialogs;
    }
    private List<string> GetFinishedNPCContent(string DialogNPC)//更新任务结算的NPC的对话表
    {
        SqliteDataReader reader;
        List<string> NPCDialogs = new List<string>();
        string MissionID_string = "\"" + FinishedMissionID + "\"";
        string DialogNPC_string = "\'" + DialogNPC + "\'";
        reader = DialogSql.ReadTable("NPCDialog", new string[] { "Content" }, new string[] { "ConversationID", "NPC", "IsTriggle" }, new string[] { "=", "=", "=" }, new string[] { MissionID_string, DialogNPC_string, "3" });
        NPCDialogs.Add(FinishedMissionID.ToString());
        while (reader.Read())
        {
            NPCDialogs.Add(reader.GetString(reader.GetOrdinal("Content")));
            //Debug.Log("NPC:" + reader.GetString(reader.GetOrdinal("Content")));
        }
        DialogSql.ExecuteQuery("UPDATE PlayerDialog SET IsTriggle = 1 WHERE ConversationID = " + FinishedMissionID + " AND IsTriggle = 3");//这两句要单独拿出来，只有触发了Accept行为才会修改
        DialogSql.ExecuteQuery("UPDATE NPCDialog SET IsTriggle = 1 WHERE ConversationID = " + FinishedMissionID + " AND IsTriggle = 3");//Accept行为需要包括两个修改
        return NPCDialogs;
    }
    private List<string> GetFinishedPlayerContent(string DialogNPC)//更新任务结算的Player对话表
    {
        
        SqliteDataReader reader;
        List<string> PlayerDialogs = new List<string>();
        string MissionID_string = "\"" + FinishedMissionID + "\"";
        string DialogNPC_string = "\'" + DialogNPC + "\'";
        //Debug.Log(MissionID_string +" and "+ DialogNPC_string+" to find Content");
        reader = DialogSql.ReadTable("PlayerDialog", new string[] { "Content" }, new string[] { "ConversationID", "NPC", "IsTriggle"}, new string[] { "=", "=", "=" }, new string[] { MissionID_string, DialogNPC_string, "3" });
        PlayerDialogs.Add(FinishedMissionID.ToString());
        //Debug.Log(PlayerDialogs.Count);
        while (reader.Read())
        {
            PlayerDialogs.Add(reader.GetString(reader.GetOrdinal("Content")));
        }
        //Debug.Log(PlayerDialogs.Count);
        //Debug.Log(DialogNPC);

        if (PlayerDialogs.Count > 1)
        {
            Tuple<string, int> MissionRewardInfo = MissionController.GetRewardInfo(FinishedMissionID);//获取任务奖励信息，奖励名字，奖励个数
            ResourceController.UpdateByObjectNameAndCount(MissionRewardInfo.Item1, MissionRewardInfo.Item2);
        }
        

        MissionController.Missions_SetToOne.Add(FinishedMissionID);//传的是string处理过的MissionID
        Debug.Log("Mission " + FinishedMissionID + " is set to done");
        

        return PlayerDialogs;
    }

    public static void SetNPCName(string Name)
    {
        NPCName = Name;
    }
    public static void SetFinishedMissionID(int MissionID)
    {
        FinishedMissionID = MissionID;
    }
    public void ReFreshData()//刷新对话
    {
        DialogSql = new SQLiteHelper("data source=Dialogs.db");


        PlayerDialogList = GetFinishedPlayerContent(NPCName);//先看有没有结算对话
        NPCDialogList = GetFinishedNPCContent(NPCName);
        if (NPCDialogList.Count <= 1 && PlayerDialogList.Count <= 1)//再看有没有任务对话
        {
            PlayerDialogList = GetPlayerContent(NPCName);//两者直接引用传出的数据，所以第0项持续更新
            NPCDialogList = GetNPCContent(NPCName);
            Debug.Log("查找任务对话");
            if (NPCDialogList.Count <= 1 && PlayerDialogList.Count <= 1)//最后看有没有任务执行中对话
            {
                PlayerDialogList = GetProcessingPlayerContent(NPCName);
                NPCDialogList = GetProcessingNPCContent(NPCName);
                Debug.Log("查找执行任务中对话");
            }
        }
        DialogSql.CloseConnection();
    }
    public static void ResetDialogsToAble(string MissionID_string)
    {
        DialogSql = new SQLiteHelper("data source=Dialogs.db");
        Debug.Log(MissionID_string);
        DialogSql.ExecuteQuery("UPDATE NPCDialog SET IsTriggle = 0 WHERE ConversationID = " + MissionID_string + " AND IsTriggle = 1");
        DialogSql.ExecuteQuery("UPDATE PlayerDialog SET IsTriggle = 0 WHERE ConversationID = " + MissionID_string + " AND IsTriggle = 1");
        DialogSql.CloseConnection();
    }
    public void ShowMissionButton()
    {
        GameObject.Find("DialogBox").transform.Find("AcceptButton").gameObject.SetActive(true);
        GameObject.Find("DialogBox").transform.Find("CancelButton").gameObject.SetActive(true);
    }
    public void AcceptButton()
    {
        //Debug.Log("Accept the mission");
        GameObject.Find("DialogBox").GetComponent<TweenPosition>().PlayReverse();
        GameObject.Find("DialogBox").transform.Find("AcceptButton").gameObject.SetActive(false);
        GameObject.Find("DialogBox").transform.Find("CancelButton").gameObject.SetActive(false);
        DialogSql = new SQLiteHelper("data source=Dialogs.db");
        DialogSql.ExecuteQuery("UPDATE PlayerDialog SET IsTriggle = 1 WHERE ConversationID = " + NPCDialogList[0] + " AND IsTriggle = 0");//这两句要单独拿出来，只有触发了Accept行为才会修改
        DialogSql.ExecuteQuery("UPDATE NPCDialog SET IsTriggle = 1 WHERE ConversationID = " + NPCDialogList[0] + " AND IsTriggle = 0");//Accept行为需要包括两个修改
        DialogSql.CloseConnection();
        IsFinish = true;
        FirstTimeToRead = true;
        CallReset();
        Player.GetComponent<BasicMove>().SetControllable(true);
    }
    public void CancelButton()
    {
        //Debug.Log("Cancel the mission");
        GameObject.Find("DialogBox").GetComponent<TweenPosition>().PlayReverse();
        GameObject.Find("DialogBox").transform.Find("AcceptButton").gameObject.SetActive(false);
        GameObject.Find("DialogBox").transform.Find("CancelButton").gameObject.SetActive(false);
        MissionController.CancelTheMission(NPCName, NPCDialogList[0]);
        IsFinish = true;
        FirstTimeToRead = true;
        CallReset();
        Player.GetComponent<BasicMove>().SetControllable(true);
    }
}
