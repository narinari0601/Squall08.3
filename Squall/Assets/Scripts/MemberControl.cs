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
    //プレイヤーを追うための
    private Vector3 def;

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
        //Memberの処理分岐
        if (GetMemberCheck == MemberCheck.isLoitering)//徘徊しているときの処理書くところ
        {
            WeratherCheck();
            if (!member.pathPending && member.remainingDistance < 0.5f)//ぐるぐる回るやつ
            {
                GotoNextPoint();
            }
        }
        else if(GetMemberCheck == MemberCheck.isCapture)//プレイヤーに捕まったらの処理書くところ
        {
            MoveToPlayer();
        }
        else//死んだときの処理を書くところ
        {

        }     
    }

    public void Initialize()
    {
        memberStates = MemberStates.isAlive;
        member = gameObject.GetComponent<NavMeshAgent>();
        member.autoBraking = false;
        GotoNextPoint();
        //プレイヤーを追うためのやつ
        def = transform.localRotation.eulerAngles;
    }

    public void MoveToPlayer()//今のところプレイヤーを追いかける
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
        member.stoppingDistance = 2.0f;


        //スプライトの回転をなくす
        Vector3 parent = this.transform.parent.transform.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(def - parent);

    }

    public void MemberToPlayer()
    {
        member.areaMask |= 1 << NavMesh.GetAreaFromName("River");
    }

    public void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        //ぐるぐる回るやつ
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
            member.isStopped = false;
            memberCheck = MemberCheck.isCapture;
            MemberToPlayer();
        }
        
    }


    
}
