using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LODs : MonoBehaviour
{
    public GameObject Lod0, Lod1;
    public GameObject camera;
    public GameObject pivot;
    public float maxDistance = 500;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(camera.transform.position, pivot.transform.position);
        
        if(distance<= maxDistance)
        {
            Lod0.SetActive(true);
            Lod1.SetActive(false);
            print(distance + " Lod0");
        }
        else
        {
            Lod0.SetActive(false);
            Lod1.SetActive(true);
            print(distance + " Lod1");
        }
    }
}
