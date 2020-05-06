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

    private Camera cameraScript;

    [SerializeField,Header("マップカメラ")]
    private GameObject mapCamera = null;

    private Camera mapCameraScript;

    [SerializeField, Header("周りの黒いやつ")]
    private GameObject black = null;

    public GameObject Camera { get => m_Camera; set => m_Camera = value; }
    public GameObject Black { get => black; set => black = value; }
    public Camera CameraScript { get => cameraScript; set => cameraScript = value; }
    public GameObject MapCamera { get => mapCamera; set => mapCamera = value; }

    void Start()
    {
        
    }

    public void Initialize()
    {
        cameraScript = m_Camera.GetComponent<Camera>();
        mapCameraScript = mapCamera.GetComponent<Camera>();
        target = GamePlayManager.instance.Player;
        mapCamera.transform.position = new Vector3(target.transform.position.x, mapCamera.transform.position.y, target.transform.position.z);
        black.SetActive(true);
        //offset = target.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var pos= new Vector3(target.transform.position.x - offset.x, m_Camera.transform.position.y, target.transform.position.z - offset.z);
        m_Camera.transform.position = pos;
    }

    public void ChangeDepath(int num)
    {
        cameraScript.depth = num;
    }

    public void PlayToMap()
    {
        if (cameraScript.depth > mapCameraScript.depth)
        {
            cameraScript.depth = 2;
            mapCameraScript.depth = 4;
        }
    }

    public void MapToPlay()
    {
        if (cameraScript.depth < mapCameraScript.depth)
        {
            cameraScript.depth = 4;
            mapCameraScript.depth = 2;
        }
    }
}
