using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField, Header("ステージ")]
    private GameObject[] stagePrefabs = new GameObject[0];

    private int stageValue;

    public static int stageNum = 0;

    //選択関連
    [SerializeField, Header("選択カーソル")]
    private Image cursolImage = null;

    private RectTransform cursolRect;

    [SerializeField, Header("選択オブジェクト達")]
    private GameObject[] selectObjcts = new GameObject[0];

    private RectTransform[] selectRects;

    private Vector3 cursolDelay;

    void Start()
    {
        stageValue = stagePrefabs.Length;

        selectRects = new RectTransform[stageValue];

        for (int i = 0; i < stageValue-1; i++)
        {
            selectRects[i] = selectObjcts[i].transform as RectTransform;
        }

        cursolRect = cursolImage.transform as RectTransform;

        cursolDelay = new Vector3(-100, 0, 0);

        if (!BGMManager.instance.SameBGM(1))
        {
            BGMManager.instance.ChangeBGM(1, 0.07f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        StageSelect();

        StageSelectToGame();

        StageSelectToTitle();
    }

    private void StageSelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (stageNum == stageValue - 2)
            {
                stageNum = 0;
            }

            else
            {
                stageNum++;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (stageNum == 0)
            {
                stageNum = stageValue - 2;
            }

            else
            {
                stageNum--;
            }
        }

        cursolRect.transform.position = selectRects[stageNum].transform.position + cursolDelay;

    }

    private void StageSelectToGame()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // イベントに登録
            //SceneManager.sceneLoaded += GameSceneLoaded;

            // シーン切り替え
            SceneManager.LoadScene("GameScene");
        }
    }

    private void StageSelectToTitle()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneManager.LoadScene("TitleScene");
        }
    }

    private void GameSceneLoaded(Scene next, LoadSceneMode mode)
    {
        // シーン切り替え後のスクリプトを取得
        var gameManager = GameObject.Find("GamePlayManager").GetComponent<GamePlayManager>();

        // データを渡す処理
        //gameManager.score = 100;

        // イベントから削除
        SceneManager.sceneLoaded -= GameSceneLoaded;
    }
}
