using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MemberControl : MonoBehaviour
{
    public NavMeshAgent member;
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
    [SerializeField, Header("全体マップ用HPスライダー")]
    private Slider mapHpSlider = null;
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
    //アニメーション関係
    public float angle;
    public Rigidbody rigid;
    private SpriteRenderer spriteRenderer;
    private bool onDamageFlag;
    //音
    AudioSource audioSource;
    public AudioClip enemyHitSE;
    public AudioClip bikkuriSE;

    public enum MemberDirection//アニメーション判定
    {
        Up,
        Down,
        Left,
        Right,
        UpStop,
        DownStop,
        LeftStop,
        RightStop,
        Deth,
    }
    private MemberDirection memberDirection;
    public MemberDirection GetMemberDirection { get => memberDirection; set => memberDirection = value; }

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
        mapHpSlider.maxValue = memberHpMax;
        playerScript = GetComponent<Playercontrol>();
        //波紋のやつ
        rippleUI = GetComponentInChildren<RippleUI>();
        //仲間の画像
        memberSprite = this.gameObject.transform.Find("MemberSprite").gameObject;
        memberMapSprite = this.gameObject.transform.Find("MemberMapSprite").gameObject;
        memberHpUI = this.gameObject.transform.Find("MemberHpUI").gameObject;
        boxCollider = GetComponent<BoxCollider>();
        oneTrigger = false;
        onDamageFlag = false;
        spriteRenderer = memberSprite.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = memberHp;
        mapHpSlider.value = memberHp;
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
            MemverDirectionSetting();
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
            MemverDirectionSetting();
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
            memberDirection = MemberDirection.Deth;
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

        angle = GetAngle(new Vector2(this.transform.position.x, this.transform.position.z),
               new Vector2(points[destPoint].transform.position.x, points[destPoint].transform.position.z));

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
                member.speed = 6;//通常時捕まえてる時のスピード
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
            audioSource.PlayOneShot(bikkuriSE);
            member.isStopped = false;//徘徊していたnavmechを止める
            memberCheck = MemberCheck.isCapture;           
            MemberToPlayer();
            script.memberList.Add(this.gameObject);
            memberLingt.transform.localScale = new Vector3(lightScale.x,lightScale.y,lightScale.z);//LifhtのScale変更

            Instantiate((GameObject)Resources.Load("reaction"), transform.position + new Vector3(0, 0, 2f),
                    Quaternion.LookRotation(new Vector3(0, -90, 0), new Vector3(0, 0, 0)),this.transform);

            if (recovery == 0)
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
            audioSource.PlayOneShot(enemyHitSE);
            memberHp -= damageToMember;//ダメージ量
            invincibleTime = 4;//無敵時間(単位-秒)
            DamageSprite();//点滅処理
        }
    }
    public void DamageSprite()
    {       
        StartCoroutine("WaitSpriteAlpha");      
    }
    IEnumerator WaitSpriteAlpha()
    {
        if(onDamageFlag)
        {
            yield break;
        }
        onDamageFlag = true;

        for(int i = 0;i<20;i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.05f);

            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.05f);
        }
        onDamageFlag = false;     
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

    public float GetAngle(Vector2 start,Vector2 target)//上基準
    {
        Vector2 dt = target - start;
        float rad = Mathf.Atan2(dt.x, dt.y);
        float degree = rad * Mathf.Rad2Deg;
        if(degree<0)
        {
            degree += 360;
        }
        return degree;
    }

    public void MemverDirectionSetting()//仲間の向きと状態を決める
    {
        //angleは 0～360
        if (GetMemberCheck == MemberCheck.isCapture)//捕まえた時の角度
        {
            for (int i = 1; i < script.memberList.Count; i++)//捕まえたメンバーの数だけ回す、誰をターゲットにするか決める
            {
                if (script.memberList[i] == this.gameObject)
                {
                    angle = GetAngle(new Vector2(this.transform.position.x, this.transform.position.z),
                 　　　　new Vector2(script.memberList[i - 1].transform.position.x, script.memberList[i - 1].transform.position.z));
                }
            }
            
            if(rigid.IsSleeping())//仲間が止まっているときのアニメーション
            {
                if (angle < 45 || angle >= 315)
                {
                    memberDirection = MemberDirection.UpStop;
                }
                else if (angle >= 45 && angle < 135)
                {
                    memberDirection = MemberDirection.RightStop;
                }
                else if (angle >= 135 && angle < 225)
                {
                    memberDirection = MemberDirection.DownStop;
                }
                else if (angle >= 225 && angle < 315)
                {
                    memberDirection = MemberDirection.LeftStop;
                }
            }
            else//仲間が動いているときのアニメーション
            {
                if (angle < 45 || angle >= 315)
                {
                    memberDirection = MemberDirection.Up;
                }
                else if (angle >= 45 && angle < 135)
                {
                    memberDirection = MemberDirection.Right;
                }
                else if (angle >= 135 && angle < 225)
                {
                    memberDirection = MemberDirection.Down;
                }
                else if (angle >= 225 && angle < 315)
                {
                    memberDirection = MemberDirection.Left;
                }
            }
        }

        if(GetMemberCheck == MemberCheck.isLoitering)//徘徊しているときの角度設定
        {
            if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)//スコールの時のアニメーション
            {
                if (angle < 45 || angle >= 315)
                {
                    memberDirection = MemberDirection.UpStop;
                }
                else if (angle >= 45 && angle < 135)
                {
                    memberDirection = MemberDirection.RightStop;
                }
                else if (angle >= 135 && angle < 225)
                {
                    memberDirection = MemberDirection.DownStop;
                }
                else if (angle >= 225 && angle < 315)
                {
                    memberDirection = MemberDirection.LeftStop;
                }
            }
            else//通常時のアニメーション
            {
                if (angle < 45 || angle >= 315)
                {
                    memberDirection = MemberDirection.Up;
                }
                else if (angle >= 45 && angle < 135)
                {
                    memberDirection = MemberDirection.Right;
                }
                else if (angle >= 135 && angle < 225)
                {
                    memberDirection = MemberDirection.Down;
                }
                else if (angle >= 225 && angle < 315)
                {
                    memberDirection = MemberDirection.Left;
                }
            }
        }
    }
}
