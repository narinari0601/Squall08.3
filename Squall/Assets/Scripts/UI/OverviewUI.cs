using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverviewUI : MonoBehaviour
{
    [SerializeField,Header("大元のパネル")]
    private GameObject OverviewPanel = null;


    private GameObject mapCamera;

    [SerializeField, Header("マップの移動速度")]
    private float cameraVelocity = 0;

    private Vector3 initPos;  //初期位置


    //風関連
    [SerializeField, Header("1番目の風")]
    private Text firstWind = null;

    [SerializeField, Header("2番目の風")]
    private Text secondWind = null;

    [SerializeField, Header("3番目の風")]
    private Text thirdWind = null;

    void Start()
    {
        
    }

    public void Initialize()
    {
        mapCamera = GamePlayManager.instance.CameraController.MapCamera;
        initPos = mapCamera.transform.position;
        SetActive(true);
    }


    void Update()
    {
        
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            OverviewPanel.SetActive(true);
        }

        else
        {
            OverviewPanel.SetActive(false);
        }
    }

    public void SetWindDirect(string first, string second, string third)
    {
        firstWind.text = first;
        secondWind.text = second;
        thirdWind.text = third;
    }

    public void MapCameraMove()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mapCamera.transform.position += new Vector3(0, 0, cameraVelocity);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            mapCamera.transform.position += new Vector3(0, 0, -cameraVelocity);
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            mapCamera.transform.position += new Vector3(cameraVelocity, 0, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            mapCamera.transform.position += new Vector3(-cameraVelocity, 0, 0);
        }
    }

    public void MapCameraReset()
    {
        mapCamera.transform.position = initPos;
    }
}
