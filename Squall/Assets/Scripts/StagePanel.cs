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

    private Color blackColor;

    private Color yellowColor;

    private Outline starOutLine;

    [SerializeField, Header("南京錠イメージ")]
    private GameObject padlockImage = null;

    RectTransform rectTransform;

    private int panelNum;

    private bool isReleased;

    void Start()
    {

    }

    public void Initialize(int stageNum, string rank)
    {

        stageNumText.text = stageNum.ToString();
        rankText.text = rank;
        blackColor = new Color(0, 0f, 0, 1);
        yellowColor = new Color(1, 0.9f, 0, 1);
        starOutLine = rankText.GetComponent<Outline>();

        if (StageData.StageRank(stageNum - 1) == StageData.Ranks[0])
        {
            rankText.color = blackColor;
            starOutLine.effectColor = new Color(1, 1, 1, 0);
        }

        else
        {
            rankText.color = yellowColor;
            starOutLine.effectColor = new Color(0, 0, 0, 0.5f);
        }

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
