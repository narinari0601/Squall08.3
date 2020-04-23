using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayManager;

public class Stage : MonoBehaviour
{

    [SerializeField, Header("プレイヤー")]
    private GameObject playerObj = null;

    private Playercontrol playerController;

    private Vector3 playerInitPos;

    [SerializeField, Header("仲間たち")]
    private GameObject[] members = new GameObject[0];

    private MemberControl[] memberControllers;

    private int memberMaxValue;

    private int hubMember;

    private int deadMember;

    [SerializeField, Header("敵たち")]
    private GameObject[] enemies = new GameObject[0];

    [SerializeField, Header("全体マップのカメラ")]
    private GameObject mapCamera = null;

    [SerializeField, Header("天気が1周する時間")]
    private float weatherRotateTime = 0;

    [SerializeField, Header("風の強さ")]
    private float windPower = 0;

    [SerializeField, Header("風の方向を順番に")]
    private SquallDirections[] squallDirArray = new SquallDirections[0];


    //波紋用タイマー
    private const float RIPPLE_TIME = 6.0f;
    private float currentTimer;


    //スコア
    private float currentScore;

    [SerializeField,Header("星3つスコア")]
    private int threeStarsScore = 6000;

    [SerializeField,Header("星2つスコア")]
    private int twoStarsScore = 4000;

    //[SerializeField,Header("星1つスコア")]
    //private int oneStarScore = 3000;


    public GameObject PlayerObj { get => playerObj; set => playerObj = value; }
    public GameObject[] Members { get => members; set => members = value; }
    public float WeatherRotateTime { get => weatherRotateTime; set => weatherRotateTime = value; }
    public SquallDirections[] SquallDirArray { get => squallDirArray; set => squallDirArray = value; }
    public float WindPower { get => windPower; set => windPower = value; }
    public GameObject MapCamera { get => mapCamera; set => mapCamera = value; }
    public MemberControl[] MemberControllers { get => memberControllers; set => memberControllers = value; }
    public Playercontrol PlayerController { get => playerController; set => playerController = value; }
    public float CurrentScore { get => currentScore; set => currentScore = value; }
    public int ThreeStarsScore { get => threeStarsScore;}
    public int TwoStarsScore { get => twoStarsScore;}
    //public int OneStarScore { get => oneStarScore; }

    void Start()
    {
        //Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        //Ripple();
    }

    public void Initialize()
    {
        playerObj.GetComponent<Playercontrol>().Initialize();
        GamePlayManager.instance.Player = playerObj;
        playerController = playerObj.GetComponent<Playercontrol>();

        playerInitPos = playerObj.transform.position;

        memberControllers = new MemberControl[members.Length];

        for (int i = 0; i < members.Length; i++)
        {
            var member = members[i];
            if (!member.GetComponentInChildren<MemberControl>())
                continue;


            var m_controler = member.GetComponentInChildren<MemberControl>();

            m_controler.Initialize();

            memberControllers[i] = m_controler;
        }

        memberMaxValue = members.Length;

        hubMember = 0;

        deadMember = 0;


        foreach (var enemy in enemies)
        {
            enemy.GetComponent<Obstacle>().Initialize();
        }

        currentTimer = 0.0f;

        currentScore = 0;

    }

    public void StageEnd()
    {

        if (memberControllers.Length == 0)
            return;

        foreach (var member in memberControllers)
        {
            if (member.GetMemberState == MemberControl.MemberStates.isHub)
            {
                hubMember++;
            }

            if (member.GetMemberState == MemberControl.MemberStates.isDaed)
            {
                deadMember++;
            }
        }

        if (deadMember == memberMaxValue)
        {
            GamePlayManager.instance.GameState = GamePlayStates.GameOver;
        }

        else if (deadMember + hubMember == memberMaxValue)
        {
            GamePlayManager.instance.GameState = GamePlayStates.Clear;
        }

        hubMember = 0;
        deadMember = 0;
    }


    public void Ripple()
    {
        var weather = GamePlayManager.instance.Weather;

        if (weather == WeatherStates.Squall)
        {
            currentTimer = 0;
        }

        else
        {

            currentTimer += Time.deltaTime;

            if (currentTimer >= RIPPLE_TIME)
            {
                foreach (var member in memberControllers)
                {
                    member.Ripple();
                }

                foreach (var enemy in enemies)
                {
                    //enemy.GetComponent<Obstacle>().Ripple();
                }

                currentTimer = 0;
            }
        }
    }

    public void ScoreUp()
    {
        var memberList = playerController.MemberList.memberList;
        int scoreMemberCount = 0;
        float plusScore = 0;

        foreach (var member in memberList)
        {
            if (member.GetComponent<MemberControl>())
            {
                var memberController = member.GetComponent<MemberControl>();

                if (memberController.GetMemberState == MemberControl.MemberStates.isAlive)
                {
                    scoreMemberCount++;
                }
            }
        }

        if (scoreMemberCount == 0)
            return;

        plusScore = (float)(1000 * scoreMemberCount * (1 + 0.5 * (scoreMemberCount - 1)));

        GamePlayManager.instance.UIManager.ScoreUpUI.ScoreUp(plusScore);

        currentScore += plusScore;
    }

    public int GetMemberAliveValue()
    {
        int memberVal = 0;

        foreach (var member in memberControllers)
        {
            if (member.GetMemberState == MemberControl.MemberStates.isAlive)
            {
                memberVal++;
            }
        }

        return memberVal;
    }
}
