using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    // Startis called before the first frame updat
    AudioClip throwse;
    AudioClip getse;
    private AudioSource _audio;
    private Vector3 wind;
    private float windpower;
    private int shouttime;
    private Vector3 damagevelocity;
    private Vector3 savevelocity;
    private Vector3 velocity;
    private GameObject bursteffect;
    public int HP;
    private int mutekitime;
    int mashcount;
    GameObject throwmash;
    Animator anim;

    //ナリが追加
    private bool isJump;  //ジャンプ中ならtrue
    private MemberList memberList;
    public bool IsJump { get => isJump; set => isJump = value; }
    public MemberList MemberList { get => memberList; set => memberList = value; }
    public int Mashcount { get => mashcount;}

    //

    enum Direc
    {
        UP,DOWN,RIGHT,LEFT
    }
    Direc _direc;
    void Start()
    {
        mutekitime = 0;
        shouttime = 0;
        getse = (AudioClip)Resources.Load("Sounds/GetSE");
        throwse = (AudioClip)Resources.Load("Sounds/ThrowSE");
        _direc = Direc.UP;
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
        throwmash = (GameObject)Resources.Load("throwmash");
        bursteffect = (GameObject)Resources.Load("Effects/Burst");
        isJump = false;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        memberList = GetComponent<MemberList>();
    }
    public void Initialize()
    {
        mutekitime = 0;
        shouttime = 0;
        getse = (AudioClip)Resources.Load("Sounds/GetSE");
        throwse = (AudioClip)Resources.Load("Sounds/HitSE");
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
        _direc = Direc.UP;
        windpower = GamePlayManager.instance.CurrentStage.WindPower;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        throwmash = (GameObject)Resources.Load("throwmash");
        isJump = false;
        memberList = GetComponent<MemberList>();
        onPlayerDamageFlag = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Play)
        {
            TimerManager();
            WindMove();
            Damagemove();

            Move();
        }

        else if (GamePlayManager.instance.GameState == GamePlayManager.GamePlayStates.Pause)
        {
            TimerManager();
            WindMove();
            Damagemove();
        }

        //Shout();
       
    }
    void TimerManager()
    {
        if (mutekitime > 0)
        {
            mutekitime--;
        }
    }
    void Move()
    {
        if (mutekitime <= 30)
        {


            if (isJump)
                return;

            wind = new Vector3(0, 0, 0);

            if (Input.GetKey(KeyCode.UpArrow))
            {
               
                velocity += new Vector3(0, 0, 0.1f);
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    _direc = Direc.UP;
                    anim.SetInteger("Direction", 0);
                }
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                velocity += new Vector3(0, 0, -0.1f);
      
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    _direc = Direc.DOWN;
                    anim.SetInteger("Direction", 1);
                }
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                velocity += new Vector3(-0.1f, 0, 0);
                
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    _direc = Direc.LEFT;
                    anim.SetInteger("Direction", 2);
                }
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                velocity += new Vector3(0.1f, 0, 0);
               
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    _direc = Direc.RIGHT;
                    anim.SetInteger("Direction", 3);
                }
            }

            if (Mathf.Abs(velocity.x) > Mathf.Abs(velocity.z))
            {
                if (velocity.x < 0)
                {
                    _direc = Direc.LEFT;
                    anim.SetInteger("Direction", 2);
                }
                else if (velocity.x > 0)
                {
                    _direc = Direc.RIGHT;
                    anim.SetInteger("Direction", 3);
                }
            }
            if (Mathf.Abs(velocity.x) < Mathf.Abs(velocity.z))
            {
                if (velocity.z < 0)
                {
                    _direc = Direc.DOWN;
                    anim.SetInteger("Direction", 1);
                }
                else if (velocity.z > 0)
                {
                    _direc = Direc.UP;
                    anim.SetInteger("Direction", 0);
                }
            }

            if (velocity.x * velocity.x + velocity.z * velocity.z > 0.01)
            {
                if (velocity.x < 0)
                {
                    velocity.x = 0.1f / -1.4f;
                }
                else if (velocity.x > 0)
                {
                    velocity.x = 0.1f / 1.4f;
                }
                if (velocity.z < 0)
                {
                    velocity.z = 0.1f / -1.4f;
                }
                else if (velocity.z > 0)
                {
                    velocity.z = 0.1f / 1.4f;
                }

            }

            transform.position += velocity;
            velocity = Vector3.zero;
            if (Input.GetKeyDown(KeyCode.C))
            {
                if (mashcount > 0)
                {
                    mashcount--;
                    _audio.PlayOneShot(throwse);
                    Instantiate((GameObject)throwmash, transform.position + GetDirec() * 9,
                        Quaternion.LookRotation(new Vector3(0, -90, 0), new Vector3(0, 0, 0)));
                }
                else
                {

                }
            }
        }
    }
    public Vector3 GetDirec()
    {
        if (_direc == Direc.DOWN)
        {
           return new Vector3(0, 0f, -0.2f);
        }
        if (_direc == Direc.UP)
        {
            return new Vector3(0, 0, 0.2f);
        }
        if (_direc == Direc.RIGHT)
        {
            return new Vector3(0.2f, 0, 0);
        }
        if (_direc == Direc.LEFT)
        {
            return new Vector3(-0.2f, 0, 0);
        }
        else
        {
            return Vector3.zero;
        }
    }
    void WindMove()
    {
        if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Up
            && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(0, 0, windpower);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Down
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(0, 0, -windpower);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Left
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(-windpower, 0, 0);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Right
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(windpower, 0, 0);
        }
        if (mutekitime == 0)
        {
            transform.position += wind;
        }
     //   transform.position *= Time.deltaTime;
    }
    //void Shout()
    //{

    //    if (Input.GetKeyDown(KeyCode.B) && shouttime == 0)
    //    {
    //        _audio.PlayOneShot(_se);
    //        GamePlayManager.instance.CurrentStage.Ripple();
    //        shouttime = 180;
    //    }
    //    else if (shouttime != 0) 
    //    {
    //        Debug.Log(shouttime);
    //        shouttime--;
    //    }
    //}
    public void Damage(Vector3 Nock)
    {
        if (mutekitime == 0)
        {
            mutekitime = 210;
            HP--;
          
            damagevelocity = Nock/10;
            damagevelocity.y = 0;

            savevelocity = Nock / 10;
            savevelocity.y = 0;
            Instantiate((GameObject)bursteffect, transform);
        }
    }
    void Damagemove()
    {
        if (mutekitime > 200)
        {
            transform.position = transform.position + damagevelocity;
            damagevelocity -= savevelocity / 5;
            //Debug.Log(damagevelocity);
            
        }
       // transform.position *= Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
         // ログを表示する

        if (collision.gameObject.tag == "Mash")
        {
            _audio.PlayOneShot(getse);
           
            mashcount++;
        }
        if(collision.gameObject.tag == "TMash")
        {
            _audio.PlayOneShot(getse);
            Debug.Log("Hit");
            mashcount++;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            //if (mutekitime == 0)
            //{
            //    mutekitime = 300;
            //    HP--;
            //    _audio.PlayOneShot(_se);
            //}
            //菅原追加
            if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
            {
                DamageSprite();//点滅処理
            }
                
                
        }
    }

    //以下菅原追加
    private bool onPlayerDamageFlag;
    public SpriteRenderer spriteRenderer;
    //Playerの画像
    //private GameObject playerSprite;
    public void DamageSprite()
    {
        StartCoroutine("WaitSpriteAlpha");
    }
    IEnumerator WaitSpriteAlpha()
    {
        if (onPlayerDamageFlag)
        {
            yield break;
        }
        onPlayerDamageFlag = true;

        for (int i = 0; i < 20; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(0.05f);

            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.05f);
        }
        onPlayerDamageFlag = false;
    }
}
