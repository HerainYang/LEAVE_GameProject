using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolBulletFunction : MonoBehaviour
{
    private float time = 0;
    private bool StartCounting = false;
    private bool FirstTimeShoot = true;
    
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name);
        if(collision.collider.name == "ground"&&FirstTimeShoot)
        {
            
            FirstTimeShoot = false;
        }
        else if((collision.collider.tag == "Monster"||collision.collider.tag == "PigMan"|| collision.collider.tag=="Giant"|| collision.collider.tag=="DogMan"|| collision.collider.tag=="NormalMan") &&FirstTimeShoot)
        {
            collision.collider.gameObject.GetComponent<MonsterFunction>().HitByPistolBullet();
        }
        StartCounting = true;
    }
    private void Update()
    {
        if(StartCounting == true)
        {
            time += Time.deltaTime;
        }
        if(time>=5)
        {
            Destroy(this.gameObject);
        }
    }
}
