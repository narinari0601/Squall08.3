using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainMotion : MonoBehaviour
{
    float defoltx;//生成初期位置

    // Start is called before the first frame update
    void Start()
    {
        defoltx = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position - new Vector3(0, 1, 0);

        if(transform.position.y <= -10)
        {
            transform.position = new Vector3(defoltx,10,0);
        }
    }
}
