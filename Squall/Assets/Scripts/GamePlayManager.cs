using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance = null;


    public enum WeatherStates
    {
        Sun,
        Sign,
        Squall,
    }

    public enum SquallDirections
    {
        Up=0,
        Down=1,
        Left=2,
        Right=3,
    }

    public enum GamePlayStates
    {
        Play,
        Map,
        Pause,
        Clear,
        GameOver,
    }


    //ゲームオブジェクト関連
    private GameObject player;

    //private GameObject[] members;

    [SerializeField, Header("メインカメラ")]
    private GameObject mainCamera = null;

    private GameObject currentCamera;

    private List<GameObject> cameraList;

    //天気関連
    private WeatherStates weather;

    //[SerializeField,Header("天気が1周する時間")]
    private float weatherRotateTime;

    [SerializeField, Header("通常時の比率")]
    private float sunRatio = 0;

    [SerializeField, Header("予兆の比率")]
    private float signRatio = 0;

    [SerializeField, Header("スコールの比率")]
    private float squallRatio = 0;

    private float toatalWeatherRatio;  //天候比の合計

    private float currentWeatherTimer;

    //風関連
    private SquallDirections squallDirection;

    private float windPower;

    //[SerializeField, Header("風の方向を順番に")]
    private SquallDirections[] squallDirArray;

    private int squallCount;  //何回目のスコールか


    //ステージ関連
    [SerializeField,Header("ステージ")]
    private GameObject[] stagePrefabs = new GameObject[0];

    //private List<GameObject> stageList;

    private Stage currentStage;

    private int stageNum;


    //ゲームの状態関連
    private GamePlayStates gameState;


    //UI関連
    [SerializeField, Header("UIマネージャーオブジェ")]
    private GameObject uIObj = null;
    private UIManager uiManager;


    public WeatherStates Weather { get => weather; set => weather = value; }
    public SquallDirections SquallDirection { get => squallDirection; set => squallDirection = value; }
    public GameObject Player { get => player; set => player = value; }
    public Stage CurrentStage { get => currentStage; set => currentStage = value; }
    public GamePlayStates GameState { get => gameState; set => gameState = value; }
    public UIManager UIManager { get => uiManager; set => uiManager = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);  //シーンを切り替えても消えない
        }
        else
        {
            Destroy(this.gameObject);
        }

        Initialize();
    }

    private void Initialize()
    {
        toatalWeatherRatio = sunRatio + signRatio + squallRatio;
        
        stageNum = 0;

        cameraList = new List<GameObject>();

        StageInitialize();

        uiManager = uIObj.GetComponent<UIManager>();
        uiManager.Initialize();
    }

    private void StageInitialize()
    {
        var stage = Instantiate(stagePrefabs[stageNum]);

        //currentStage = stageList[stageNum].GetComponent<Stage>();

        currentStage = stage.GetComponent<Stage>();
        currentStage.Initialize();

        weather = WeatherStates.Sun;

        currentWeatherTimer = 0;

        squallCount = 0;

        weatherRotateTime = currentStage.WeatherRotateTime;

        windPower = currentStage.WindPower;

        squallDirArray = currentStage.SquallDirArray;

        squallDirection = squallDirArray[0];

        ///
        //player = currentStage.PlayerObj;

        //player.GetComponent<Playercontrol>().Initialize();

        //members = currentStage.Members;

        //for (int i = 0; i < members.Length; i++)
        //{
        //    Debug.Log(members[i].name);
        //}

        mainCamera.GetComponent<CameraController>().Initialize();

        cameraList.Clear();
        cameraList.Add(mainCamera.GetComponent<CameraController>().Camera);
        var mapCamera = currentStage.MapCamera;
        cameraList.Add(mapCamera);
        currentCamera = cameraList[0];
        cameraList[1].SetActive(false);
        
    }

    private void FixedUpdate()
    {
        ChangeCamera();
        ChangeWeather();

        if (gameState == GamePlayStates.Play)
        {
            StageEndCheack();
            currentStage.Ripple();
        }

        else if (gameState == GamePlayStates.Map)
        {
            uiManager.HiddenPlayUI();

            MapEnd();
        }

        else if (gameState == GamePlayStates.Clear)
        {
            GameClear();
        }

        else if (gameState == GamePlayStates.GameOver)
        {
            GameOver();
        }

        RetryScene();


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEnd();
        }
    }

    private void ChangeWeather()
    {
        currentWeatherTimer += Time.deltaTime;

        if (currentWeatherTimer >= weatherRotateTime)
        {
            currentWeatherTimer = 0;

            squallCount++;

            if (squallCount == squallDirArray.Length)
            {
                squallCount = 0;
            }

            squallDirection = squallDirArray[squallCount];
        }

        //晴れ
        if (currentWeatherTimer < sunRatio / toatalWeatherRatio * weatherRotateTime)
        {
            weather = WeatherStates.Sun;
            //Debug.Log("晴れ");
        }

        //スコール
        else if (currentWeatherTimer >= (toatalWeatherRatio - squallRatio) / toatalWeatherRatio * weatherRotateTime)
        {
            weather = WeatherStates.Squall;
            uiManager.WindDirectUI.SetActive(false);
            //Debug.Log("スコール");
        }

        //予兆
        else
        {
            weather = WeatherStates.Sign;
            
            var currentDir= (int)squallDirArray[(squallCount + 0) % squallDirArray.Length];
            var secondDir = (int)squallDirArray[(squallCount + 1) % squallDirArray.Length];
            var thirdDir= (int)squallDirArray[(squallCount + 2) % squallDirArray.Length];
            uiManager.WindDirectUI.ChangeDirection(currentDir, secondDir, thirdDir);

            if (gameState == GamePlayStates.Play)
            {
                uiManager.WindDirectUI.SetActive(true);
            }

            //currentDirect.text = directStrings[(int)squallDirArray[squallCount]];
            //secondDirect.text = directStrings[(int)secondDir];
            //thirdDirect.text = directStrings[(int)thirdDir];
            //Debug.Log("予兆");
        }


    }

    public void NextStage()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //var stage = Instantiate(stagePrefabs[stageNum]);
            //stage.GetComponent<Stage>().Initialize();
            //stageList[stageNum] = stage;

            Destroy(currentStage.gameObject);

            stageNum++;
            

            if (stageNum > stagePrefabs.Length - 1)
            {
                stageNum = 0;
            }

            StageInitialize();
        }
    }

    public void StageEndCheack()
    {
        currentStage.StageEnd();
    }

    public void ChangeCamera()
    {
        var state = gameState;

        if (state == GamePlayStates.Play)
        {
            if (currentCamera == cameraList[0])
                return;

            currentCamera = cameraList[0];
            cameraList[1].SetActive(false);
            cameraList[0].SetActive(true);
        }

        else if (state == GamePlayStates.Map)
        {
            if (currentCamera == cameraList[1])
                return;

            currentCamera = cameraList[1];
            cameraList[0].SetActive(false);
            cameraList[1].SetActive(true);
        }
    }

    public void MapEnd()
    {
        if (gameState == GamePlayStates.Map)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                uiManager.MemberAliveUI.SetActive(true);
                gameState = GamePlayStates.Play;
            }
        }
    }

    public void RetryScene()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void GameClear()
    {
        Debug.Log("クリア");
    }

    public void GameOver()
    {

    }
    

    public void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
