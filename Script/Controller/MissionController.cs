using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
/*关于任务数据库
 * MissionID任务编号，唯一，与对话编号对应，每个任务都不同。
 * NPCName，接取任务的NPC名字，在对应NPC可以接任务。
 * NPCFinish，提交人物的NPC名字，当State为3的时候可以在这个NPC提交任务获得奖励。
 * MissionType，任务类型，1为打怪任务，2为收集任务，3为对话任务。
 * TriggerDay，可以触发任务的日期，只有当前日期大于这个值才可以触发任务。
 * State，任务状态，0为未接取，1为已完成，2为进行中，3为待结算。
 * State为0且当前日期大于TriggerDay任务可以被接取，接取后State设为2，并且加入MissionProcessingList。
 * State为1时在任何时候都不可以被接取，State为2的时候任务正在进行中，任何State为2的任务都会在初始化的时候加入MissionProcessingList。
 * State为3时找到NPCFinish就可以提交任务
 * MissionDescription是任务描述，在UI中更新即可
 * TargetName为任务目标的名字，如果为打怪任务就是要打的怪，如果是收集任务就是要收集的物品，如果是对话任务就是要对话的人，一般为NPCFinish就可以
 * ProcessingCount为任务进度，即目前打了多少怪，收集了多少物品，对话的话直接为1就可以，当ProcessingCount等于TargetCount的时候就可以把State设为3
 * TargetCount为完成任务需要的个数，仅用于判断任务可不可以进入结算阶段
 * MissionReward为完成任务之后获得的奖励类型
 * MissionRewardNumber为完成任务之后获得的奖励个数
 */


//保存要保存什么？把要设为1的保存了
public class MissionController : MonoBehaviour
{
    public static List<Tuple<string,int>> MissionProcessingList = new List<Tuple<string,int>>();

    public static ArrayList MissionDetailInfo_1 = new ArrayList();//这三个用于存储任务的详细信息
    public static ArrayList MissionDetailInfo_2 = new ArrayList();//【0】MissionID(int)任务编号,【1】MissionType(int)任务类型,【2】TargetName(string)目标名称,【3】ProcessingCount(int)目前计数,【4】TargetCount(int)目标计数
    public static ArrayList MissionDetailInfo_3 = new ArrayList();//一共五项

    public static List<int> Missions_SetToOne = new List<int>();

    private static SQLiteHelper MissionSql;
    public UILabel MissionDiscribe_1;
    public UILabel MissionDiscribe_2;
    public UILabel MissionDiscribe_3;
    private static List<UILabel> MissionUIList = new List<UILabel>();

    private int CurrentListCount = 0;
    private void Start()
    {
        MissionUIList.Add(MissionDiscribe_1);
        MissionUIList.Add(MissionDiscribe_2);
        MissionUIList.Add(MissionDiscribe_3);
        MissionSql = new SQLiteHelper("data source=Missions.db");
        Missions_SetToOne.Clear();
        InitList();
        MissionSql.CloseConnection();
    }
    private void Update()
    {
        MissionFinishChecker();//每次检查任务是否完成
        try
        {
            MissionSql.CloseConnection();
        }
        catch(Exception)
        {

        }
        if(Missions_SetToOne.Count!=0)
        {
            foreach(int MissionID in Missions_SetToOne)
            {
                if(MissionID!=0)
                SetMissionDone(MissionID);
            }
            Missions_SetToOne.Clear();
            Debug.Log("MissionSetToOne Has " + Missions_SetToOne.Count);
        }
        if (CurrentListCount!= MissionProcessingList.Count)//当正在进行的任务列表中的数量发生改变的时候更新UI
        {
            //Debug.Log("Test");
            CurrentListCount = MissionProcessingList.Count;
            UpdateUIMissionDescription();
        }
    }
    
    private static void MissionFinishChecker()//将任务状态设置为待结算
    {
        if (MissionDetailInfo_1.Count != 0)//当MissionDetailInfo有东西的时候进判断
        {
            if ((int)MissionDetailInfo_1[3] == (int)MissionDetailInfo_1[4])
            {
                //更新State为3，待结算
                MissionSql = new SQLiteHelper("data source=Missions.db");
                string MissionID_string = "\'" + (int)MissionDetailInfo_1[0] + "\'";
                MissionSql.UpdateValues("MissionInfo", new string[] { "State" }, new string[] { "3" }, "MissionID", "=", MissionID_string);
                Debug.Log("任务" + MissionDetailInfo_1[0] + "已完成");
                MissionDetailInfo_1.Clear();
                MissionSql.CloseConnection();
            }
        }
        if (MissionDetailInfo_2.Count != 0)
        {
            if ((int)MissionDetailInfo_2[3] == (int)MissionDetailInfo_2[4])
            {
                //同上
                MissionSql = new SQLiteHelper("data source=Missions.db");
                string MissionID_string = "\'" + (int)MissionDetailInfo_2[0] + "\'";
                MissionSql.UpdateValues("MissionInfo", new string[] { "State" }, new string[] { "3" }, "MissionID", "=", MissionID_string);
                Debug.Log("任务" + MissionDetailInfo_2[0] + "已完成");
                MissionDetailInfo_2.Clear();
                MissionSql.CloseConnection();
            }
        }
        if (MissionDetailInfo_3.Count != 0)
        {
            if ((int)MissionDetailInfo_3[3] == (int)MissionDetailInfo_3[4])
            {
                //同上
                MissionSql = new SQLiteHelper("data source=Missions.db");
                string MissionID_string = "\'" + (int)MissionDetailInfo_3[0] + "\'";
                MissionSql.UpdateValues("MissionInfo", new string[] { "State" }, new string[] { "3" }, "MissionID", "=", MissionID_string);
                Debug.Log("任务" + MissionDetailInfo_3[0] + "已完成");
                MissionDetailInfo_3.Clear();
                MissionSql.CloseConnection();
            }
        }
    }
    
    public static int CheckMissionState(int MissionID)
    {
        int State;
        string MissionID_string = "\"" + MissionID + "\"";
        MissionSql = new SQLiteHelper("data source=Missions.db");
        SqliteDataReader reader = MissionSql.ReadTable("MissionInfo", new string[] { "State" }, new string[] { "MissionID" }, new string[] { "=" }, new string[] { MissionID_string });
        reader.Read();
        State = reader.GetInt32(reader.GetOrdinal("State"));
        MissionSql.CloseConnection();
        return State;
    }

    public static void SetMissionDone(int MissionID)//将任务设为1，即已经完成永远不可读
    {
        string MissionID_string = "\"" + MissionID + "\"";
        MissionSql = new SQLiteHelper("data source=Missions.db");
        //Debug.Log(MissionID_string);
        
        SqliteDataReader reader = MissionSql.ReadTable("MissionInfo", new string[] { "NPCName", "MissionDescription" }, new string[] { "MissionID" }, new string[] { "=" }, new string[] { MissionID_string });
        reader.Read();

        
        MissionProcessingList.Remove(new Tuple<String, int>(reader.GetString(reader.GetOrdinal("NPCName")), MissionID));
        string MissionDescription = reader.GetString(reader.GetOrdinal("MissionDescription"));
        for (int i = 0; i < 3; i++)//伪更新，实际上只是把内容设置为空了
        {
            if (MissionUIList[i].text == MissionDescription)
            {
                MissionUIList[i].text = "";
            }
        }
        MissionDetailClear(MissionID);
        MissionSql.UpdateValues("MissionInfo", new string[] { "State" }, new string[] { "1" }, "MissionID", "=", MissionID_string);
        MissionSql.CloseConnection();
    }//会发生死锁

    public static void UpdateUIMissionDescription()
    {
        MissionSql = new SQLiteHelper("data source=Missions.db");
        SqliteDataReader reader;
        int Counter = 0;
        foreach (Tuple<string, int> kvp in MissionProcessingList)
        {
            string MissionID_string = "\"" + kvp.Item2 + "\"";
            reader = MissionSql.ReadTable("MissionInfo", new string[] { "MissionDescription" }, new string[] { "MissionID" }, new string[] { "=" }, new string[] { MissionID_string });
            reader.Read();
            MissionUIList[Counter].text = reader.GetString(reader.GetOrdinal("MissionDescription"));
            //Debug.Log(MissionUIList[Counter]);
            Counter++;
        }
        MissionSql.CloseConnection();
    }
    private static ArrayList MissionInfoArrayListChooser()//用于挑选空了的MissionDetailInfo 
    {
        if(MissionDetailInfo_1.Count == 0)
        {
            return MissionDetailInfo_1;
        }
        else if(MissionDetailInfo_2.Count == 0)
        {
            return MissionDetailInfo_2;
        }
        else
        {
            return MissionDetailInfo_3;
        }
    }
    private static void MissionDetailClear(int MissionID)
    {
        if(MissionDetailInfo_1.Count!=0&&(int)MissionDetailInfo_1[0] == MissionID)
        {
            MissionDetailInfo_1.Clear();
        }
        if (MissionDetailInfo_2.Count!=0&&(int)MissionDetailInfo_2[0] == MissionID)
        {
            MissionDetailInfo_2.Clear();
        }
        if (MissionDetailInfo_3.Count!=0&&(int)MissionDetailInfo_3[0] == MissionID)
        {
            MissionDetailInfo_3.Clear();
        }
    }
    public static void SetItemState(int MissionID)//用于更改Mission状态，从未执行变为执行中
    {
        string MissionID_string = "\'" + MissionID + "\'";
        MissionSql.UpdateValues("MissionInfo", new string[] { "State" }, new string[] { "2" }, "MissionID", "=", MissionID_string);
    }
    public static int AddItemToList(string NPCName,int MissionID)
    {
        MissionSql = new SQLiteHelper("data source=Missions.db");
        if (MissionProcessingList.Count>=3)
        {
            //从这个地方开始拦截，尚需修改
            Debug.Log("任务已满！");
            MissionSql.CloseConnection();
            return 2;
        }
        else
        {
            //搜索相关任务，如果有就返回1
            string NPCName_string = "\'" + NPCName + "\'";
            string MissionID_string = "\"" + MissionID + "\"";
            SqliteDataReader reader = MissionSql.ReadTable("MissionInfo", new string[] { "MissionID", "NPCName","MissionType","TargetName","ProcessingCount","TargetCount" }, new string[] { "NPCName", "State","TriggerDay" }, new string[] { "=", "=","<=" }, new string[] { NPCName_string, "0","\""+BasicInformation.Day+"\"" });//此处记得加上时间判断，用于筛选出改NPC可行的任务
            if (reader.Read())
            {
                
                MissionProcessingList.Add(new Tuple<String, int>(reader.GetString(reader.GetOrdinal("NPCName")), reader.GetInt32(reader.GetOrdinal("MissionID"))));
                SetItemState(reader.GetInt32(reader.GetOrdinal("MissionID")));//改为2
                Debug.Log("MissionID = " + reader.GetInt32(reader.GetOrdinal("MissionID")));
                Debug.Log("任务已添加");
                //在这里更新这个表
                ArrayList MissionDetailInfo = MissionInfoArrayListChooser();//任务完成之后记得clear则会个MissionDetailInfo
                MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("MissionID")));
                MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("MissionType")));
                MissionDetailInfo.Add(reader.GetString(reader.GetOrdinal("TargetName")));
                MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("ProcessingCount")));
                MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("TargetCount")));
                Debug.Log("任务类型为：" + (int)MissionDetailInfo[1]);
                //Debug.Log((int)MissionDetailInfo[1] == 3);
                if((int)MissionDetailInfo[1] == 3)//如果任务为聊天任务直接就进入结算状态
                {
                    Debug.Log("聊天任务");
                    MissionSql.UpdateValues("MissionInfo", new string[] { "State" }, new string[] { "3" }, "MissionID", "=", "\"" + reader.GetInt32(reader.GetOrdinal("MissionID")) + "\"");
                }
                MissionSql.CloseConnection();
                return 1;
            }
            else//没有这个NPC，或者这个NPC在这个日期没有任务
            {
                Debug.Log("没有这个NPC，或者这个NPC在这个日期没有任务");

                MissionSql.CloseConnection();
                return 0;
            }
            //如果没有就返回0
        }
        
    }
    public static Tuple<string, int> FindNPCDialog(string NPCName)//查找任务进行列表中的任务信息
    {
        
        int MissionID = 0;
        try
        {
            foreach(Tuple<String, int> kvp in MissionProcessingList)
            {
                if(kvp.Item1 == NPCName)
                {
                    MissionID = kvp.Item2;
                }
            }
            //如果有这个NPC相关的任务，则返回这个任务ID
        }
        catch
        {
            MissionID = 0;//如果没有则设为0
        }
        return new Tuple<string, int>(NPCName, MissionID);
    }
    public void InitList()//初始化任务列表
    {
        
        SqliteDataReader reader = MissionSql.ReadTable("MissionInfo", new string[] { "MissionID", "NPCName", "MissionType", "TargetName", "ProcessingCount", "TargetCount" }, new string[] { "State" }, new string[] { "=" }, new string[] { "2" });//将进行中的任务重新添加到任务列表中
        while (reader.Read())
        {
            
            MissionProcessingList.Add(new Tuple<String, int>(reader.GetString(reader.GetOrdinal("NPCName")), reader.GetInt32(reader.GetOrdinal("MissionID"))));
            ArrayList MissionDetailInfo = MissionInfoArrayListChooser();//任务完成之后记得clear则会个MissionDetailInfo
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("MissionID")));
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("MissionType")));
            MissionDetailInfo.Add(reader.GetString(reader.GetOrdinal("TargetName")));
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("ProcessingCount")));
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("TargetCount")));
            
        }
        reader = MissionSql.ReadTable("MissionInfo", new string[] { "MissionID", "NPCName", "MissionType", "TargetName", "ProcessingCount", "TargetCount" }, new string[] { "State" }, new string[] { "=" }, new string[] { "3" });//将结算的任务重新添加到任务列表中
        while (reader.Read())
        {
            string NPCName = reader.GetString(reader.GetOrdinal("NPCName"));
            int MissionID = reader.GetInt32(reader.GetOrdinal("MissionID"));

            MissionProcessingList.Add(new Tuple<String, int>(NPCName, MissionID));
            ArrayList MissionDetailInfo = MissionInfoArrayListChooser();//任务完成之后记得clear则会个MissionDetailInfo
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("MissionID")));
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("MissionType")));
            MissionDetailInfo.Add(reader.GetString(reader.GetOrdinal("TargetName")));
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("ProcessingCount")));
            MissionDetailInfo.Add(reader.GetInt32(reader.GetOrdinal("TargetCount")));
        }
        
    }
    public static int FindFinishMission(string NPCFinish)//判断当前NPC有没有结算的任务
    {
        MissionSql = new SQLiteHelper("data source=Missions.db");
        string Mission_string;
        string NPCName_string;//传入的是用于判断有无结算的NPC，所以是NPCFinish
        SqliteDataReader reader;
        foreach(Tuple<String, int> kvp in MissionProcessingList)
        {
            Mission_string = "\"" + kvp.Item2 + "\"";
            NPCName_string = "\'" + NPCFinish + "\'";
            Debug.Log(NPCFinish +" and "+ kvp.Item2);
            reader = MissionSql.ReadTable("MissionInfo", new string[] { "State" }, new string[] { "MissionID", "NPCFinish", "State" }, new string[] { "=", "=", "=" }, new string[] { Mission_string, NPCName_string, "3" });
            try
            {
                reader.Read();
                if (reader.GetInt32(reader.GetOrdinal("State")) == 3)
                {
                    MissionProcessingList.Remove(kvp);
                    return kvp.Item2;//返回任务ID
                }
            }
            catch(Exception)
            {
                //Debug.Log("Nope");
            }
        }
        MissionSql.CloseConnection();
        return 0;
    }
    public static Tuple<string,int> GetRewardInfo(int Mission)
    {
        MissionSql.CloseConnection();
        String Mission_string = "\"" + Mission + "\"";
        MissionSql = new SQLiteHelper("data source=Missions.db");
        SqliteDataReader reader;
        reader = MissionSql.ReadTable("MissionInfo", new string[] { "MissionReward", "MissionRewardNumber" }, new string[] { "MissionID" }, new string[] { "=" }, new string[] { Mission_string });

        reader.Read();
        string MissionReward = reader.GetString(reader.GetOrdinal("MissionReward"));
        int MissionRewardNumber = reader.GetInt32(reader.GetOrdinal("MissionRewardNumber"));
        Tuple<string, int> MissionRewardInfo = new Tuple<string, int>(MissionReward, MissionRewardNumber);
        MissionSql.CloseConnection();
        return MissionRewardInfo;
    }
    public static void CancelTheMission(string NPCName,string MissionID_string)
    {
        
        MissionProcessingList.Remove(new Tuple<String, int>(NPCName, Convert.ToInt32(MissionID_string)));

        MissionSql.CloseConnection();
        MissionSql = new SQLiteHelper("data source=Missions.db");
        

        SqliteDataReader reader = MissionSql.ReadTable("MissionInfo", new string[] { "NPCName", "State","MissionDescription" }, new string[] { "MissionID" }, new string[] { "=" }, new string[] { MissionID_string });
        reader.Read();

        if (reader.GetInt32(reader.GetOrdinal("State")) != 1)
        {
            //MissionProcessingList.Remove(reader.GetString(reader.GetOrdinal("NPCName")));
            string MissionDescription = reader.GetString(reader.GetOrdinal("MissionDescription"));
            for (int i = 0; i < 3; i++)//伪更新，实际上只是把内容设置为空了
            {
                if (MissionUIList[i].text == MissionDescription)
                {
                    MissionUIList[i].text = "";
                }
            }
            DialogController.ResetDialogsToAble(MissionID_string);
            MissionSql.UpdateValues("MissionInfo", new string[] { "State" }, new string[] { "0" }, "MissionID", "=", MissionID_string);
        }
        MissionSql.CloseConnection();
        Debug.Log("任务已清除");
    }
}
