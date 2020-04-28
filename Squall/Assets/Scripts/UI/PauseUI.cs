using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    [SerializeField,Header("大元のパネル")]
    private GameObject pausePanel = null;

    [SerializeField, Header("選択カーソル")]
    private Image cursolImage = null;

    [SerializeField, Header("選択カーソルが合わさるものたち")]
    private GameObject[] selectedObjcts = new GameObject[0];

    private RectTransform cursolPos;

    private int selectNum;

    private int selectValue;


    void Start()
    {
        
    }

    public void Initialize()
    {
        cursolPos = cursolImage.transform as RectTransform;
        selectNum = 0;
        selectValue = selectedObjcts.Length;
        SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseUpdate()
    {
        CursolMove();

        Select();

        if (Input.GetKeyDown(KeyCode.P))
        {
            ReturnToGame();
        }
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            pausePanel.SetActive(true);
        }

        else
        {
            pausePanel.SetActive(false);
        }
    }

    public void CursolMove()
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
                selectNum = selectValue - 1;
            }

            else
            {
                selectNum--;
            }
        }

        Vector3 pos = new Vector3(cursolPos.position.x, (selectedObjcts[selectNum].transform as RectTransform).position.y, 0);
        cursolPos.position = pos;
    }

    public void Select()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (selectNum == 0)
            {
                ReturnToGame();
            }

            else if (selectNum == 1)
            {
                ResetGame();
            }

            else if (selectNum == 2)
            {
                EndGame();
            }
        }
    }

    private void ReturnToGame()
    {
        Initialize();
        GamePlayManager.instance.GameState = GamePlayManager.GamePlayStates.Play;
        SetActive(false);
    }

    private void ResetGame()
    {
        GamePlayManager.instance.RetryScene();
    }

    private void EndGame()
    {

    }
}
