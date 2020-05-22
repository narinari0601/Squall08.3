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


    //操作説明関連
    [SerializeField, Header("スタート操作")]
    private GameObject startPanel = null;
    [SerializeField, Header("外に出る操作")]
    private GameObject outPanel = null;

    private bool isStart;


    //風関連
    [SerializeField, Header("1番目の風")]
    private Text firstWind = null;

    [SerializeField, Header("2番目の風")]
    private Text secondWind = null;

    [SerializeField, Header("3番目の風")]
    private Text thirdWind = null;


    //シーンネーム関連
    [SerializeField,Header("マップ表示中のパネル")]
    private GameObject scenePanel = null;

    [SerializeField,Header("ステージ名テキスト")]
    private Text stageNameText = null;

    private RectTransform scenePanelRect;

    private Vector3 initScenePos;
    private Vector3 initSceneScale;

    private Vector3 endScenePos;
    private Vector3 endSceneScale;
    private bool isScenePanelMoveEnd = false;
    private float moveTimer;

    //スコアパネル関連
    [SerializeField,Header("星3つテキスト")]
    private Text threeStarsText = null;

    //[SerializeField,Header("ハイスコアテキスト")]
    //private Text highScoreText = null;

    [SerializeField,Header("現在のポイントテキスト")]
    private Text currentScoreText = null;


    [SerializeField,Header("フェード用パネル")]
    private GameObject fadePanel = null;
    private Image fadeImage;
    private float fadeAlpha;


    void Start()
    {
        
    }

    public void Initialize()
    {

        
        mapCamera = GamePlayManager.instance.CameraController.MapCamera;
        initPos = mapCamera.transform.position;
        startPanel.SetActive(true);
        outPanel.SetActive(false);
        isStart = false;
        scenePanelRect = scenePanel.transform as RectTransform;

        SetActive(true);

        if (!isScenePanelMoveEnd)
        {
            initScenePos = scenePanelRect.transform.position;
            initSceneScale = new Vector3(1, 1, 1);
            endScenePos = new Vector3(140.0f, 640.0f, 0.0f);
            endSceneScale = new Vector3(0.4f, 0.4f, 0.4f);
            //initScenePos = RectTransformUtility.WorldToScreenPoint(Camera.main, scenePanel.transform.position);

            scenePanelRect.transform.position = initScenePos;

            moveTimer = 1.0f;
            fadeAlpha = 0.9f;
            fadeImage = fadePanel.GetComponent<Image>();
        }

        stageNameText.text = "ステージ" + (StageData.StageNum + 1).ToString();

        ChangeScore();
        
    }


    void Update()
    {
        if (isStart)
        {
            startPanel.SetActive(false);
            outPanel.SetActive(true);
        }

        ScenePanelMove();

        ChangeScore();
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

    public void GameStart()
    {
        if(!isStart)
        isStart = true;
    }

    public void ScenePanelMove()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            scenePanelRect.transform.position = initScenePos;
            scenePanelRect.transform.localScale = initSceneScale;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            scenePanelRect.transform.position = endScenePos;
            scenePanelRect.transform.localScale = endSceneScale;
        }

        moveTimer -= Time.deltaTime;

        if (moveTimer > 0)
            return;


        if (scenePanelRect.transform.position.x < endScenePos.x && scenePanelRect.transform.position.y > endScenePos.y)
        {
            scenePanelRect.transform.position = endScenePos;
            isScenePanelMoveEnd = true;
        }

        if (!isScenePanelMoveEnd)
        {
            fadeAlpha -= 0.02f;
            fadeImage.color = new Color(0, 0, 0, fadeAlpha);
            var pos = (endScenePos - initScenePos) / 60;
            var scale = 0.01f;
            scenePanelRect.transform.position += pos;
            scenePanelRect.transform.localScale -= new Vector3(scale, scale, 0);
        }
    }

    private void ChangeScore()
    {
        var currentStage = GamePlayManager.instance.CurrentStage;

        threeStarsText.text = "★★★スコア : " + currentStage.ThreeStarsScore;
        //highScoreText.text = "ハイスコア : " + StageData.HighScores[StageData.StageNum];
        currentScoreText.text = "今のスコア : " + currentStage.CurrentScore;
    }
}
