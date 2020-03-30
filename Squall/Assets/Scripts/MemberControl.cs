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

    public GameObject player;
    MemberList script;

    public float navSpeed;
    public float navStop;
    private int memberNumber;


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
        isHub,//拠点
    }
    private MemberCheck memberCheck;
    public MemberCheck GetMemberCheck { get => memberCheck; set => memberCheck = value; }


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
        //プレイヤーを追うためのやつ
        def = transform.localRotation.eulerAngles;
        //
        script = player.GetComponent<MemberList>();
        member.speed += navSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(memberNumber);
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
            PlayerFollows();
            
        }
        else if (GetMemberCheck == MemberCheck.isHub)
        {
            //今のところ何もしない
        }
        else//死んだときの処理を書くところ
        {

        }     
    }

    public void PlayerFollows()//仲間がドラクエの隊列みたいにPlayerを追いかける
    {
        for (int i = 1; i <= script.memberList.Count; i++)//捕まえたメンバーの数だけ回す
        {
            if (memberNumber == i)
            {
                member.destination = script.memberList[i - 1].transform.position;
                member.stoppingDistance = navStop;//Playerが止まってからどれくらいで止まるか、
            }
        }

        //スプライトの回転をなくす
        Vector3 parent = this.transform.parent.transform.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(def - parent);
    }



    //public void MoveToPlayer()//今のところプレイヤーを追いかける
    //{
    //    //目的地と自分の距離
    //    Vector3 dir = GamePlayManager.instance.Player.transform.position - this.transform.position;
    //    //目的地の位置
    //    Vector3 pos = this.transform.position + dir * 1.5f;
    //    //目的地の方向を向く
    //    this.transform.rotation = Quaternion.LookRotation(dir);
    //    //目的地を指定する
    //    member.destination = pos;
    //    //目的地からどれくらい離れて停止するか
    //    member.stoppingDistance = 2.0f;


       

    //}

    public void MemberToPlayer()//仲間がPlayerにつかまってから川を避けなくする
    {
        member.areaMask |= 1 << NavMesh.GetAreaFromName("River");
        member.SetAreaCost(NavMesh.GetAreaFromName("River"), 1.0f);
    }

    public void GotoNextPoint()
    {
        if (points.Length == 0)
            return;
        //ぐるぐる回るやつ
        member.destination = points[destPoint].transform.position;
        destPoint = (destPoint + 1) % points.Length;
    }

    public void WeratherCheck()//天気を確認()
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

    public void MemberHubCheck()
    {
        if(GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Map)
        {
            if (GetMemberCheck == MemberCheck.isCapture)
            {
                memberCheck = MemberCheck.isHub;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && GetMemberCheck == MemberCheck.isLoitering)
        {
            member.isStopped = false;
            memberCheck = MemberCheck.isCapture;
            MemberToPlayer();
            script.memberList.Add(this.gameObject);
            memberNumber = script.memberList.Count - 1;
            Debug.Log("当たった");
            

        }
        
    }


    
}
