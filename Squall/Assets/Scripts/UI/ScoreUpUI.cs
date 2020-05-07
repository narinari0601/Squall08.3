using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUpUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel = null;

    [SerializeField]
    private Text scoreUpText = null;

    private RectTransform scoreTextRect;

    private float currentActiveTimer;

    private const float MAX_ACTIVE_TIME = 2.0f;

    //private Vector3 basePos;

    private Vector2 delayPos;

    void Start()
    {

    }

    public void Initialize()
    {
        currentActiveTimer = 0;
        scoreUpText.text = "";
        SetActive(false);
        scoreTextRect = scoreUpText.transform as RectTransform;
        //basePos = RectTransformUtility.WorldToScreenPoint(Camera.main, GamePlayManager.instance.UIManager.BasedirectionUI.BaseCamp.transform.position);
        delayPos = new Vector2(-8, 10);
    }

    void Update()
    {
        MoveUI();
    }

    public void SetActive(bool value)
    {
        if (value)
        {
            panel.SetActive(true);
        }

        else
        {
            panel.SetActive(false);
        }
    }

    public void ScoreUp(float score)
    {

        scoreUpText.text = "+" + score;

        SetActive(true);
    }

    private void MoveUI()
    {
        if (panel.activeSelf)
        {
            var basePos = RectTransformUtility.WorldToScreenPoint(Camera.main, GamePlayManager.instance.UIManager.BasedirectionUI.BaseCamp.transform.position);
            delayPos += new Vector2(0, 0.15f);
            basePos += delayPos;

            scoreTextRect.transform.position = basePos;

            currentActiveTimer += Time.deltaTime;

            if (currentActiveTimer > MAX_ACTIVE_TIME)
            {
                Initialize();
            }
        }
    }
}
