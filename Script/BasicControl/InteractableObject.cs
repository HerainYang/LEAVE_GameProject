using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private float InteractRange;
    public List<string> storehouse = new List<string>();
    // Start is called before the first frame update
    private float GetThirdLength(float FirstLength,float SecondLength)
    {
        return Mathf.Sqrt(FirstLength * FirstLength + SecondLength * SecondLength);
    }

    void Start()
    {
        InteractRange = GetThirdLength(this.GetComponent<Collider>().bounds.size.y, GetThirdLength(this.GetComponent<Collider>().bounds.size.x, this.GetComponent<Collider>().bounds.size.z)) / 2;
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] Hits;
        Hits = Physics.OverlapSphere(this.transform.position, InteractRange);
        foreach(Collider Hit in Hits)
        {
            if(Hit.name == "player")
            {
                if(Input.GetKeyDown("f"))
                {
                    Debug.Log("互动中");
                    GameObject player = GameObject.Find(Hit.name);
                    List<string> list = player.GetComponent<BasicMove>().Package;
                    for(int i = 0;i<list.Count;i++)
                    {
                        storehouse.Add(list[i]);
                        
                        Debug.Log("你存放了" + list[i]);
                    }
                    list.Clear();
                }
            }
        }
        
    }
}
