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

    [SerializeField, Header("カリキュレイトパネル")]
    private GameObject calculatePanel = null;

    [SerializeField, Header("新しいほうのスコアテキスト")]
    private Text stageScoreText = null;

    private List<float> scoreList;

    private RectTransform scoreTextRect;

    private Vector3 initScoreTextPos;

    [SerializeField, Header("ボーナスのほうのスコアテキスト")]
    private Text bonusScoreText = null;

    private List<float> bonusScoreList;

    private RectTransform bonusScoreRect;

    private Vector3 initBonusScoreTextPos;

    private Vector3 halfScorePos;  //2つのテキストの中間

    [SerializeField, Header("トータルスコアパネル")]
    private GameObject totalScorePanel = null;

    [SerializeField, Header("トータルスコア")]
    private Text totalScoreText = null;

    private RectTransform totalScoreRect;

    private bool isScoreCalculate;   //計算が終わったらtrue

    private const float CALCULATE_TIME = 3.0f;

    private float currentCalculateTimer;

    private bool isScoreMove;
    private bool isScoreMoveEnd;

    private Vector3 scoreMoveEndPos;
    private Vector3 initScoreMovePos;

    private const float SCORE_MOVE_TIME = 1.0f;
    private float currentScoreMoveTimer;

    [SerializeField, Header("マルチプルスコアパネル")]
    private GameObject mulScorePanel = null;

    [SerializeField,Header("掛けたスコアテキスト達")]
    private Text[] mulTexts = new Text[0];

    [SerializeField, Header("新しいほうの掛けたスコアテキスト")]
    private Text mulScoreText = null;

    [SerializeField,Header("イコールのテキスト")]
    private Text mulEqualText = null;

    private RectTransform[] mulTextRects;

    private float[] mulScores;  //掛けた計算結果が入る配列

    private Vector3[] mulRectPostions;

    private const float MUL_TIME = 1.0f;

    private float currentMulTimer;

    private bool isMul;

    private const float SUM_TIME = 1.0f;

    private float currentSumTimer;

    private const float SUM_STOP_TIME = 1.0f;

    private float currentSumStopTimer;

    

    private bool isSum;


    //星関連
    [SerializeField, Header("星判定用のパネル")]
    private GameObject starPanel = null;

    [SerializeField, Header("星のテキスト")]
    private Text starText = null;
    


    //選択関連
    [SerializeField, Header("選択用のパネル")]
    private GameObject selectPanel = null;

    [SerializeField, Header("選択カーソル")]
    private GameObject cursolImage = null;

    private RectTransform cursolRect;

    [SerializeField, Header("選択オブジェクト達")]
    private GameObject[] selectObjcts = new GameObject[0];

    private RectTransform[] selectRects;

    private int selectNum;

    private int selectMaxValue;

    [SerializeField,Header("ラストステージ用選択パネル")]
    private GameObject lastSelectPanel = null;

    [SerializeField, Header("ラスト用選択カーソル")]
    private GameObject lastCursolImage = null;

    private RectTransform lastCursolRect;

    [SerializeField,Header("ラストステージ用選択オブジェクト")]
    private GameObject[] lastSelectObjct = new GameObject[0];

    private RectTransform[] lastSelectRects;

    private int lastSelectValue;
    



    void Start()
    {
        //Initialize();
    }

    public void Initialize()
    {
        //スコア関連
        stageScore = 0;
        currentScore = 0;
        scoreText = "";

        totalScoreRect = totalScoreText.GetComponent<RectTransform>();

        scoreList = new List<float>();
        bonusScoreList = new List<float>();
        stageScoreText.text = "";
        bonusScoreText.text = "";

        scoreTextRect = stageScoreText.GetComponent<RectTransform>();
        //initScoreTextPos = new Vector3(422.9f, 280.9f, 0);
        initScoreTextPos = new Vector3(398.7f, 246.9f, 0);
        //scoreTextRect.position = initScoreTextPos;

        bonusScoreRect = bonusScoreText.GetComponent<RectTransform>();
        //initBonusScoreTextPos = new Vector3(817.9f, 281.8f, 0);
        initBonusScoreTextPos = new Vector3(845.0f, 248.0f, 0);
        //bonusScoreRect.position = initBonusScoreTextPos;

        mulScoreText.text = "";
        mulEqualText.text = "";


        //halfScorePos = (initScoreTextPos + initBonusScoreTextPos) / 2;
        halfScorePos = new Vector3(totalScoreRect.position.x, (initScoreTextPos.y + initBonusScoreTextPos.y) / 2, 0);

        isScoreCalculate = false;
        currentCalculateTimer = 0;

        

        isScoreMove = false;
        isScoreMoveEnd = false;
        scoreMoveEndPos = new Vector3(640, 460, 0);
        initScoreMovePos = new Vector3(993.7f, 67.5f, 0);
        totalScoreRect.position = initScoreMovePos;
        totalScoreRect.localScale = new Vector3(1.5f, 1.5f, 0.5f);

        currentScoreMoveTimer = SCORE_MOVE_TIME;

        mulRectPostions = new Vector3[4]
            {
                new Vector3(totalScoreRect.position.x,403.9f,0),
                new Vector3(totalScoreRect.position.x,301.2f,0),
                new Vector3(totalScoreRect.position.x,195.6f,0),
                new Vector3(totalScoreRect.position.x,91.4f,0),
            };

        //mulTextRects = new RectTransform[mulTexts.Length];

        //for (int i = 0; i < mulTexts.Length; i++)
        //{
        //    mulTexts[i].text = "";
        //    mulTextRects[i] = mulTexts[i].GetComponent<RectTransform>();
        //    //Debug.Log(mulTextRects[i].position);
        //    mulTextRects[i].position = mulRectPostions[i];
        //}

        mulScores = new float[4] { 0, 0, 0, 0 };

        currentMulTimer = MUL_TIME;

        isMul = false;

        currentSumTimer = SUM_TIME;
        currentSumStopTimer = SUM_STOP_TIME;

        isSum = false;

        //選択関連
        selectNum = 0;
        selectMaxValue = selectObjcts.Length;

        selectRects = new RectTransform[selectMaxValue];

        for (int i = 0; i < selectMaxValue; i++)
        {
            selectRects[i] = selectObjcts[i].transform as RectTransform;
        }

        cursolRect = cursolImage.transform as RectTransform;

        lastSelectValue = lastSelectObjct.Length;
        lastSelectRects = new RectTransform[lastSelectValue];

        for (int i = 0; i < lastSelectValue; i++)
        {
            lastSelectRects[i] = lastSelectObjct[i].transform as RectTransform;
        }
        lastCursolRect = lastCursolImage.transform as RectTransform;
        
        scorePanel.SetActive(true);
        starPanel.SetActive(false);
        selectPanel.SetActive(false);
        lastSelectPanel.SetActive(false);
        calculatePanel.SetActive(true);
        mulScorePanel.SetActive(true);
        totalScorePanel.SetActive(true);

        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //CursolMove();
        //ScoreUp();

        if (isScoreCalculate)
        {
            currentCalculateTimer += Time.deltaTime;

            if (currentCalculateTimer > CALCULATE_TIME)
            {
                scorePanel.SetActive(false);
                //isScoreMove = true;
                //isMul = true;
                isScoreMove = true;
            }
        }

        MoveTotalScore();
        ScoreMultiple();
        SumScore();
        RankJudgment();

        var stageNum = GamePlayManager.instance.StageNum;

        if (stageNum == 9)
        {
            LastSelectMove();
        }

        else
        {
            SelectMove();
        }
        
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

    public void ScoreUp(float normalScore, float bonusScore)
    {
        scoreList.Add(normalScore);
        bonusScoreList.Add(bonusScore);
    }

    public void ScoreCalculate()
    {
        //if (currentScore > GamePlayManager.instance.CurrentStage.CurrentScore)
        //{
        //    currentScore += 20;
        //    scoreText = currentScore.ToString();
        //    scoreNumberText.text = scoreText;

        //}

        //stageScore = GamePlayManager.instance.CurrentStage.CurrentScore;
        //scoreText = stageScore.ToString();
        //scoreNumberText.text = scoreText;

        //string scoreStr;

        if (isScoreCalculate)
            return;

        foreach (var score in scoreList)
        {
            string str = score.ToString() + "\n";
            stageScoreText.text += str;
        }

        foreach (var bonus in bonusScoreList)
        {
            bonusScoreText.text += "×" + bonus.ToString("0.0") + "\n";
        }

        totalScoreText.text = GamePlayManager.instance.CurrentStage.CurrentScore.ToString();

        for (int i = 0; i < scoreList.Count; i++)
        {
            string scoreStr = (scoreList[i] * bonusScoreList[i]).ToString() + "\n";
            mulScoreText.text += scoreStr;

            string equal = "=\n";
            mulEqualText.text += equal;
        }

        //for (int i = 0; i < scoreList.Count; i++)
        //{
        //    mulScores[i] = scoreList[i] * bonusScoreList[i];
        //}

        //for (int i = 0; i < mulScores.Length; i++)
        //{
        //    string scoreStr;
        //    float score = mulScores[i];

        //    if (score == 0)
        //    {
        //        scoreStr = "";
        //    }

        //    else
        //    {
        //        scoreStr = score.ToString();
        //    }

        //    mulTexts[i].text = scoreStr;
        //}
        isScoreCalculate = true;
    }

    private void ScoreMultiple()
    {
        if (!isMul)
            return;

        currentMulTimer -= Time.deltaTime;

        var plusPos = (halfScorePos - initScoreTextPos) / 90;
        var minusPos = (halfScorePos - initBonusScoreTextPos) / 90;

        scoreTextRect.position += plusPos;
        bonusScoreRect.position += minusPos;

        if (currentMulTimer < 0)
        {
            isMul = false;
            isSum = true;

            calculatePanel.SetActive(false);
            mulScorePanel.SetActive(true);
        }

    }

    private void SumScore()
    {
        if (!isSum)
            return;

        currentSumStopTimer -= Time.deltaTime;

        if (currentSumStopTimer < 0)
        {
            currentSumTimer -= Time.deltaTime;

            for (int i = 0; i < mulTextRects.Length; i++)
            {
                var offset = (totalScoreRect.position - mulTextRects[i].position) / 60;
                mulTextRects[i].position += offset;
            }

            if (currentSumTimer < 0)
            {
                isSum = false;
                mulScorePanel.SetActive(false);
                totalScorePanel.SetActive(true);
                isScoreMoveEnd = true;
            }
        }

    }

    public void MoveTotalScore()
    {
        if (isScoreMove)
        {

            if (totalScoreRect.position.x < scoreMoveEndPos.x && totalScoreRect.position.y > scoreMoveEndPos.y)
            {
                totalScoreRect.position = scoreMoveEndPos;
                isScoreMoveEnd = true;

            }

            if (!isScoreMoveEnd)
            {
                var pos = (scoreMoveEndPos - initScoreMovePos) / 60;
                totalScoreRect.transform.position += pos;
                var scale = 0.02f;
                totalScoreRect.transform.localScale += new Vector3(scale, scale, 0);
            }
        }
    }

    public void RankJudgment()
    {
        if (!isScoreMoveEnd)
            return;

        var stage = GamePlayManager.instance.CurrentStage;

        stageScore = stage.CurrentScore;

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

        starPanel.SetActive(true);
    }
    

    public void SelectMove()
    {
        if (!isScoreMoveEnd)
            return;

        selectPanel.SetActive(true);

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
        

        cursolRect.transform.position = selectRects[selectNum].transform.position;

        NextScene();
    }

    public void LastSelectMove()
    {
        if (!isScoreMoveEnd)
            return;

        lastSelectPanel.SetActive(true);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectNum == lastSelectValue - 1)
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
                selectNum = lastSelectValue - 1;
            }

            else
            {
                selectNum--;
            }
        }


        lastCursolRect.transform.position = lastSelectRects[selectNum].transform.position;

        LastNextScene();
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
                BGMManager.instance.ChangeBGM(0, 0.04f);
                GamePlayManager.instance.StageInitialize();
            }

            else if (selectNum == 2)
            {
                GamePlayManager.instance.GameToStageSelect();
            }
        }
    }

    public void LastNextScene()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectNum == 0)
            {
                BGMManager.instance.ChangeBGM(0, 0.04f);
                GamePlayManager.instance.StageInitialize();
            }

            else if (selectNum == 1)
            {
                GamePlayManager.instance.GameToStageSelect();
            }
            
        }
    }
}
