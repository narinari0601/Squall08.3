using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField,Header("音源")]
    private AudioClip[] clips = new AudioClip[0];

    private AudioSource audioSource;


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

    private bool isControl;  //操作できるならtrue


    void Start()
    {
        stageValue = stagePrefabs.Length;

        selectRects = new RectTransform[stageValue];

        for (int i = 1; i < stageValue; i++)
        {
            selectRects[i] = selectObjcts[i].transform as RectTransform;
        }

        cursolRect = cursolImage.transform as RectTransform;

        cursolDelay = new Vector3(-100, 0, 0);

        isControl = true;


        audioSource = GetComponent<AudioSource>();


        if (!BGMManager.instance.SameBGM(1))
        {
            BGMManager.instance.ChangeBGM(1, 0.07f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isControl)
        {
            StageSelect();

            StageSelectToGame();

            StageSelectToTitle();
        }
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

        cursolRect.transform.position = selectRects[stageNum+1].transform.position + cursolDelay;

    }

    private void StageSelectToGame()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // イベントに登録
            //SceneManager.sceneLoaded += GameSceneLoaded;

            audioSource.PlayOneShot(clips[0]);
            isControl = false;

            StartCoroutine("LoadPreparation");
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

    private IEnumerator LoadPreparation()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audioSource.isPlaying)
            {
                // シーン切り替え
                SceneManager.LoadScene("GameScene");
                break;
            }
        }
    }
}
