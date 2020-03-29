using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MemberControl : MonoBehaviour
{
    public NavMeshAgent member;
    public Transform[] points;
    private int destPoint = 0;
    private float temperature = 100;

    public enum MemberStates//メンバーの状態
    {
        isAlive,//生きてる
        isDaed,//死んでる
        isHub//拠点
    }

    private MemberStates memberStates;//メンバーの状態
    public MemberStates GetMemberState { get => memberStates; set => memberStates = value; }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        memberStates = MemberStates.isAlive;
        member = gameObject.GetComponent<NavMeshAgent>();
        member.autoBraking = false;
        GotoNextPoint();
    }

    public void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        member.destination = points[destPoint].transform.position;
        destPoint = (destPoint + 1) % points.Length;
    }

    public void WeratherCheck()
    {
        if(GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            member.isStopped = true;
            temperature -= 0.5f;
            
        }
        else
        {
            member.isStopped = false;
            temperature += 0.5f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("二階堂真紅");
        
    }


    // Update is called once per frame
    void Update()
    {
        temperature = 100;
        WeratherCheck();

        if (!member.pathPending && member.remainingDistance < 0.5f)
        {
            GotoNextPoint();
        }
    }
}
