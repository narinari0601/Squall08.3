using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
    [SerializeField, Header("文字パネル")]
    private GameObject textPanel = null;

    [SerializeField, Header("ステージ番号")]
    private Text stageNumText = null;

    [SerializeField, Header("ランクテキスト")]
    private Text rankText = null;

    [SerializeField,Header("南京錠イメージ")]
    private GameObject padlockImage = null;

    RectTransform rectTransform;

    private int panelNum;

    private bool isReleased;

    void Start()
    {
        //rectTransform = GetComponent<RectTransform>();
        //rectTransform.anchoredPosition = new Vector2(100,400);
    }

    public void Initialize(int stageNum,string rank)
    {
        
        stageNumText.text = stageNum.ToString();
        rankText.text = rank;


        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(180 + ((stageNum - 1) % 5) * 230, 420 - ((stageNum - 1) / 5) * 200);

        isReleased = false;
        panelNum = stageNum - 1;

        textPanel.SetActive(false);
        padlockImage.SetActive(true);
    }
    
    void Update()
    {
        if (!isReleased)
        {
            isReleased = StageData.StagePlayFlags[panelNum];

            if (isReleased)
            {
                padlockImage.SetActive(false);
                textPanel.SetActive(true);
            }
        }
    }
}
