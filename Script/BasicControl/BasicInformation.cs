using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicInformation : MonoBehaviour
{
    public static int Health = 7;//整个游戏的体力
    public static float HPConst = 100;
    public static float HP;//单局资源区模式的体力
    public static float IntellectConst = 600;//600合适
    public static float Intellect;//单局资源区模式的耐久值
    public static int Day = 0;
    private int DayCurrent;
    public float TimeCounter;
    private int TimeCounterHelper;
    public GameObject UI_Day;
    public static int NowWhere = 0;//现在的位置在哪，实验室为0，沙漠为1，森林为2，城市为3
    public static bool FirstEnterResourceArea = true;

    public GameObject NewDayAnimation;
    private const float AnimationRemainCD = 2;
    private float AnimationTimeCounter;

    public static bool JustEnterLabArea = false;
    public static bool JustEnterResourceArea = false;
    public static bool IsDie = false;
    public static bool HealthIsAdd = false;

    public static GameObject player;
    public static GameObject DesertStartPosition;
    public static GameObject LabStartPosition;
    
    public GameObject SurviveGuide;
    public GameObject SurviveBoard;

    public GameObject UIHealth;

    public AudioSource Audio_Desert;
    public AudioSource Audio_Lab_1;
    public AudioSource Audio_Lab_2;
    public GameObject KnifeEffect;


    public GameObject AnteAlarm;
    public GameObject GerrardAlarm;
    public GameObject KanterAlarm;


    private void Start()
    {
        HP = HPConst;
        Intellect = IntellectConst;
        TimeCounter = 0;
        TimeCounterHelper = 0;
        DayCurrent = Day;
        AnimationTimeCounter = 0;
        player = this.gameObject;
        DesertStartPosition = GameObject.Find("DesertStartPoint");
        LabStartPosition = GameObject.Find("LabStartPoint");
        NPCAlarmUpdate(0);
    }
    private void Update()
    {
        if(transform.position.y<=0)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        }
        if(NowWhere == 0)
        {
            Audio_Desert.enabled = false;
            Audio_Lab_1.enabled = true;
            Audio_Lab_2.enabled = true;
            KnifeEffect.SetActive(false);
            RenderSettings.fog = false;
        }
        else
        {
            Audio_Desert.enabled = true;
            Audio_Lab_1.enabled = false;
            Audio_Lab_2.enabled = false;
            RenderSettings.fog = true;
            this.gameObject.GetComponent<BasicMove>().SetControllable(true);
            RenderSettings.fogColor = new Color(0.86f, 0.64f, 0.19f, 1);
        }
        TimeCounter += Time.deltaTime;
        if ((int)TimeCounter > TimeCounterHelper && Intellect > 0 && NowWhere != 0)
        {
            TimeCounterHelper = (int)TimeCounter;
            Intellect -= 5;
            
        }
        else if((int)TimeCounter > TimeCounterHelper && Intellect <= 0)
        {
            TimeCounterHelper = (int)TimeCounter;
            HP -= 1;
        }
        UI_Day.GetComponent<UILabel>().text = Convert.ToString(Day);

        if(Day!=DayCurrent)
        {
            NewDayAnimation.SetActive(true);
            NewDayAnimation.transform.Find("Label").GetComponent<UILabel>().text = "Day " + Day;
            NewDayAnimation.transform.Find("Label").GetComponent<UILabel>().color = Color.white;
            DayCurrent = Day;
            FirstEnterResourceArea = true;
            UpdateUIHealth();
            NPCAlarmUpdate(Day);
            if (Day <= 15 && 0 < Day)
            {
                DailyLetterController.ShowLetter();
            }
            if(Day==0)
            {
                DailyLetterController.HideLetter();
            }
            
        }
        else if(JustEnterResourceArea)
        {
            NewDayAnimation.SetActive(true);

            NewDayAnimation.transform.Find("Label").GetComponent<UILabel>().color = Color.yellow;
            NewDayAnimation.transform.Find("Label").GetComponent<UILabel>().text = "沙漠区";
            SurviveBoard.SetActive(true);
            SurviveGuide.GetComponent<UILabel>().text = "放下木头然后点火可以制造篝火恢复体力，喝水可以恢复理智";
            
            //this.GetComponent<BoxCollider>().center = new Vector3(this.GetComponent<BoxCollider>().center.x, 1.24f, this.GetComponent<BoxCollider>().center.y);
            JustEnterResourceArea = false;
        }
        else if(IsDie)
        {
            NewDayAnimation.SetActive(true);
            SurviveBoard.SetActive(false);

            NewDayAnimation.transform.Find("Label").GetComponent<UILabel>().color = Color.red;
            NewDayAnimation.transform.Find("Label").GetComponent<UILabel>().text = "重伤";
            
            IsDie = false;
            EnterLabArea();
            Health -= 1;
            UpdateUIHealth();

            Debug.Log("Die!");
            
        }

        if (NewDayAnimation.activeInHierarchy && AnimationTimeCounter <= AnimationRemainCD)
        {
            AnimationTimeCounter += Time.deltaTime;
        }
        else if (NewDayAnimation.activeInHierarchy && AnimationTimeCounter > AnimationRemainCD)
        {
            AnimationTimeCounter = 0;
            //NewDayAnimation.transform.Find("Label").GetComponent<UILabel>().color = Color.yellow;
            NewDayAnimation.SetActive(false);
            HP = HPConst;
            Intellect = IntellectConst;
        }
        if(HealthIsAdd)
        {
            UpdateUIHealth();
            HealthIsAdd = false;
        }
    }
    public void UpdateUIHealth()
    {
        UIHealth.GetComponent<UILabel>().text = Convert.ToString(Health) + "/7";
    }
    public static void NewDay()
    {
        Day++;
        Health -= 1;
        ResourceController.NewDaySettleAccount();
        ResourceController.UpdateByObjectNameAndCount("PistolBullet", 200);
        ResourceController.UpdateByObjectNameAndCount("GrenadeBullet", 200);
    }
    public static void EnterResourceArea()
    {

            FirstEnterResourceArea = false;
            JustEnterResourceArea = true;
            NowWhere = 1;

            player.transform.position = DesertStartPosition.transform.position;
            player.GetComponent<NavMeshAgent>().enabled = true;
                    
    }
    public static void EnterLabArea()
    {
            JustEnterLabArea = true;
        player.transform.position = LabStartPosition.transform.position + Vector3.up;
        NowWhere = 0;
        
        player.GetComponent<NavMeshAgent>().enabled = false;
        //player.GetComponent<BoxCollider>().center = new Vector3(player.GetComponent<BoxCollider>().center.x, 0.79f, player.GetComponent<BoxCollider>().center.y);
        
    }
    
    public static void SetPlace(int Place)
    {
        NowWhere = Place;
        HP = HPConst;
        Intellect = IntellectConst;
    }
    public static int GetPlace()
    {
        return NowWhere;
    }
    public static float GetRateHP()
    {
        return HP / HPConst;
    }
    public static float GetRateIntellect()
    {
        return Intellect / IntellectConst;
    }
    public static void Damage(int Value)
    {
        HP -= Value;
    }
    public static float GetRemainHP()
    {
        return HP;
    }
    public static void AddHP()
    {
        if (HP + 1 <= HPConst)
            HP += 1;
        else
            HP = HPConst;
    }

    public void NPCAlarmUpdate(int Day)
    {
        switch(Day)
        {
            case 0:
                {
                    GerrardAlarm.transform.Find("Label").GetComponent<UILabel>().text = "杰拉德似乎找你有事";
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(true);
                    break;
                }
                
            case 1:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(false);
                    break;
                }
            case 2:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(true);
                    break;
                }
            case 3:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(false);
                    break;
                }
            case 4:
                {
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(false);
                    break;
                }
            case 5:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(false);
                    break;
                }
            case 6:
                {
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(false);
                    break;
                }
            case 7:
                {
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(false);
                    KanterAlarm.SetActive(true);
                    break;
                }
            case 8:
                {
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(false);
                    KanterAlarm.SetActive(true);
                    break;
                }
            case 9:
                {
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(true);
                    break;
                }
            case 10:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(false);
                    KanterAlarm.SetActive(false);
                    break;
                }
            case 11:
                {
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(false);
                    KanterAlarm.SetActive(true);
                    break;
                }
            case 12:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(false);
                    KanterAlarm.SetActive(false);
                    break;
                }
            case 13:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(true);
                    KanterAlarm.SetActive(true);
                    break;
                }
            case 14:
                {
                    AnteAlarm.SetActive(true);
                    GerrardAlarm.SetActive(false);
                    KanterAlarm.SetActive(false);
                    break;
                }
            default:
                {
                    AnteAlarm.SetActive(false);
                    GerrardAlarm.SetActive(true);
                    GerrardAlarm.transform.Find("Label").GetComponent<UILabel>().text = "请尽快和大家把所有事情商量好";
                    KanterAlarm.SetActive(false);
                    break;
                }
        }
    }
}
