using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Mono.Data.Sqlite;

public class ResourceController : MonoBehaviour
{
    public static GameObject player;

    public static SQLiteHelper PackageInfo;

    public static GameObject Resource_Wood;//将所有资源种类都加载进去，后期设定掉率自由选择
    public UISprite Wood;
    public static GameObject Resource_Metal;
    public UISprite Metal;
    public static GameObject Resource_Stone;
    public UISprite Stone;
    public static GameObject Resource_GrenadeBullet;
    public UISprite GrenadeBullet;
    public static GameObject Resource_PistolBullet;
    public UISprite PistolBullet;
    public static GameObject Resource_Water;
    public UISprite Water;
    public static GameObject Resource_Oil;
    public UISprite Oil;

    public UISprite Pistol;
    public UISprite Grenate;
    public UISprite Light;
    public UISprite Knife;
    public UISprite Serum;

    //public List<UISprite> UISpriteList = new List<UISprite>();
    public static Dictionary<string, UISprite> UISpriteList = new Dictionary<string, UISprite>();

    private static int Seed = 0;
    private void Start()
    {
        player = GameObject.Find("player");
        Resource_Wood = (GameObject)Resources.Load("Prefab/Wood");
        Resource_Metal = (GameObject)Resources.Load("Prefab/Metal");
        Resource_Stone = (GameObject)Resources.Load("Prefab/Stone");
        Resource_GrenadeBullet = (GameObject)Resources.Load("Prefab/GrenadeBullet");
        Resource_PistolBullet = (GameObject)Resources.Load("Prefab/PistolBullet");
        Resource_Water = (GameObject)Resources.Load("Prefab/Water");
        Resource_Oil = (GameObject)Resources.Load("Prefab/oil");
        UISpriteList.Add("Wood", Wood);
        UISpriteList.Add("Metal", Metal);
        UISpriteList.Add("Stone", Stone);
        UISpriteList.Add("GrenadeBullet", GrenadeBullet);
        UISpriteList.Add("PistolBullet", PistolBullet);
        UISpriteList.Add("Water", Water);
        UISpriteList.Add("Oil", Oil);
        UISpriteList.Add("Serum", Serum);
        UISpriteList.Add("Pistol", Pistol);
        UISpriteList.Add("Light", Light);
        UISpriteList.Add("Grenade", Grenate);
        UISpriteList.Add("Knife", Knife);
    }
    public static void UIUpdateObject(string ObjectName)//对单个物品数量进行数据库同步，用于没必要更新全部值的时候
    {
        string Name_string = "\"" + ObjectName + "\"";
        PackageInfo = new SQLiteHelper("data source = Package.db");
        SqliteDataReader reader;
        reader = PackageInfo.ReadTable("PackageInfo", new string[] { "ObjectCount" }, new string[] { "ObjectName" }, new string[] { "=" }, new string[] { Name_string });
        reader.Read();
        UISpriteList[ObjectName].transform.Find("Amount").gameObject.GetComponent<UILabel>().text = reader.GetInt32(reader.GetOrdinal("ObjectCount")).ToString();
        PackageInfo.CloseConnection();
    }
    public static void UIUpdate()//更新所有物品数量
    {
        foreach (KeyValuePair<string, UISprite> Item in UISpriteList)
        {
            string Name = Item.Key;
            //Debug.Log(Name);
            string Name_string = "\"" + Name + "\"";
            PackageInfo = new SQLiteHelper("data source = Package.db");
            SqliteDataReader reader;
            reader = PackageInfo.ReadTable("PackageInfo", new string[] { "ObjectCount" }, new string[] { "ObjectName" }, new string[] { "=" }, new string[] { Name_string });
            reader.Read();
            Item.Value.transform.Find("Amount").gameObject.GetComponent<UILabel>().text = reader.GetInt32(reader.GetOrdinal("ObjectCount")).ToString();
            PackageInfo.CloseConnection();
        }
    }
    
    public static void CreateObject(GameObject Monster, Vector3 Position, Quaternion Rotation)//物品掉率
    {
        CreateWater(Monster, Position, Rotation);
        CreateWood(Monster, Position, Rotation);
        CreateWood(Monster, Position, Rotation);
        CreateWood(Monster, Position, Rotation);
        CreateWood(Monster, Position, Rotation);
        CreateOil(Monster, Position, Rotation);
        CreateMetal(Monster, Position, Rotation);
        CreateStone(Monster, Position, Rotation);
        CreateStone(Monster, Position, Rotation);
        CreateStone(Monster, Position, Rotation);
    }
    private static void CreateMetal(GameObject Monster, Vector3 Position, Quaternion Rotation)
    {
        GameObject Metal = Instantiate(Resource_Metal, Position, Rotation);
        Metal.name = "Metal";
        Metal.transform.parent = Monster.transform.parent;
        Debug.Log("Metal");
    }
    private static void CreateWood(GameObject Monster, Vector3 Position, Quaternion Rotation)
    {
        GameObject Wood = Instantiate(Resource_Wood, Position, Rotation);
        Wood.name = "Wood";
        Wood.transform.parent = Monster.transform.parent;
        Debug.Log("Wood");
    }
    private static void CreateOil(GameObject Monster, Vector3 Position, Quaternion Rotation)
    {
        GameObject Oil = Instantiate(Resource_Oil, Position, Rotation);
        Oil.name = "Oil";
        Oil.transform.parent = Monster.transform.parent;
        Debug.Log("Oil");
    }
    private static void CreateWater(GameObject Monster, Vector3 Position, Quaternion Rotation)
    {
        GameObject Water = Instantiate(Resource_Water, Position, Rotation);
        Water.name = "Water";
        Water.transform.parent = Monster.transform.parent;
        Debug.Log("Water");
    }
    private static void CreateStone(GameObject Monster, Vector3 Position, Quaternion Rotation)
    {
        GameObject Stone = Instantiate(Resource_Stone, Position, Rotation);
        Stone.name = "Stone";
        Stone.transform.parent = Monster.transform.parent;
        Debug.Log("Stone");
    }
    public static void NewDaySettleAccount()
    {
        int Wood_ActuallyTake = TakeResource("Wood", 99, true);
        int Metal_ActuallyTake = TakeResource("Metal", 99, true);
        int Stone_ActuallyTake = TakeResource("Stone", 99, true);
        int Oil_ActuallyTake = TakeResource("Oil", 99, true);

        UpdateByObjectNameAndCount("Serum", Wood_ActuallyTake / 20);
        UpdateByObjectNameAndCount("Water", Stone_ActuallyTake/5);
        UpdateByObjectNameAndCount("PistolBullet", Metal_ActuallyTake * 20);
        UpdateByObjectNameAndCount("GrenadeBullet", Metal_ActuallyTake * 20);
        UpdateByObjectNameAndCount("Water", Oil_ActuallyTake / 2);

        UIUpdate();
    }
    public static void UpdateByObjectNameAndCount(string ObjectName, int Count)//增加物品数量（大于1）
    {
        PackageInfo = new SQLiteHelper("data source = Package.db");
        string ObjectName_string = "\'" + ObjectName + "\'";
        SqliteDataReader reader;
        reader = PackageInfo.ReadTable("PackageInfo", new string[] { "ObjectCount", "ObjectLimitCount" }, new string[] { "ObjectName" }, new string[] { "=" }, new string[] { ObjectName_string });
        reader.Read();
        int ObjectCount = reader.GetInt32(reader.GetOrdinal("ObjectCount"));

        int ObjectLimitCount = reader.GetInt32(reader.GetOrdinal("ObjectLimitCount"));

        int ActualAdd;

        if (ObjectCount + Count > ObjectLimitCount)
        {
            ActualAdd = ObjectLimitCount;
        }
        else
        {
            ActualAdd = ObjectCount + Count;
        }
        string ActualAdd_String = "\"" + ActualAdd + "\"";
        PackageInfo.UpdateValues("PackageInfo", new string[] { "ObjectCount" }, new string[] { ActualAdd_String }, "ObjectName", "=", ObjectName_string);
        PackageInfo.CloseConnection();
    }
    public static void ObjectUpdate(string ObjectName)//增加物品数量（等于1）
    {
        PackageInfo = new SQLiteHelper("data source=Package.db");
        SqliteDataReader reader;
        string ObjectName_string = "\'" + ObjectName + "\'";
        reader = PackageInfo.ReadTable("PackageInfo", new string[] { "ObjectCount", "ObjectLimitCount" }, new string[] { "ObjectName" }, new string[] { "=" }, new string[] { ObjectName_string });
        reader.Read();
        int ObjectCount = reader.GetInt32(reader.GetOrdinal("ObjectCount"));
        string ObjectCount_Add_String = "\"" + (ObjectCount + 1) + "\"";
        int ObjectLimitCount = reader.GetInt32(reader.GetOrdinal("ObjectLimitCount"));
        if (ObjectCount < ObjectLimitCount)
            PackageInfo.UpdateValues("PackageInfo", new string[] { "ObjectCount" }, new string[] { ObjectCount_Add_String }, "ObjectName", "=", ObjectName_string);
        PackageInfo.CloseConnection();
    }
    public static void PlaceWood()//防止木头
    {
        GameObject Wood = Instantiate(Resource_Wood, player.transform.position + player.transform.forward * 1, player.transform.rotation);
        Wood.name = "Wood";
        Wood.transform.parent = player.transform.transform.parent;
    }
    public static void DrinkWater()//喝水
    {
        if(BasicInformation.Intellect + 150>=600)
        {
            BasicInformation.Intellect = 600;
        }
        else
        {
            BasicInformation.Intellect += 150;
        }
    }
    public static void UesSerum()
    {
        if(BasicInformation.Health+1>=7)
        {
            BasicInformation.Health = 7;
        }
        else
        {
            BasicInformation.Health += 1;
        }
    }
    public static int TakeResource(String ResourceName, int TakeNumber, bool MustTake)//MustTake表示一定要取，True代表不够也要取走，False代表不够就不取走
    {
        int ActuallyTake = 0;
        string ResourceName_String = "\'" + ResourceName + "\'";
        SqliteDataReader reader;
        PackageInfo = new SQLiteHelper("data source=Package.db");
        reader = PackageInfo.ReadTable("PackageInfo", new string[] { "ObjectCount" }, new string[] { "ObjectName" }, new string[] { "=" }, new string[] { ResourceName_String });
        reader.Read();
        int AlreadyHave = reader.GetInt32(reader.GetOrdinal("ObjectCount"));
        if (AlreadyHave >= TakeNumber)
        {
            ActuallyTake = TakeNumber;//如果现有数量大于要取数量，就按需求拿走
            int RemainNumber = AlreadyHave - ActuallyTake;
            PackageInfo.UpdateValues("PackageInfo", new string[] { "ObjectCount" }, new string[] { "\"" + RemainNumber + "\"" }, "ObjectName", "=", ResourceName_String);
        }
        else if (AlreadyHave < TakeNumber && MustTake)
        {
            ActuallyTake = AlreadyHave;//如果现有数量小于要取数量，就要多少取多少
            PackageInfo.UpdateValues("PackageInfo", new string[] { "ObjectCount" }, new string[] { "\"" + 0 + "\"" }, "ObjectName", "=", ResourceName_String);
        }
        else
        {
            ActuallyTake = -1;
        }

        PackageInfo.CloseConnection();
        return ActuallyTake;

    }
    public static bool GunShoot(int WeaponType)
    {
        PackageInfo = new SQLiteHelper("data source=Package.db");
        int BulletNumber;
        bool OKToShoot = false;
        switch (WeaponType)//0 Knife,1 Pistol,2 Grenade
        {
            case 0:
                {
                    OKToShoot = true;
                    Debug.Log("Knife");
                    break;
                }
            case 1:
                {
                    string BulletType = "PistolBullet";
                    SqliteDataReader reader;
                    reader = PackageInfo.ReadTable("PackageInfo", new string[] { "ObjectCount" }, new string[] { "ObjectName" }, new string[] { "=" }, new string[] { "\"" + BulletType + "\"" });
                    reader.Read();
                    BulletNumber = reader.GetInt32(reader.GetOrdinal("ObjectCount"));
                    if (BulletNumber <= 0)
                        OKToShoot = false;
                    else
                    {
                        PackageInfo.UpdateValues("PackageInfo", new string[] { "ObjectCount" }, new string[] { "\"" + (BulletNumber - 1) + "\"" }, "ObjectName", "=", "\"" + BulletType + "\"");
                        OKToShoot = true;
                    }
                    Debug.Log(BulletType + OKToShoot);
                    break;
                }
            case 2:
                {
                    string BulletType = "GrenadeBullet";
                    SqliteDataReader reader;
                    reader = PackageInfo.ReadTable("PackageInfo", new string[] { "ObjectCount" }, new string[] { "ObjectName" }, new string[] { "=" }, new string[] { "\"" + BulletType + "\"" });
                    reader.Read();
                    BulletNumber = reader.GetInt32(reader.GetOrdinal("ObjectCount"));
                    if (BulletNumber <= 0)
                        OKToShoot = false;
                    else
                    {
                        PackageInfo.UpdateValues("PackageInfo", new string[] { "ObjectCount" }, new string[] { "\"" + (BulletNumber - 1) + "\"" }, "ObjectName", "=", "\"" + BulletType + "\"");
                        OKToShoot = true;
                    }
                    //Debug.Log(BulletType + OKToShoot);
                    break;
                }
            default:
                {
                    OKToShoot = false;
                    Debug.Log("Default");
                    break;
                }
        }
        PackageInfo.CloseConnection();
        return OKToShoot;
    }
    public static void LightTheWood()
    {
        GameObject Player = GameObject.Find("player");
        
        Collider[] Hits = Physics.OverlapSphere(Player.transform.position, 1);
        foreach(Collider hit in Hits)
        {
            Debug.Log(hit.name);
            if (hit.name == "Wood")
            {
                GameObject Resource_Fire = (GameObject)Resources.Load("Prefab/Bonfire");
                Vector3 Position = hit.gameObject.transform.position;
                Quaternion Rotation = hit.gameObject.transform.rotation;
                Destroy(hit.gameObject);
                GameObject Fire = Instantiate(Resource_Fire, Position, Rotation);
                Fire.name = "Fire";
                Fire.transform.parent = player.transform.parent;
            }
        }
        /*if (Physics.Raycast(ray,out HitInfo,1))
        {
            Debug.Log(HitInfo.collider.name);
            if(HitInfo.collider.name == "Wood")
            {
                GameObject Resource_Fire = (GameObject)Resources.Load("Prefab/Bonfire");
                Vector3 Position = HitInfo.collider.gameObject.transform.position;
                Quaternion Rotation = HitInfo.collider.gameObject.transform.rotation;
                Destroy(HitInfo.collider.gameObject);
                GameObject Fire = Instantiate(Resource_Fire, Position, Rotation);
                Fire.name = "Fire";
                Fire.transform.parent = player.transform.parent;
            }
        }*/
    }
}
