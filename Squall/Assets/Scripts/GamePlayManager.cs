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
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3,
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

    [SerializeField, Header("メインカメラ")]
    private GameObject mainCamera = null;

    private CameraController cameraController;

    private SquallCameraBlind cameraBlind;

    //天気関連
    private WeatherStates weather;

    private float weatherRotateTime;

    [SerializeField, Header("通常時の比率")]
    private float sunRatio = 0;

    [SerializeField, Header("予兆の比率")]
    private float signRatio = 0;

    [SerializeField, Header("スコールの比率")]
    private float squallRatio = 0;

    private float toatalWeatherRatio;  //天候比の合計


    //風関連
    private SquallDirections squallDirection;

    private float windPower;



    //ステージ関連
    [SerializeField, Header("ステージ")]
    private GameObject[] stagePrefabs = new GameObject[0];

    private List<GameObject> stageList;

    private Stage currentStage;

    private int stageNum = 0;

    [SerializeField, Header("bake2つ")]
    private GameObject[] navBakes = new GameObject[2];



    //ゲームの状態関連
    private GamePlayStates gameState;



    //UI関連
    [SerializeField, Header("UIマネージャーオブジェ")]
    private GameObject uIObj = null;
    private UIManager uiManager;


    //音関連
    private AudioSource audioSource;

    [SerializeField, Header("SE")]
    private AudioClip[] audioClipsSE = new AudioClip[0];

    private bool isBGM;


    public WeatherStates Weather { get => weather; set => weather = value; }
    public SquallDirections SquallDirection { get => squallDirection; set => squallDirection = value; }
    public GameObject Player { get => player; set => player = value; }
    public Stage CurrentStage { get => currentStage; set => currentStage = value; }
    public GamePlayStates GameState { get => gameState; set => gameState = value; }
    public UIManager UIManager { get => uiManager; set => uiManager = value; }
    public GameObject[] NavBakes { get => navBakes; set => navBakes = value; }
    public CameraController CameraController { get => cameraController; set => cameraController = value; }
    public GameObject MainCamera { get => mainCamera; set => mainCamera = value; }
    public int StageNum { get => stageNum; set => stageNum = value; }

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
        stageNum = StageData.StageNum;

        toatalWeatherRatio = sunRatio + signRatio + squallRatio;

        cameraBlind = mainCamera.GetComponent<SquallCameraBlind>();

        audioSource = GetComponent<AudioSource>();

        foreach (var nav in navBakes)
        {
            nav.GetComponent<NavBakeScript>().Initialize();
        }
        uiManager = uIObj.GetComponent<UIManager>();

        StageInitialize();

        isBGM = false;

    }

    public void StageInitialize()
    {
        if (currentStage != null)
        {
            currentStage.gameObject.SetActive(false);
            Destroy(currentStage.gameObject);
            currentStage = null;
        }

        var stage = Instantiate(StageData.StageList[stageNum]);

        foreach (var nav in navBakes)
        {
            nav.GetComponent<NavBakeScript>().NavReBake();
        }

        currentStage = stage.GetComponent<Stage>();
        currentStage.Initialize();

        weather = WeatherStates.Sun;

        gameState = GamePlayStates.Map;

        weatherRotateTime = currentStage.WeatherRotateTime;

        windPower = currentStage.WindPower;

        cameraBlind.Initialize();
    }

    private void Update()
    {
        if (!isBGM)
        {
            BGMManager.instance.ChangeBGM(0, 0.04f);
            isBGM = true;
        }

        ChangeCamera();

        if (gameState == GamePlayStates.Play)
        {
            StageEndCheack();
            currentStage.Ripple();
            currentStage.ChangeWeather();
            PauseStart();
            uiManager.UpdatePlayUI();
        }

        else if (gameState == GamePlayStates.Map)
        {
            uiManager.OverviewUI.MapCameraMove();
            MapEnd();
        }

        else if (gameState == GamePlayStates.Pause)
        {
            StageEndCheack();
            currentStage.ChangeWeather();
            uiManager.PauseUI.PauseUpdate();
            uiManager.UpdatePlayUI();
        }

        else if (gameState == GamePlayStates.Clear)
        {
            GameClear();
        }

        else if (gameState == GamePlayStates.GameOver)
        {
            GameOver();
        }


        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    BGMManager.instance.ChangeBGM(0, 0.04f);
        //    StageInitialize();
        //}


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEnd();
        }


    }



    public void NextStage()
    {
        currentStage.gameObject.SetActive(false);
        Destroy(currentStage.gameObject);

        stageNum++;


        if (stageNum > StageData.StageList.Count - 1)
        {
            stageNum = 0;
        }


        BGMManager.instance.ChangeBGM(0, 0.04f);

        StageInitialize();

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
            cameraController.MapToPlay();
        }

        else if (state == GamePlayStates.Map)
        {
            cameraController.PlayToMap();
        }
    }

    public void MapEnd()
    {
        if (gameState == GamePlayStates.Map)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                uiManager.PlayUIActiveTrue();
                uiManager.OverviewUI.MapCameraReset();
                uiManager.OverviewUI.SetActive(false);
                UIManager.OverviewUI.GameStart();
                gameState = GamePlayStates.Play;
            }
        }
    }

    public void PlaySE(int num, float volume)
    {
        audioSource.volume = volume;
        audioSource.PlayOneShot(audioClipsSE[num]);
    }

    public void PauseStart()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            uiManager.PlayUIActiveFalse();
            uiManager.PauseUI.SetActive(true);
            gameState = GamePlayStates.Pause;
        }
    }

    

    public void GameClear()
    {
        cameraController.Black.SetActive(false);
        cameraController.SignBlack.SetActive(false);

        var gameClearUI = uiManager.GameClearUI;
        gameClearUI.SetActive(true);
        gameClearUI.ScoreCalculate();
    }

    public void GameOver()
    {
        uiManager.GameOverUI.GameOverUpdate();
    }

    public void GameToStageSelect()
    {
        StageData.StageNum = stageNum;
        SceneManager.LoadScene("StageSelectScene");
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
