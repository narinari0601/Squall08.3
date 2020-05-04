using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MemberControl : MonoBehaviour
{
    public NavMeshAgent member;
    //public float navSpeed;//navの動くスピード
    public float navStop;//どれだけ離れて止まるか
    public Transform[] points;
    private int destPoint = 0;
    [SerializeField, Header("仲間のHP")]
    public float memberHp;//仲間のhp

    [SerializeField, Header("仲間が最初から受けるダメージ")]
    public float memberStartDamageHp = 0;//仲間が最初から受けるダメージ

    [SerializeField, Header("敵に当たった時のダメージ")]
    public float damageToMember = 0;//仲間が敵二当たった時のダメージ

    [SerializeField, Header("仲間を捕まえた時の回復量 0なら最大値まで回復")]
    public float recovery = 0;//仲間が最初から受けるダメージ

    const float MIN = 0;
    //const float MAX =200;
    private float memberHpMax;//最大hp
    //プレイヤーを追うための
    private Vector3 def;
    private GameObject player;
    MemberList script;
    Playercontrol playerScript;

   
    //仲間の光
    private GameObject memberLingt;
    [SerializeField, Header("仲間の光のスケール")]
    public Vector3 lightScale;
    public Slider slider;
    private float invincibleTime = 0;
    //仲間の速さ
    private Vector3 wind;
    private float windpower;
    //波紋
    RippleUI rippleUI;
    //仲間の画像
    private GameObject memberSprite;
    //仲間の画像(マップ版)
    private GameObject memberMapSprite;
    //仲間のコライダー
    private BoxCollider boxCollider;
    private GameObject memberHpUI;
    //非表示を一回しか呼ばないためのフラグ
    private bool oneTrigger = false;

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

    }

    public void Initialize()
    {
        memberHpMax = memberHp;//最大値の固定
        memberHp -= memberStartDamageHp;
        //player = GameObject.Find("Player");
        player = GamePlayManager.instance.CurrentStage.PlayerObj;
        //memberLingt = this.gameObject.transform.Find("MemberLight");
        memberLingt = this.gameObject.transform.Find("MemberLight").gameObject;
        memberStates = MemberStates.isAlive;//生きてる
        memberCheck = MemberCheck.isLoitering;//徘徊
        member = gameObject.GetComponent<NavMeshAgent>();
        member.autoBraking = false;
        GotoNextPoint();
        //プレイヤーを追うためのやつ
        def = transform.localRotation.eulerAngles;
        windpower = GamePlayManager.instance.CurrentStage.WindPower;
        new Vector3(0, 0, 0);
        //メンバーリストを取得
        script = player.GetComponent<MemberList>();
        // member.speed += navSpeed;
        slider.maxValue = memberHpMax;
        playerScript = GetComponent<Playercontrol>();
        //波紋のやつ
        rippleUI = GetComponentInChildren<RippleUI>();
        //仲間の画像
        memberSprite = this.gameObject.transform.Find("MemberSprite").gameObject;
        memberMapSprite = this.gameObject.transform.Find("MemberMapSprite").gameObject;
        memberHpUI = this.gameObject.transform.Find("MemberHpUI").gameObject;
        boxCollider = GetComponent<BoxCollider>();
        oneTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {    
        //if(Input.GetKeyDown(KeyCode.A))
        //{
        //    Debug.Log(Vector2.Angle(new Vector2(player.transform.position.x, player.transform.position.z), new Vector2(this.gameObject.transform.position.x, this.gameObject.transform.position.z)));
        //}
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
            MemberDontRotaion();//HPバーが回転しないように
            WeratherCheck();//天気の確認
            SquallCheck();//スコールの時止まるやつ
            if (!member.pathPending && member.remainingDistance < 0.5f)
            {
                GotoNextPoint();//ぐるぐる回るやつ(徘徊)
            }
        }
        else if(GetMemberCheck == MemberCheck.isCapture)//プレイヤーに捕まったらの処理書くところ
        {
            if (memberHp <= 0)//死んだら
            {
                memberCheck = MemberCheck.isDead;
                memberStates = MemberStates.isDaed;
                script.memberList.Remove(this.gameObject);
            }
                      
            WeratherCheck();//天気の確認
            PlayerFollows();//隊列になるやつ
            //MemberHubCheck();//拠点についたら
            
            if(invincibleTime>0)//敵と当たった時の無敵時間
            {
                invincibleTime -= Time.deltaTime;
            }
          
        }
        else if (GetMemberCheck == MemberCheck.isHub)//拠点いるときの処理書くところ
        {
            MemberDontRotaion();//HPバーが回転しないように
            if(!oneTrigger)
            {
                memberSprite.SetActive(false);//仲間の画像を非表示
                memberMapSprite.SetActive(false);//仲間のマップ画像を非表示
                memberLingt.SetActive(false);//仲間のライトを非表示
                memberHpUI.SetActive(false);//仲間のHpUIを非表示
                boxCollider.enabled = false;//仲間同時の当たり判定を消す
                oneTrigger = true;
            }            
        }
        else//死んだときの処理を書くところ
        {
            MemberDontRotaion();//HPバーが回転しないように
        }     
    }

    public void MemberDontRotaion()//スプライトの回転をなくす
    {
        Vector3 parent = this.transform.parent.transform.localRotation.eulerAngles;
        this.transform.localRotation = Quaternion.Euler(def - parent);
    }

    public void PlayerFollows()//仲間がドラクエの隊列みたいにPlayerを追いかける
    {
        for (int i = 1; i < script.memberList.Count; i++)//捕まえたメンバーの数だけ回す
        {
            if (script.memberList[i] == this.gameObject)
            {
                member.destination = script.memberList[i -1].transform.position;
                member.stoppingDistance = navStop;
            }
        }
        MemberDontRotaion();//スプライトの回転をなくす
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
        //ぐるぐる回るやつ(徘徊)
        member.destination = points[destPoint].transform.position;
        destPoint = (destPoint + 1) % points.Length;

    }

    public void WeratherCheck()//天気を確認()
    {
        if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            if(GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Play 
                || GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Pause)//ポーズとプレイ中のみダメージを受けるを受ける
            {
                memberHp -= Time.deltaTime * 4;//毎秒減る量
            }
            
            memberHp = System.Math.Max(memberHp, MIN);//最小値を超えたら戻す
            member.speed = 8;//Playerを追いかけるスピードを変更
        }
        else 
        {
            //memberHp += Time.deltaTime * 2;//
            memberHp = System.Math.Min(memberHp, memberHpMax);//最大値を超えたら戻す
            if(GetMemberCheck == MemberCheck.isLoitering)
            {
                member.speed = 3;//通常時捕まえていないときのスピード
            }
            else if(GetMemberCheck == MemberCheck.isCapture)
            {
                member.speed = 7;//通常時捕まえてる時のスピード
            }   
        }           
    }

    public void SquallCheck()//スコールの時止まるやつ
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

    public void MemberHubCheck()//拠点についたか
    {
        //if(GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Map)
        {
            if (GetMemberCheck == MemberCheck.isCapture)
            {
                memberCheck = MemberCheck.isHub;
                memberStates = MemberStates.isHub;
                script.memberList.Remove(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && GetMemberCheck == MemberCheck.isLoitering)
        {
            member.isStopped = false;//徘徊していたnavmechを止める
            memberCheck = MemberCheck.isCapture;           
            MemberToPlayer();
            script.memberList.Add(this.gameObject);
            memberLingt.transform.localScale = new Vector3(lightScale.x,lightScale.y,lightScale.z);//LifhtのScale変更
            if(recovery == 0)
            {
                memberHp = memberHpMax;//HPを全回復
            }
            else
            {
                memberHp += recovery;
            }
            
            member.agentTypeID = 0;//navmeshを変更
        }     
        if(other.gameObject.tag =="Enemy" && GetMemberCheck == MemberCheck.isCapture && invincibleTime <=0 
            && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall )
        {
            memberHp -= damageToMember;//ダメージ量
            invincibleTime = 5;//無敵時間(単位-秒)
            Debug.Log("aaaaaaaa");
        }
    }
    public void Ripple()
    {
        if (GetMemberCheck == MemberCheck.isLoitering )
        {
            rippleUI.Ripple();//徘徊しているときに呼ばれるUI
        }
       
    }

    public float GetMemberHp()
    {
        return memberHp;
    }

    public float GetMaxMemberHp()
    {
        return memberHpMax;
    }

}
