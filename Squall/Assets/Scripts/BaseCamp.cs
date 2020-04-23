using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCamp : MonoBehaviour
{

    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
       
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
}
