using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemberAliveUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel = null;

    [SerializeField]
    private Text memberAliveUI = null;

    void Start()
    {
        
    }

    public void Initialize()
    {
        memberAliveUI.text = "あと" +GamePlayManager.instance.CurrentStage.GetMemberAliveValue() + "人";
        SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        //MemberCount();
    }

    public void MemberCount()
    {
        memberAliveUI.text = "あと" + GamePlayManager.instance.CurrentStage.GetMemberAliveValue() + "人";
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
}
