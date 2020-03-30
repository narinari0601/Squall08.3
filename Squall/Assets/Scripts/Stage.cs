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

    private MemberControl[] memberControllers;

    private int memberMaxValue;

    private int stageClearMember;

    [SerializeField, Header("全体マップのカメラ")]
    private GameObject mapCamera = null;

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
    public GameObject MapCamera { get => mapCamera; set => mapCamera = value; }

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

        memberControllers = new MemberControl[members.Length];

        for (int i = 0; i < members.Length; i++)
        {
            //Debug.Log(members[i].name);
            var member = members[i];
            if (!member.GetComponentInChildren<MemberControl>())
                continue;
            

            var m_controler = member.GetComponentInChildren<MemberControl>();

            m_controler.Initialize();

            memberControllers[i] = m_controler;
        }

        memberMaxValue = members.Length;

        stageClearMember = 0;
        
        
    }

    public void StageClear()
    {

        if (memberControllers.Length == 0)
            return;

        foreach (var member in memberControllers)
        {

            //if (member.GetMemberCheck == MemberControl.MemberCheck.isCapture)
            //{
            //    stageClearMember++;
            //}
            

            if (member.GetMemberState == MemberControl.MemberStates.isDaed ||
                member.GetMemberState == MemberControl.MemberStates.isHub)
            {
                stageClearMember++;
            }
        }

        if (stageClearMember == memberMaxValue)
        {
            Debug.Log("クリア");
        }

        stageClearMember = 0;
    }
}
