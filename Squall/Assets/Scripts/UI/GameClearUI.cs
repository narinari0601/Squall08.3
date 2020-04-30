using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameClearUI : MonoBehaviour
{
    [SerializeField, Header("ゲームクリア全体のパネル")]
    private GameObject gameClearPanel = null;


    //スコア関連
    [SerializeField, Header("スコア用のパネル")]
    private GameObject scorePanel = null;

    [SerializeField, Header("スコアのテキスト")]
    private Text scoreNumberText = null;

    private float stageScore;

    private float currentScore;

    private string scoreText;

    //星関連
    [SerializeField, Header("星判定用のパネル")]
    private GameObject starPanel = null;

    [SerializeField, Header("星のテキスト")]
    private Text starText = null;


    //選択関連
    [SerializeField, Header("選択用のパネル")]
    private GameObject selectPanel = null;

    [SerializeField, Header("選択カーソル")]
    private Image cursolImage = null;

    private RectTransform cursolRect;

    [SerializeField, Header("選択オブジェクト達")]
    private GameObject[] selectObjcts = new GameObject[0];

    private RectTransform[] selectRects;

    private int selectNum;

    private int selectMaxValue;

    private Vector3 cursolDelay;



    void Start()
    {
        //Initialize();
    }

    public void Initialize()
    {
        stageScore = 0;
        currentScore = 0;
        scoreText = "";

        selectNum = 0;
        selectMaxValue = selectObjcts.Length;

        selectRects = new RectTransform[selectMaxValue];

        for (int i = 0; i < selectMaxValue; i++)
        {
            selectRects[i] = selectObjcts[i].transform as RectTransform;
        }

        cursolRect = cursolImage.transform as RectTransform;

        cursolDelay = new Vector3(-330, 0, 0);

        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //CursolMove();
        //ScoreUp();
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            gameClearPanel.SetActive(true);
        }

        else
        {
            gameClearPanel.SetActive(false);
        }
    }

    public void ScoreUp()
    {
        //if (currentScore > GamePlayManager.instance.CurrentStage.CurrentScore)
        //{
        //    currentScore += 20;
        //    scoreText = currentScore.ToString();
        //    scoreNumberText.text = scoreText;

        //}

        stageScore = GamePlayManager.instance.CurrentStage.CurrentScore;
        scoreText = stageScore.ToString();
        scoreNumberText.text = scoreText;
    }

    public void RankJudgment()
    {
        var stage = GamePlayManager.instance.CurrentStage;

        if (stageScore >= stage.ThreeStarsScore)
        {
            starText.text = " ★ ★ ★ ";
        }

        else if (stageScore >= stage.TwoStarsScore)
        {
            starText.text = "  ★  ★  ";
        }

        else 
        {
            starText.text = "    ★    ";
        }
    }
    

    public void CursolMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectNum == selectMaxValue - 1)
            {
                selectNum = 0;
            }

            else
            {
                selectNum++;
            }
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectNum == 0)
            {
                selectNum = selectMaxValue - 1;
            }

            else
            {
                selectNum--;
            }
        }
        

        cursolRect.transform.position = selectRects[selectNum].transform.position + cursolDelay;
    }

    public void NextScene()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectNum == 0)
            {
                GamePlayManager.instance.NextStage();
            }

            else if (selectNum == 1)
            {
                GamePlayManager.instance.StageInitialize();
            }

            else if (selectNum == 2)
            {
                //SceneManager.LoadScene("StageSelectScene");
            }
        }
    }
}
