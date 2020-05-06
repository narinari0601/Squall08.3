using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp : MonoBehaviour
{
    
    private const string MAIN_CAMERA_TAG_NAME = "MainCamera";

    //カメラに表示されているか
    public bool _isRendered = false;

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
       
        _isRendered = false;
    }

    public void Initialize()
    {
        _isRendered = false;
    }

    public void TouchBaseCamp()
    {
        //GamePlayManager.instance.CurrentStage.ScoreUp();

        //foreach (var member in GamePlayManager.instance.CurrentStage.MemberControllers)
        //{
        //    member.MemberHubCheck();
        //}

        //GamePlayManager.instance.StageEndCheack();

        //if (GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Play)
        //{
        //    GamePlayManager.instance.GameState = GamePlayManager.GamePlayStates.Map;
        //}

        var currentStage = GamePlayManager.instance.CurrentStage;

        if (currentStage.PlayerController.MemberList.memberList.Count < 2)
        {
            var uiMaanager = GamePlayManager.instance.UIManager;

            uiMaanager.OverviewUIAllSetActive(true);
            uiMaanager.PlayUIActiveFalse();
            uiMaanager.PauseUI.ResetUI();
            GamePlayManager.instance.GameState = GamePlayManager.GamePlayStates.Map;
        }

        else
        {
            GamePlayManager.instance.CurrentStage.ScoreUp();

            foreach (var member in GamePlayManager.instance.CurrentStage.MemberControllers)
            {
                member.MemberHubCheck();
            }

            GamePlayManager.instance.StageEndCheack();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;

        if (obj.tag == "Player")
        {
            TouchBaseCamp();
        }
    }

    //カメラに映ってる間に呼ばれる
    private void OnWillRenderObject()
    {
        //メインカメラに映った時だけ_isRenderedを有効に
        //if (Camera.current.tag == MAIN_CAMERA_TAG_NAME)
        //{

        //    _isRendered = true;
        //}

        if (Camera.current.name == "MainCamera")
        {
            _isRendered = true;
        }
    }


    public bool IsCameraCheck()
    {
        
        return _isRendered;
        
    }
}
