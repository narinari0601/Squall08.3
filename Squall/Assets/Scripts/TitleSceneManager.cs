using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleSceneManager : MonoBehaviour
{
    [SerializeField,Header("選択カーソル")]
    private Image cursolImage = null;

    [SerializeField,Header("選択カーソルが合わさるものたち")]
    private GameObject[] selectedObjcts = new GameObject[0];

    private RectTransform cursolPos;

    private int selectNum;

    private int selectValue;

    private bool isControl;

    [SerializeField,Header("黒幕")]
    private GameObject blackPanel = null;

    private float blackTimer;


    //音関連
    [SerializeField,Header("音源")]
    private AudioClip[] audioClips = new AudioClip[0];

    private AudioSource audioSource;


    private void Awake()
    {
        // PC向けビルドだったらサイズ変更
        if (Application.platform == RuntimePlatform.WindowsPlayer ||
        Application.platform == RuntimePlatform.OSXPlayer ||
        Application.platform == RuntimePlatform.LinuxPlayer)
        {
            Screen.SetResolution(1280, 720, true);
        }

        Cursor.visible = false;
    }

    void Start()
    {
        cursolPos = cursolImage.transform as RectTransform;
        selectNum = 0;
        selectValue = selectedObjcts.Length;
        isControl = true;

        audioSource = GetComponent<AudioSource>();

        if (!BGMManager.instance.SameBGM(1))
        {
            BGMManager.instance.ChangeBGM(1, 0.07f);
        }

        blackTimer = 0;



        //Debug.Log((selectedObjcts[0].transform as RectTransform).position);
        //Debug.Log((selectedObjcts[1].transform as RectTransform).position);
        //Debug.Log(cursolPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isControl)
        {
            CursolMove();

            SelectScene();
        }

        blackTimer++;
        /*if (blackTimer > 2)
        {
            blackPanel.SetActive(false);
        }*/

        

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameEnd();
        }
    }

    private void CursolMove()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectNum == selectValue - 1)
            {
                selectNum = 0;
            }

            else
            {
                selectNum++;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectNum == 0)
            {
                selectNum = selectValue-1;
            }

            else
            {
                selectNum--;
            }
        }

        Vector3 pos = new Vector3(cursolPos.position.x, (selectedObjcts[selectNum].transform as RectTransform).position.y, 0);
        cursolPos.position = pos;
    }

    private void SelectScene()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectNum == 0)
            {
                TitleToStageSelect();
            }

            else if (selectNum == 1)
            {
                GameEnd();
            }
        }
    }


    public void TitleToStageSelect()
    {
        audioSource.PlayOneShot(audioClips[0]);
        isControl = false;

        StartCoroutine("LoadPreparation");

        //SceneManager.LoadScene("StageSelectScene");
    }

    private IEnumerator LoadPreparation()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            if (!audioSource.isPlaying)
            {
                // シーン切り替え
                SceneManager.LoadScene("StageSelectScene");
                break;
            }
        }
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
