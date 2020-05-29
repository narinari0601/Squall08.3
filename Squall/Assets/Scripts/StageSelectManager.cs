using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectManager : MonoBehaviour
{
    public static StageSelectManager instance = null;


    [SerializeField,Header("音源")]
    private AudioClip[] clips = new AudioClip[0];

    private AudioSource audioSource;


    [SerializeField, Header("ステージ")]
    private GameObject[] stagePrefabs = new GameObject[0];
    
    private  List<GameObject> stageList;

    private int stageValue;

    private int stageNum = 0;

    private int releasedStageValue;

    //選択関連
    [SerializeField, Header("選択カーソル")]
    private Image cursolImage = null;

    private RectTransform cursolRect;

    [SerializeField,Header("選択パネルUI")]
    private GameObject stageSelectPanelObj = null;

    private StageSelectPanelUI selectPanelUI;

    //[SerializeField, Header("選択オブジェクト達")]
    private GameObject[] selectObjcts;

    private RectTransform[] selectRects;

    private int selectedValue;  //選択できるステージの数

    private Vector3 cursolDelay;

    private bool isControl;  //操作できるならtrue

    [SerializeField,Header("確認用")]
    private Dictionary<int, bool> releasedValues;

    public List<GameObject> StageList { get => stageList; }
    public int StageValue { get => stageValue; }
    public int StageNum { get => stageNum;}
    public GameObject[] SelectObjcts { get => selectObjcts; set => selectObjcts = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        Initialize();
    }
    

    private void Initialize()
    {
        StageData.SetStagePrefab(stagePrefabs);
        StageData.DataInitialize();

        stageList = StageData.StageList;

        stageNum = StageData.StageNum;

        stageValue = stageList.Count;

        releasedStageValue = StageData.releasedStageValue();

        selectObjcts = new GameObject[StageValue];

        selectPanelUI = stageSelectPanelObj.GetComponent<StageSelectPanelUI>();
        selectPanelUI.Initialize();


        selectRects = new RectTransform[stageValue];

        for (int i = 0; i < stageValue; i++)
        {
            selectRects[i] = selectObjcts[i].transform as RectTransform;
        }


        cursolRect = cursolImage.transform as RectTransform;

        //菅原変更
        cursolDelay = new Vector3(0, 0, 0);

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


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEnd();
        }


        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StageData.ReleaseAllStage();
            releasedStageValue = StageData.releasedStageValue();
        }
    }

    private void StageSelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (stageNum == releasedStageValue-1)
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
                stageNum = releasedStageValue - 1;
            }

            else
            {
                stageNum--;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (stageNum / 5 == 0 || releasedStageValue < 5)
            {
                return;
            }

            stageNum -= 5;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (stageNum / 5 == 1 || releasedStageValue < 5)
                return;

            stageNum += 5;
        }

        cursolRect.transform.position = selectRects[stageNum].transform.position + cursolDelay;

    }

    private void StageSelectToGame()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // イベントに登録
            //SceneManager.sceneLoaded += GameSceneLoaded;

            audioSource.PlayOneShot(clips[0]);
            isControl = false;
            StageData.StageNum = stageNum;

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

    private void GameEnd()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
