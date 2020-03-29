using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayManager;

public class Stage : MonoBehaviour
{

    [SerializeField,Header("プレイヤー")]
    private GameObject playerObj = null;

    private Vector3 playerInitPos;

    [SerializeField,Header("仲間たち")]
    private GameObject[] members = new GameObject[0];

    [SerializeField, Header("天気が1周する時間")]
    private float weatherRotateTime = 0;

    [SerializeField, Header("風の強さ")]
    private float windPower = 0;

    [SerializeField, Header("風の方向を順番に")]
    private SquallDirections[] squallDirArray = new SquallDirections[0];

    public GameObject PlayerObj { get => playerObj; set => playerObj = value; }
    public GameObject[] Members { get => members; set => members = value; }
    public float WeatherRotateTime { get => weatherRotateTime; set => weatherRotateTime = value; }
    public SquallDirections[] SquallDirArray { get => squallDirArray; set => squallDirArray = value; }
    public float WindPower { get => windPower; set => windPower = value; }

    void Start()
    {
        //Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        playerInitPos = playerObj.transform.position;

        for (int i = 0; i < members.Length; i++)
        {
            //Debug.Log(members[i].name);
        }
    }
}
