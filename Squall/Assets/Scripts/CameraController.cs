using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField, Header("カメラのターゲット")]
    private GameObject target;

    [SerializeField, Header("playerとの距離")]
    private Vector3 offset = Vector3.zero;
    

    void Start()
    {
        
    }

    public void Initialize()
    {
        target = GamePlayManager.instance.Player;
        //offset = target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position - offset;
    }
}
