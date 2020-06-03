using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
    [SerializeField, Header("ロック解除状態のパネル")]
    private GameObject unLockPanel = null;

    [SerializeField, Header("ロック状態のパネル")]
    private GameObject lockPanel = null;

    [SerializeField, Header("ステージ番号")]
    private Text stageNumText = null;

    [SerializeField, Header("ランクテキスト")]
    private Text rankText = null;

    [SerializeField,Header("ロック時のステージ番号")]
    private Text lockStageNum = null;

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
        lockStageNum.text= stageNum.ToString();
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

        unLockPanel.SetActive(false);
        //padlockImage.SetActive(true);
        lockPanel.SetActive(true);
    }

    void Update()
    {
        if (!isReleased)
        {
            isReleased = StageData.StagePlayFlags[panelNum];

            if (isReleased)
            {
                //padlockImage.SetActive(false);
                lockPanel.SetActive(false);
                unLockPanel.SetActive(true);
            }
        }
    }
}
