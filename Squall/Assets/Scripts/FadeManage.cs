using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManage : MonoBehaviour
{
    Image color;
    float fading;

    // Start is called before the first frame update
    void Start()
    {
        color = GetComponent<Image>();
        fading = 0;
    }

    // Update is called once per frame
    void Update()
    {
        fading += 0.1f*Time.deltaTime;
        color.color = color.color - new Color(0,0,0,fading);
        
    }
}
