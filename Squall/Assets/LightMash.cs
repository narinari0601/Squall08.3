﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightMash : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
       
        
            Debug.Log("Hit"); // ログを表示する
        
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
