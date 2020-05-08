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

    void Start()
    {
        cursolPos = cursolImage.transform as RectTransform;
        selectNum = 0;
        selectValue = selectedObjcts.Length;

        if (!BGMManager.instance.SameBGM(1))
        {
            BGMManager.instance.ChangeBGM(1, 0.07f);
        }

        //Debug.Log((selectedObjcts[0].transform as RectTransform).position);
        //Debug.Log((selectedObjcts[1].transform as RectTransform).position);
        //Debug.Log(cursolPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        CursolMove();

        SelectScene();
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
