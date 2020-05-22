using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WakuFlashing : MonoBehaviour
{

    public float speed = 1.0f;

 
   
    private Image image;
    private float time;

    void Start()
    {
        image = this.gameObject.GetComponent<Image>();

    }

    void Update()
    {
        image.color = GetAlphaColor(image.color);
    }

    //Alpha値を更新してColorを返す
    Color GetAlphaColor(Color color)
    {
        time += Time.deltaTime * 5.0f * speed;
        color.a = Mathf.Sin(time) * 0.5f + 0.5f;
        if(color.a <= 0.3)
        {
            color.a = 0.3f;
        }

        return color;
    }


}
