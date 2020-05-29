using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GamePlayManager;

public class Stage : MonoBehaviour
{

    [SerializeField, Header("プレイヤー")]
    private GameObject playerObj = null;

    private Playercontrol playerController;

    [SerializeField, Header("仲間たち")]
    private GameObject[] members = new GameObject[0];

    private MemberControl[] memberControllers;

    private int memberMaxValue;

    private int hubMember;

    private int deadMember;

    [SerializeField, Header("敵たち")]
    private GameObject[] enemies = new GameObject[0];

    [SerializeField, Header("拠点")]
    private GameObject baseCamp = null;


    //天気関連
    [SerializeField, Header("天気が1周する時間")]
    private float weatherRotateTime = 0;
    
    private const float SUN_RATIO = 2;
    
    private const float SIGN_RATIO = 3;
    
    private const float SQUALL_RATIO = 5;

    private const float TOATAL_WEATHER_RATIO = 10;  //天候比の合計

    private float currentWeatherTimer;

    //風関連
    [SerializeField, Header("風の強さ")]
    private float windPower = 0;

    [SerializeField, Header("風の方向を順番に")]
    private SquallDirections[] squallDirArray = new SquallDirections[0];
    
    private List<SquallDirections> squallDirList;

    private int squallCount;  //何回目のスコールか

    private bool isInsert;

    private SquallDirections interruptDir;  //割り込みスコールの方向


    //波紋用タイマー
    private const float RIPPLE_TIME = 4.0f;
    private float currentTimer;


    //スコア
    private float currentScore;
    

    [SerializeField, Header("星3つスコア")]
    private int threeStarsScore = 6000;

    [SerializeField, Header("星2つスコア")]
    private int twoStarsScore = 4000;

    //[SerializeField,Header("星1つスコア")]
    //private int oneStarScore = 3000;


    public GameObject PlayerObj { get => playerObj; set => playerObj = value; }
    public GameObject[] Members { get => members; set => members = value; }
    public GameObject BaseCamp { get => baseCamp; set => baseCamp = value; }
    public float WeatherRotateTime { get => weatherRotateTime; set => weatherRotateTime = value; }
    public SquallDirections[] SquallDirArray { get => squallDirArray; set => squallDirArray = value; }
    public float WindPower { get => windPower; set => windPower = value; }
    public MemberControl[] MemberControllers { get => memberControllers; set => memberControllers = value; }
    public Playercontrol PlayerController { get => playerController; set => playerController = value; }
    public float CurrentScore { get => currentScore; set => currentScore = value; }
    public int ThreeStarsScore { get => threeStarsScore; }
    public int TwoStarsScore { get => twoStarsScore; }
    public bool IsInsert { get => isInsert; set => isInsert = value; }


    //public int OneStarScore { get => oneStarScore; }

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
        playerObj.GetComponent<Playercontrol>().Initialize();
        GamePlayManager.instance.Player = playerObj;
        playerController = playerObj.GetComponent<Playercontrol>();

        GamePlayManager.instance.CameraController = GamePlayManager.instance.MainCamera.GetComponent<CameraController>();
        GamePlayManager.instance.CameraController.Initialize();

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

        currentTimer = RIPPLE_TIME;

        currentScore = 0;

        GamePlayManager.instance.UIManager.Initialize();

        //天気関連
        //toatalWeatherRatio = SUN_RATIO + SIGN_RATIO + SQUALL_RATIO;

        //風関連
        currentWeatherTimer = 0;

        squallCount = 0;

        squallDirList = new List<SquallDirections>();

        for (int i = 0; i < squallDirArray.Length; i++)
        {
            squallDirList.Add(squallDirArray[i]);
        }

        //squallDirection = squallDirArray[0];

        GamePlayManager.instance.SquallDirection = squallDirList[squallCount];

        isInsert = false;
        interruptDir = SquallDirections.Up;

        var currentDir = (int)squallDirList[(squallCount + 0) % squallDirList.Count];
        var secondDir = (int)squallDirList[(squallCount + 1) % squallDirList.Count];
        var thirdDir = (int)squallDirList[(squallCount + 2) % squallDirList.Count];

        GamePlayManager.instance.UIManager.WindDirectUI.ChangeDirection(currentDir, secondDir, thirdDir);

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

        if (deadMember > 0)
        {
            GamePlayManager.instance.GameState = GamePlayStates.GameOver;
            var uiManager = GamePlayManager.instance.UIManager;
            uiManager.PlayUIActiveFalse();
            uiManager.PauseUI.SetActive(false);
            BGMManager.instance.StopBGM();
        }

        else if (hubMember == memberMaxValue)
        {
            GamePlayManager.instance.GameState = GamePlayStates.Clear;

            if (currentScore >= StageData.HighScores[GamePlayManager.instance.StageNum])
            {
                StageData.HighScores[GamePlayManager.instance.StageNum] = currentScore;
            }

            StageData.StageClear(GamePlayManager.instance.StageNum + 1);

            var uiManager = GamePlayManager.instance.UIManager;
            uiManager.PlayUIActiveFalse();
            uiManager.PauseUI.SetActive(false);
            BGMManager.instance.ChangeBGM(2, 0.07f);   
        }

        hubMember = 0;
        deadMember = 0;
    }

    public void ChangeWeather()
    {
        currentWeatherTimer += Time.deltaTime;

        if (currentWeatherTimer >= weatherRotateTime)
        {
            currentWeatherTimer = 0;

            if (isInsert)
            {
                if (GamePlayManager.instance.SquallDirection == interruptDir)
                {
                    isInsert = false;
                    squallDirList.RemoveAt(squallCount);

                    squallCount--;
                    if (squallCount < 0)
                    {
                        squallCount = squallDirList.Count - 1;
                    }
                }
            }

            squallCount++;

            //if (squallCount == squallDirArray.Length)
            //{
            //    squallCount = 0;
            //}

            if (squallCount == squallDirList.Count)
            {
                squallCount = 0;
            }

            //squallDirection = squallDirArray[squallCount];

            GamePlayManager.instance.SquallDirection = squallDirList[squallCount];
        }

        //var weather = GamePlayManager.instance.Weather;
        //var uiManager = GamePlayManager.instance.UIManager;

        //晴れ
        if (currentWeatherTimer < SUN_RATIO / TOATAL_WEATHER_RATIO * weatherRotateTime)
        {
            var currentDir = (int)squallDirList[(squallCount + 0) % squallDirList.Count];
            var secondDir = (int)squallDirList[(squallCount + 1) % squallDirList.Count];
            var thirdDir = (int)squallDirList[(squallCount + 2) % squallDirList.Count];
            GamePlayManager.instance.UIManager.WindDirectUI.ChangeDirection(currentDir, secondDir, thirdDir);

            GamePlayManager.instance.Weather = WeatherStates.Sun;
            //Debug.Log("晴れ");
        }

        //スコール
        else if (currentWeatherTimer >= (TOATAL_WEATHER_RATIO - SQUALL_RATIO) / TOATAL_WEATHER_RATIO * weatherRotateTime)
        {
            GamePlayManager.instance.Weather = WeatherStates.Squall;
            GamePlayManager.instance.UIManager.WindDirectUI.SetActive(false);
            //Debug.Log("スコール");
        }

        //予兆
        else
        {
            GamePlayManager.instance.Weather = WeatherStates.Sign;

            //var currentDir= (int)squallDirArray[(squallCount + 0) % squallDirArray.Length];
            //var secondDir = (int)squallDirArray[(squallCount + 1) % squallDirArray.Length];
            //var thirdDir= (int)squallDirArray[(squallCount + 2) % squallDirArray.Length];

            var currentDir = (int)squallDirList[(squallCount + 0) % squallDirList.Count];
            var secondDir = (int)squallDirList[(squallCount + 1) % squallDirList.Count];
            var thirdDir = (int)squallDirList[(squallCount + 2) % squallDirList.Count];
            GamePlayManager.instance.UIManager.WindDirectUI.ChangeDirection(currentDir, secondDir, thirdDir);

            if (GamePlayManager.instance.GameState == GamePlayStates.Play)
            {
                GamePlayManager.instance.UIManager.WindDirectUI.SetActive(true);
            }

            //currentDirect.text = directStrings[(int)squallDirArray[squallCount]];
            //secondDirect.text = directStrings[(int)secondDir];
            //thirdDirect.text = directStrings[(int)thirdDir];
            //Debug.Log("予兆");
        }


    }

    public void InterrectWind(SquallDirections dir)
    {
        if (!isInsert)
        {
            isInsert = true;

            if (GamePlayManager.instance.Weather == WeatherStates.Squall)
            {
                squallDirList.Insert(squallCount + 1, dir);
            }

            else
            {
                squallDirList.Insert(squallCount, dir);
            }

            interruptDir = dir;
            GamePlayManager.instance.SquallDirection = squallDirList[squallCount];

            GamePlayManager.instance.PlaySE(0, 0.7f);

            var currentDir = (int)squallDirList[(squallCount + 0) % squallDirList.Count];
            var secondDir = (int)squallDirList[(squallCount + 1) % squallDirList.Count];
            var thirdDir = (int)squallDirList[(squallCount + 2) % squallDirList.Count];

            GamePlayManager.instance.UIManager.WindDirectUI.ChangeDirection(currentDir, secondDir, thirdDir);
        }
    }

    public void Ripple()
    {
        var weather = GamePlayManager.instance.Weather;

        if (weather == WeatherStates.Squall)
        {
            currentTimer = RIPPLE_TIME;
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

        float bonusScore = (float)(1 + 0.5 * (scoreMemberCount - 1));
        plusScore = (float)(1000 * scoreMemberCount * bonusScore);

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
