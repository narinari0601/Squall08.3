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


    //ナリが追加
    private bool isJump;  //ジャンプ中ならtrue
    private MemberList memberList;
    public bool IsJump { get => isJump; set => isJump = value; }
    public MemberList MemberList { get => memberList; set => memberList = value; }

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

        throwmash = (GameObject)Resources.Load("throwmash");
        isJump = false;
        memberList = GetComponent<MemberList>();
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
        if (isJump)
            return;

        wind = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _direc = Direc.UP;
            velocity += new Vector3(0, 0, 0.1f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            velocity += new Vector3(0, 0, -0.1f);
            _direc = Direc.DOWN;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            velocity += new Vector3(-0.1f, 0, 0);
            _direc = Direc.LEFT;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            velocity += new Vector3(0.1f, 0, 0);
            _direc = Direc.RIGHT;
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
        if (mutekitime == 0)
        {
            transform.position += velocity;
        }
        velocity = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (mashcount > 0)
            {
                mashcount--;
                _audio.PlayOneShot(throwse);
                Instantiate((GameObject)throwmash, transform.position + GetDirec() * 9 ,
                    Quaternion.LookRotation(new Vector3(0, -90, 0), new Vector3(0, 0, 0)));
            }
            else
            {

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
            mutekitime = 180;
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
        if (mutekitime > 170)
        {
            transform.position = transform.position + damagevelocity;
            damagevelocity -= savevelocity / 5;
            Debug.Log(damagevelocity);
            
        }
       // transform.position *= Time.deltaTime;
    }
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Hit"); // ログを表示する

        if (collision.gameObject.tag == "Mash")
        {
            _audio.PlayOneShot(getse);
            Debug.Log(getse.name);
            mashcount++;
        }
        else if(collision.gameObject.tag == "TMash")
        {
            _audio.PlayOneShot(getse);
            mashcount++;
        }
        //if (collision.gameObject.tag == "Enemy")
        //{
        //    if (mutekitime == 0)
        //    {
        //        mutekitime = 300;
        //        HP--;
        //        _audio.PlayOneShot(_se);
        //    }
        //}
    }
}
