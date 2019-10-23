using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{

    //申请GameObject变量用来承载预制体（拖动预制体赋值）
    public GameObject Bullet;


    public static bool NeedToUpdateBulletNumber = false;
    public static int WeaponState;//Knife为0，Pistol为1，Grenade为2
    //申请float变量设置推动力大小
    public static float Thrust;
    private float TimeCounter = 0;
    public float KnifeLength = 1;
    public static float ShootSpeed;
    private bool Shootable = true;
    private static float ShootRate;
    public Animator ani;

    public GameObject Pistol;
    public GameObject Grenade;
    public GameObject Knife;
    public AudioSource Audio_GunShoot;
    public AudioClip Audio_GunShoot_Source;

    public GameObject KnifeEffect;

    private void Start()
    {
        SetPistolPattern();
        //Bullet = (GameObject)Resources.Load("Prefab/Bullet");
        ShootRate = 1 / ShootSpeed;
    }
    void Update()
    {
        if(BasicInformation.GetPlace()!=0)
        {
            if (WeaponState == 0)
            {
                Collider[] Hits;
                Hits = Physics.OverlapSphere(this.transform.position, KnifeLength);
                foreach (Collider hit in Hits)
                {
                    if (hit.tag == "Monster" || hit.tag == "PigMan" || hit.tag == "Giant" || hit.tag == "DogMan" || hit.tag == "NormalMan")
                    {
                        hit.gameObject.GetComponent<MonsterFunction>().HitByKnife();
                        //hit.gameObject.GetComponent<Rigidbody>().AddForce((hit.gameObject.transform.position - this.transform.position) * 100);
                        Debug.Log("HitByKnife");

                    }
                }
                BasicInformation.HP -= 0.1f;
            }
            else if (Shootable && (Input.GetKey("j")))//成功扣掉子弹数目之后才可以射击
            {
                
                    CheckShoot(); //调用射击的方法
                
                ani.SetBool("Up_IdleToShoot", true);
                ani.SetBool("Up_IdleToShoot_Grenade",true);
                ResourceController.UIUpdateObject("PistolBullet");
                ResourceController.UIUpdateObject("GrenadeBullet");
                Audio_GunShoot.PlayOneShot(Audio_GunShoot_Source, 1f);
                
            }
            else if(Input.GetKeyUp("j")|| Input.GetMouseButtonUp(0))
            {
                ani.SetBool("Up_IdleToShoot", false);
                ani.SetBool("Up_IdleToShoot_Grenade", false);
            }


            //Debug.Log(Time.deltaTime);
            TimeCounter += Time.deltaTime;

            if (TimeCounter >= ShootRate)
            {
                Shootable = true;
                TimeCounter = 0;
            }
        }
        if(BasicInformation.NowWhere == 0)
        {
            Pistol.SetActive(false);
            Grenade.SetActive(false);
            Knife.SetActive(false);
        }
        else if(WeaponState == 1)
        {
            Pistol.SetActive(true);
            Grenade.SetActive(false);
            Knife.SetActive(false);
            ani.SetBool("Up_IdleToShoot_Grenade", false);
            ani.SetBool("Up_IdleToKnife", false);
            KnifeEffect.SetActive(false);
        }
        else if(WeaponState == 2)
        {
            Pistol.SetActive(false);
            Grenade.SetActive(true);
            Knife.SetActive(false);
            ani.SetBool("Up_IdleToShoot", false);
            ani.SetBool("Up_IdleToKnife", false);
            KnifeEffect.SetActive(false);
        }
        else if(WeaponState == 0)
        {
            Pistol.SetActive(false);
            Grenade.SetActive(false);
            Knife.SetActive(true);
            ani.SetBool("Up_IdleToShoot", false);
            ani.SetBool("Up_IdleToShoot_Grenade", false);
            KnifeEffect.SetActive(true);
        }
    }


    

    //检测开火函数
    void CheckShoot()
    {
            if (ResourceController.GunShoot(WeaponState))
            {
                //实例化一个bullet的克隆体。

                GameObject clone = Instantiate(Bullet,
                                    transform.position + transform.forward * 1 + transform.up * 1,
                                    transform.rotation)
                                    as GameObject;
                if (WeaponState == 1)
                {
                    clone.GetComponent<PistolBulletFunction>().enabled = true;
                    clone.GetComponent<GrenadeBulletFunction>().enabled = false;
                    
                }
                else if(WeaponState == 2)
                {
                    clone.GetComponent<PistolBulletFunction>().enabled = false;
                    clone.GetComponent<GrenadeBulletFunction>().enabled = true;
                    
                }
                //获取克隆体的刚体组件。
                Rigidbody rb = clone.GetComponent<Rigidbody>();

                //给克隆体的刚体组件一个推动力。
                rb.AddForce(this.transform.forward * Thrust);
                Shootable = false;
                //Debug.Log("Shoot");
            }
            else
            {
                Debug.Log("fail to Shoot");
            }
            
    }

    
    
    public static void SetPistolPattern()
    {
        ShootSpeed = 2;
        Thrust = 3000f;
        WeaponState = 1;
        ShootRate = 1 / ShootSpeed;
        //Debug.Log("PistolPattern"+WeaponState);
    }
    public static void SetGrenadePattern()
    {
        ShootSpeed = 5;
        Thrust = 3000f;
        WeaponState = 2;
        ShootRate = 1 / ShootSpeed;
        //Debug.Log("GrenadePattern"+WeaponState);
    }
    public static void SetKnife()
    {
        ShootSpeed = 0.5f;
        Thrust = 0f;
        WeaponState = 0;
        ShootRate = 1 / ShootSpeed;
        //Debug.Log("KnifeState");
    }
}
