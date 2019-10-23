using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCameraFollow : MonoBehaviour
{
    private float Height = 15;
    public GameObject player;

    // Update is called once per frame
    public void Add()
    {
        GetComponent<Camera>().orthographicSize++;
    }
    public void Subtract()
    {
        GetComponent<Camera>().orthographicSize--;
    }
    void Update()
    {
        this.transform.position = player.transform.position + player.transform.up * Height;
    }
}
