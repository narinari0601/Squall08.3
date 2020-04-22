using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectManager : MonoBehaviour
{
    [SerializeField, Header("ステージ")]
    private GameObject[] stages = new GameObject[0];
    private int stageValue;

    private int stageNum;

    void Start()
    {
        stageValue = stages.Length;

        stageNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        StageSelect();
    }

    private void StageSelect()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (stageNum == stageValue)
            {
                stageNum = 1;
            }

            else
            {
                stageNum++;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (stageNum == 1)
            {
                stageNum = stageValue;
            }

            else
            {
                stageNum--;
            }
        }
    }

    private void GoToPlay()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

        }
    }
}
