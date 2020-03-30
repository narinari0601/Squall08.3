using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp : MonoBehaviour
{
    private GameObject map;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MapDisplay()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(true);

        var obj = collision.gameObject;

        if (obj.tag == "Player")
        {
            Debug.Log(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;

        if (obj.tag == "Player")
        {
            Debug.Log(true);
        }

        Debug.Log(true);
    }
}
