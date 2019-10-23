using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceBehavior : MonoBehaviour
{
    Collider[] Hits;
    private float InteractRange;
    public static SQLiteHelper PackageDB;
    public float RemainTime = 15;//物体停留时间
    private float TimeCounter = 0;
    private float GetThirdLength(float FirstLength, float SecondLength)
    {
        return Mathf.Sqrt(FirstLength * FirstLength + SecondLength * SecondLength);
    }
    private void Start()
    {
        InteractRange = GetThirdLength(this.GetComponent<BoxCollider>().bounds.size.y, GetThirdLength(this.GetComponent<Collider>().bounds.size.x, this.GetComponent<Collider>().bounds.size.z)) / 2;
        
    }

    void Update()
    {
        TimeCounter += Time.deltaTime;
        if(TimeCounter>=RemainTime)
        {
            Destroy(this.gameObject);
        }
        Hits = Physics.OverlapSphere(this.transform.position, InteractRange);
        try
        {
            foreach (Collider Hit in Hits)
            {
                if (Hit.name == "player")
                {
                    if (Input.GetKeyDown("f"))
                    {
                        
                        GameObject player = GameObject.Find("player");
                        List<string> list = player.GetComponent<BasicMove>().Package;
                        list.Add(this.name);
                        ResourceController.ObjectUpdate(this.name);
                        /*
                        if(this.name == "GrenadeBullet"||this.name == "PistolBullet")
                        {
                            Shoot.NeedToUpdateBulletNumber = true;
                        }
                        */
                        Debug.Log("你得到了" + this.name);
                        MissionChecker();
                        DestroyImmediate(this.gameObject);

                    }
                }
            }
        }
        catch(System.Exception)
        {
            //只是不想看到这里这个无所谓的报错
        }
    }
    private void MissionChecker()
    {
        if (MissionController.MissionDetailInfo_1.Count != 0 && (int)MissionController.MissionDetailInfo_1[1] == 2)//有任务且人物类型为0，并且为收集任务
        {
            Debug.Log("需要：" + (string)MissionController.MissionDetailInfo_1[2]);
            if ((string)MissionController.MissionDetailInfo_1[2] == this.tag && (int)MissionController.MissionDetailInfo_1[3] != (int)MissionController.MissionDetailInfo_1[4])
            {
                Debug.Log("+1");
                MissionController.MissionDetailInfo_1[3] = (int)MissionController.MissionDetailInfo_1[3] + 1;
            }
            else
            {
                Debug.Log("None");
            }
        }

        if (MissionController.MissionDetailInfo_2.Count != 0 && (int)MissionController.MissionDetailInfo_2[1] == 2)
        {
            Debug.Log("需要：" + (string)MissionController.MissionDetailInfo_2[2]);
            if ((string)MissionController.MissionDetailInfo_2[2] == this.tag && (int)MissionController.MissionDetailInfo_2[3] != (int)MissionController.MissionDetailInfo_2[4])
            {
                Debug.Log("+1");
                MissionController.MissionDetailInfo_2[3] = (int)MissionController.MissionDetailInfo_2[3] + 1;
            }
        }

        if (MissionController.MissionDetailInfo_3.Count != 0 && (int)MissionController.MissionDetailInfo_3[1] == 2)
        {
            Debug.Log("需要：" + (string)MissionController.MissionDetailInfo_3[2]);
            Debug.Log("inside MissionChecker:" + this.tag);
            Debug.Log((string)MissionController.MissionDetailInfo_1[2] == this.tag);
            if ((string)MissionController.MissionDetailInfo_3[2] == this.tag && (int)MissionController.MissionDetailInfo_3[3] != (int)MissionController.MissionDetailInfo_3[4])
            {
                Debug.Log("+1");
                MissionController.MissionDetailInfo_3[3] = (int)MissionController.MissionDetailInfo_3[3] + 1;
            }
        }
    }

}
