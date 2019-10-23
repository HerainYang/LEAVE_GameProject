using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterFunction : MonoBehaviour
{
    public int HP = 100;//可以根据不同怪兽的属性来进行更改
    private int HPRemain;
    public int AttackRange;
    public int VisionRange;
    public float speed;
    public int Damage;
    public int StrongHoldRange;

    public float AttackCD;//攻击cd
    public float ResurrectionTime = 10f;//复活时间
    public float DestinationRefreshTime = 5f;//巡逻地点刷新时间

    private GameObject player;

    private bool Attackable = true;//能否攻击
    private bool IsDead = false;//是否死亡
    private bool DestinationRefreshable = true;//能否刷新目的地

    private float TimeCounter = 0;
    private float AttackTimeCounter = 0;
    private float DestinationCounter = 0;

    private Vector3 VeryFar = new Vector3(0, -1000, 0);
    public Vector3 StrongHoldPosition;
    private Vector3 PatrolDestination;

    public Animator ani;

    public AudioSource Audio_Enemy;
    public AudioClip AudioClip_Standing;
    public AudioClip AudioClip_Attack;

    void Start()
    {
        StrongHoldPosition = this.transform.position;
        player = GameObject.Find("player");
        HPRemain = HP;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsDead&&TimeCounter<ResurrectionTime)
        {
            TimeCounter += Time.deltaTime;
            //Debug.Log("Dead and Wait");
        }
        else if (IsDead&&TimeCounter>=ResurrectionTime)
        {
            HPRemain = HP;
            TimeCounter = 0;
            IsDead = false;
            gameObject.transform.position = StrongHoldPosition;
            GetComponent<NavMeshAgent>().enabled = true;
            //Debug.Log("Dead and Already");
        }
        
        if(HPRemain>0&&!IsDead)
        {
            Action();
            //Debug.Log("Alive and moving");
        }
        else if(HPRemain<=0&&!IsDead)//生命值为0但不是未死状态
        {
            IsDead = true;
            MissionChecker();
            Debug.Log("awsl");
            ResourceController.CreateObject(this.gameObject,transform.position, transform.rotation);
            this.transform.position = VeryFar;
            GetComponent<NavMeshAgent>().enabled = false;
            //Debug.Log("Alive but Already to Dead");
        }

        
    }
    private void MissionChecker()
    {
        if(MissionController.MissionDetailInfo_1.Count!=0&&(int)MissionController.MissionDetailInfo_1[1] == 1)
        {
            if ((string)MissionController.MissionDetailInfo_1[2] == this.tag&&(int)MissionController.MissionDetailInfo_1[3]!=(int)MissionController.MissionDetailInfo_1[4])
            {
                Debug.Log("+1");
                MissionController.MissionDetailInfo_1[3] = (int)MissionController.MissionDetailInfo_1[3] + 1;
            }
        }

        if(MissionController.MissionDetailInfo_2.Count!=0&& (int)MissionController.MissionDetailInfo_2[1] == 1)
        {
            if ((string)MissionController.MissionDetailInfo_2[2] == this.tag&& (int)MissionController.MissionDetailInfo_2[3] != (int)MissionController.MissionDetailInfo_2[4])
            {
                Debug.Log("+1");
                MissionController.MissionDetailInfo_2[3] = (int)MissionController.MissionDetailInfo_2[3] + 1;
            }
        }

        if(MissionController.MissionDetailInfo_3.Count!=0&& (int)MissionController.MissionDetailInfo_3[1] == 1)
        {
            Debug.Log("inside MissionChecker:" + this.tag);
            Debug.Log((string)MissionController.MissionDetailInfo_1[2] == this.tag);
            if ((string)MissionController.MissionDetailInfo_3[2] == this.tag&& (int)MissionController.MissionDetailInfo_3[3] != (int)MissionController.MissionDetailInfo_3[4])
            {
                Debug.Log("+1");
                MissionController.MissionDetailInfo_3[3] = (int)MissionController.MissionDetailInfo_3[3] + 1;
            }
        }
    }
    public void Action()
    {
        if ((player.transform.position - this.transform.position).magnitude <= AttackRange)
        {
            Attack();
            //Debug.Log("Attacking");
            //ani.SetBool("Lower_Run", false);
            ani.SetBool("Upper_Run", false);
        }
        else if ((player.transform.position - this.transform.position).magnitude <= VisionRange && (player.transform.position - this.transform.position).magnitude > AttackRange)
        {
            Follow();
            //Debug.Log("Following");
            ani.SetBool("Lower_Run", true);
            ani.SetBool("Upper_Run", true);
            ani.SetBool("Lower_Walk", false);
        }
        else if((player.transform.position - this.transform.position).magnitude > VisionRange && (this.transform.position-StrongHoldPosition).magnitude<=StrongHoldRange)
        {
            Patrol();
            //Debug.Log("Patrolling");
            this.GetComponent<NavMeshAgent>().speed = 1f;
            ani.SetBool("Lower_Run", false);
            ani.SetBool("Upper_Run", false);
        }
        else if((player.transform.position - this.transform.position).magnitude > VisionRange&& (this.transform.position - StrongHoldPosition).magnitude > StrongHoldRange)
        {
            //Debug.Log("Impossible");
            ani.SetBool("Lower_Run", false);
            ani.SetBool("Upper_Run", false);
            GoBack();
        }
    }
    void Attack()
    {
        if(Attackable)
        {
            Debug.Log("Attack");
            BasicInformation.Damage(Damage);
            Attackable = false;
            ani.Play("AttackUpper", 0, 0);
            Audio_Enemy.PlayOneShot(AudioClip_Attack, 1f);
        }
        else if(!Attackable&&AttackTimeCounter<AttackCD)
        {
            AttackTimeCounter+=Time.deltaTime;
            
        }
        else if(!Attackable&&AttackTimeCounter>=AttackCD)
        {
            Attackable = true;
            AttackTimeCounter = 0;
        }
        
    }
    void Follow()
    {
        this.GetComponent<NavMeshAgent>().destination = player.transform.position;
        this.GetComponent<NavMeshAgent>().speed = 3.5f;
    }
    void Patrol()
    {
        if(DestinationRefreshable)
        {
            Vector3 RandomCircle = Random.insideUnitSphere * StrongHoldRange;
            float D_Position_x = RandomCircle.x + StrongHoldPosition.x;
            float D_Position_z = RandomCircle.z + StrongHoldPosition.z;//平移圆心
            PatrolDestination = new Vector3(D_Position_x, this.transform.position.y, D_Position_z);
            DestinationRefreshable = false;

            ani.SetBool("Upper_Walk", true);
            ani.SetBool("Lower_Walk", true);
        }
        else if(!DestinationRefreshable&&DestinationCounter<DestinationRefreshTime)
        {
            DestinationCounter += Time.deltaTime;
            if(this.transform.position.x == PatrolDestination.x&&this.transform.position.z == PatrolDestination.z)
            {
                //Debug.Log("Arrival");

                ani.SetBool("Upper_Walk", false);
                ani.SetBool("Lower_Walk", false);
            }
            
        }
        else if(!DestinationRefreshable&&DestinationCounter>=DestinationRefreshTime)
        {
            DestinationRefreshable = true;
            DestinationCounter = 0;
            Audio_Enemy.PlayOneShot(AudioClip_Standing, 1f);
        }
        this.GetComponent<NavMeshAgent>().destination = PatrolDestination;
        //Debug.Log(PatrolDestination);
    }
    public void GoBack()
    {
        this.GetComponent<NavMeshAgent>().destination = StrongHoldPosition;
    }
    public void HitByPistolBullet()
    {
        HPRemain -= 5;
        Debug.Log(HPRemain);
        ColorController.IsHit = true;
    }
    public void HitByGrenadeBullet()
    {
        HPRemain -= 5;
        ColorController.IsHit = true;
    }
    public void HitByKnife()
    {
        HPRemain -= 1;
        ColorController.IsHit = true;
    }
}
