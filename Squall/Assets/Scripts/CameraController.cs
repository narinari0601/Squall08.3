using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField, Header("カメラのターゲット")]
    private GameObject target;

    [SerializeField, Header("playerとの距離")]
    private Vector3 offset = Vector3.zero;

    [SerializeField, Header("メインカメラ")]
    private GameObject m_Camera = null;

    [SerializeField, Header("周りの黒いやつ")]
    private GameObject black = null;

    public GameObject Camera { get => m_Camera; set => m_Camera = value; }
    public GameObject Black { get => black; set => black = value; }

    void Start()
    {
        
    }

    public void Initialize()
    {
        target = GamePlayManager.instance.Player;
        black.SetActive(true);
        //offset = target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position - offset;
    }
}
