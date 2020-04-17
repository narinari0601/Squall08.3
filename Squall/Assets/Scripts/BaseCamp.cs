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

    public void MapDisplay()
    {
        foreach (var member in GamePlayManager.instance.CurrentStage.MemberControllers)
        {
            member.MemberHubCheck();
        }

        GamePlayManager.instance.StageEndCheack();

        if (GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Play)
        {
            GamePlayManager.instance.GameState = GamePlayManager.GamePlayStates.Map;
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
            MapDisplay();
        }
    }
}
