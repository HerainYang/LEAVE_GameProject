using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBulletFunction : MonoBehaviour
{
    private Collider[] Hits;
    public float ExplodeRadius = 2;
    public float ExplodeForce = 100;
    private float time = 0;
    private bool StartCounting = false;
    private bool FirstTimeShoot = true;
    private Vector3 ExplodePosition;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.collider.name);
        if ((collision.collider.name == "ground"|| collision.collider.tag == "Monster" || collision.collider.tag == "PigMan" || collision.collider.tag == "Giant" || collision.collider.tag == "DogMan" || collision.collider.tag == "NormalMan") &&FirstTimeShoot)
        {
            StartCounting = true;
            ExplodePosition = this.transform.position;
            Explode(ExplodePosition);
            FirstTimeShoot = false;
        }
    }
    private void Explode(Vector3 Position)
    {
        Hits = Physics.OverlapSphere(Position, ExplodeRadius);
        foreach(Collider hit in Hits)
        {
            if(hit.tag == "Monster"||hit.tag == "PigMan"|| hit.tag == "Giant"|| hit.tag == "DogMan"|| hit.tag == "NormalMan")
            {
                Debug.Log("Attacked");
                hit.gameObject.GetComponent<Rigidbody>().AddForce((hit.gameObject.transform.position - Position) * ExplodeForce);
                hit.gameObject.GetComponent<MonsterFunction>().HitByGrenadeBullet();
            }
        }
    }
    private void Update()
    {
        if (StartCounting == true)
        {
            time += Time.deltaTime;
        }
        if (time >= 5)
        {
            Destroy(this.gameObject);
        }
    }
}
