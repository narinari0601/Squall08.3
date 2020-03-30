using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasedirectionUI : MonoBehaviour
{
    public GameObject baseCamp;
    public GameObject baseUI;
    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        if (baseCamp == null)
        {
            baseCamp = GameObject.FindWithTag("BaseCamp");
        }
    }

    // Update is called once per frame
    void Update()
    {
        baseUI.transform.position = camera.WorldToScreenPoint(baseCamp.transform.position);
        if(baseUI.transform.position.x < 60)
        {
            baseUI.transform.position = new Vector3(60,baseUI.transform.position.y,baseUI.transform.position.z);
        }
        if (baseUI.transform.position.x > 1220)
        {
            baseUI.transform.position = new Vector3(1220, baseUI.transform.position.y, baseUI.transform.position.z);
        }
        if (baseUI.transform.position.y < 50)
        {
            baseUI.transform.position = new Vector3(baseUI.transform.position.x,50, baseUI.transform.position.z);
        }
        if (baseUI.transform.position.y > 670)
        {
            baseUI.transform.position = new Vector3(baseUI.transform.position.x,670, baseUI.transform.position.z);
        }
    }
}
