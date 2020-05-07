using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField, Header("大元のパネル")]
    private GameObject gameOverPanel = null;

    bool gameoverflag;
    public GameObject panelobj;
    public Text tex;
    Image panel;
    float red, green, blue;
    float alpha;
    //float membercount;
    //float currentmember;

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
        //gameoverflag = true ;
        //panel = panelobj.GetComponent<Image>();
        //red = panel.color.r;
        //green = panel.color.g;
        //blue = panel.color.b;
        //alpha = 0;
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            gameOverPanel.SetActive(true);
        }

        else
        {
            gameOverPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //currentmember =0;
        //foreach(var a in GamePlayManager.instance.CurrentStage.MemberControllers)
        //{
        //    if (a.GetMemberState == MemberControl.MemberStates.isDaed)
        //    {
        //        currentmember++;
        //    }
        //}
        //if (currentmember == membercount )  

        //if (GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.GameOver)
        //{
        //    panel.color = new Color(red, green, blue, alpha);
        //    alpha += 0.025f;
        //    if (alpha > 1)
        //    {
        //        tex.text = "味方が全滅した...";
        //        alpha = 1;
        //        //if (Input.GetKeyDown(KeyCode.A))
        //        //{
        //        //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //        //}
        //    }

        //}
        //else
        //{
        //    alpha--;
        //    if (alpha < 0)
        //    {
        //        alpha = 0;
        //    }
        //}
    }

    public void GameOverUpdate()
    {
        if (!gameOverPanel.activeSelf)
            SetActive(true);

        panel.color = new Color(red, green, blue, alpha);
        alpha += 0.025f;

        if (alpha > 1)
        {
            selectPanel.SetActive(true);
            tex.text = "味方が全滅した...";
            alpha = 1;

            CursolMove();
            NextScene();
        }


    }

    public void Initialize()
    {
        gameoverflag = false;
        panel = panelobj.GetComponent<Image>();
        red = panel.color.r;
        green = panel.color.g;
        blue = panel.color.b;
        alpha = 0;

        //membercount = GamePlayManager.instance.CurrentStage.MemberControllers.Length;

        selectNum = 0;
        selectMaxValue = selectObjcts.Length;

        selectRects = new RectTransform[selectMaxValue];

        for (int i = 0; i < selectMaxValue; i++)
        {
            selectRects[i] = selectObjcts[i].transform as RectTransform;
        }

        cursolRect = cursolImage.transform as RectTransform;

        cursolDelay = new Vector3(-230, 0, 0);

        SetActive(false);

        selectPanel.SetActive(false);
    }

    public void CursolMove()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
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

        else if (Input.GetKeyDown(KeyCode.UpArrow))
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


        var pos = new Vector3(cursolRect.transform.position.x, selectRects[selectNum].transform.position.y, 0);
        cursolRect.transform.position = pos;

        //cursolRect.transform.position = selectRects[selectNum].transform.position + cursolDelay;
    }

    public void NextScene()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectNum == 0)
            {
                GamePlayManager.instance.StageInitialize();
            }

            else if (selectNum == 1)
            {
                GamePlayManager.instance.GameToStageSelect();
            }

        }
    }
}
