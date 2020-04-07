using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MemberControl : MonoBehaviour
{
    public NavMeshAgent member;
    public Transform[] points;
    private int destPoint = 0;
    public float memberHp = 100;
    const float MIN = 0;
    const float MAX =100;
    private float memberHpMax;
    //プレイヤーを追うための
    private Vector3 def;
    private GameObject player;
    MemberList script;


    public float navSpeed;
    public float navStop;
    private int memberNumber;
    private Transform memberLingt;
    public Vector3 lightScale;
    public Slider slider;

    //仲間の速さ
    private Vector3 wind;
    private float windpower;
    //波紋
    RippleUI rippleUI;


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
        memberHpMax = memberHp;//最大値の固定
        player = GameObject.Find("Player");
        memberLingt = this.gameObject.transform.Find("MemberLight");
        memberStates = MemberStates.isAlive;
        memberCheck = MemberCheck.isLoitering;
        member = gameObject.GetComponent<NavMeshAgent>();
        member.autoBraking = false;
        GotoNextPoint();
        //プレイヤーを追うためのやつ
        def = transform.localRotation.eulerAngles;
        windpower = GamePlayManager.instance.CurrentStage.WindPower;
        new Vector3(0, 0, 0);
        //
        script = player.GetComponent<MemberList>();
        member.speed += navSpeed;

        //波紋のやつ
        rippleUI = GetComponentInChildren<RippleUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(memberHp);
        slider.value = memberHp;
        //Memberの処理分岐
        if (GetMemberCheck == MemberCheck.isLoitering)//徘徊しているときの処理書くところ
        {
            if (memberHp <= 0)
            {
                memberCheck = MemberCheck.isDead;
                memberStates = MemberStates.isDaed;
            }

            //スプライトの回転をなくす(Hpバーの回転もなくす）
            Vector3 parent = this.transform.parent.transform.localRotation.eulerAngles;
            this.transform.localRotation = Quaternion.Euler(def - parent);
            WeratherCheck();
            SquallCheck();
            if (!member.pathPending && member.remainingDistance < 0.5f)
            {
                GotoNextPoint();//ぐるぐる回るやつ(徘徊)
            }
        }
        else if(GetMemberCheck == MemberCheck.isCapture)//プレイヤーに捕まったらの処理書くところ
        {
            if (memberHp <= 0)
                memberCheck = MemberCheck.isDead;
            //WindMemberMove();
            WeratherCheck();
            PlayerFollows();
            MemberHubCheck();

        }
        else if (GetMemberCheck == MemberCheck.isHub)//拠点いるときの処理書くところ
        {
            //今のところ何もしない
        }
        else//死んだときの処理を書くところ
        {

        }     
    }

    public void PlayerFollows()//仲間がドラクエの隊列みたいにPlayerを追いかける
    {
        for (int i = 1; i < script.memberList.Count; i++)//捕まえたメンバーの数だけ回す
        {
            if (script.memberList[i] == this.gameObject)
            {
                member.destination = script.memberList[i - 1].transform.position;
                member.stoppingDistance = navStop;
            }
        }
        //スプライトの回転をなくす
        Vector3 parent = this.transform.parent.transform.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(def - parent);
    }

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
        if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            memberHp -= Time.deltaTime * 5;
            memberHp = System.Math.Max(memberHp, MIN);//最小値を超えたら戻す
        }
        else 
        {
            memberHp += Time.deltaTime * 2;
            memberHp = System.Math.Min(memberHp, memberHpMax);//最大値を超えたら戻す
        }           
    }

    public void SquallCheck()
    {
        if (GetMemberCheck == MemberCheck.isLoitering)
        {
            if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
            {
                member.isStopped = true;
            }
            else
            {
                member.isStopped = false;
            }
        }
    }

    public void MemberHubCheck()
    {
        if(GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Map)//今のところは拠点についたら
        {
            if (GetMemberCheck == MemberCheck.isCapture)
            {
                memberCheck = MemberCheck.isHub;
                memberStates = MemberStates.isHub;
                script.memberList.Remove(this.gameObject);//
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
            memberLingt.transform.localScale = new Vector3(lightScale.x,lightScale.y,lightScale.z);//LifhtのScale変更
            memberHp = 100;//HPを回復
        }      
    }

    //void WindMemberMove()
    //{
    //    if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Up
    //       && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
    //    {
    //        wind = new Vector3(0, 0, windpower);
    //    }
    //    else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Down
    //       && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
    //    {
    //        wind = new Vector3(0, 0, -windpower);
    //    }
    //    else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Left
    //       && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
    //    {
    //        wind = new Vector3(-windpower, 0, 0);
    //    }
    //    else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Right
    //       && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
    //    {
    //        wind = new Vector3(windpower, 0, 0);
    //    }
    //    transform.position += wind;
    //}

    public void Ripple()
    {
        rippleUI.Ripple();
    }
}
