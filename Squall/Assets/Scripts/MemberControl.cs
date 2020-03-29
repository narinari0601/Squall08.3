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


    public enum MemberCheck//メンバーがつかまっているかの状態
    {
        isLoitering,//徘徊してる
        isCapture,//捕まえた
        isDead,//死んだ
    }
    private MemberCheck memberCheck;
    public MemberCheck GetMemberCheck { get => memberCheck; set => memberCheck = value; }


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        temperature = 100;

        if (GetMemberCheck == MemberCheck.isLoitering)
        {
            WeratherCheck();
            if (!member.pathPending && member.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
        }
        else
        {
            MoveToPlayer();
        }

        
    }

    public void Initialize()
    {
        memberStates = MemberStates.isAlive;
        member = gameObject.GetComponent<NavMeshAgent>();
        member.autoBraking = false;
        GotoNextPoint();
    }

    public void MoveToPlayer()
    {
        //目的地と自分の距離
        Vector3 dir = GamePlayManager.instance.Player.transform.position - this.transform.position;
        //目的地の位置
        Vector3 pos = this.transform.position + dir * 1.5f;
        //目的地の方向を向く
        this.transform.rotation = Quaternion.LookRotation(dir);
        //目的地を指定する
        member.destination = pos;
        //目的地からどれくらい離れて停止するか
        member.stoppingDistance = 3.0f;
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
        if(other.gameObject.tag == "Player")
        {
            memberCheck = MemberCheck.isCapture;
        }
        
    }


    
}
