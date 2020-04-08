using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    // Startis called before the first frame updat
    public AudioClip _se;
    private AudioSource _audio;
    private Vector3 wind;
    private float windpower;
    private int shouttime;
    private Vector3 damagevelocity;
    private Vector3 savevelocity;
    private Vector3 velocity;
    public int HP;
    private int mutekitime;
    int mashcount;
    GameObject throwmash;
    enum Direc
    {
        UP,DOWN,RIGHT,LEFT
    }
    Direc _direc;
    void Start()
    {
        mutekitime = 0;
        shouttime = 0;
        _direc = Direc.UP;
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
        throwmash = (GameObject)Resources.Load("throwmash");
    }
    public void Initialize()
    {
        mutekitime = 0;
        shouttime = 0;
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
        _direc = Direc.UP;
        windpower = GamePlayManager.instance.CurrentStage.WindPower;

        throwmash = (GameObject)Resources.Load("throwmash");
    }
    // Update is called once per frame
    void Update()
    {
        TimerManager();
        WindMove();
        Damagemove();
       
            Move();
        
        Shout();
       
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
        if (Input.GetKey(KeyCode.C))
        {
            if (mashcount > 0)
            {
                mashcount--;

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
    void Shout()
    {

        if (Input.GetKeyDown(KeyCode.B) && shouttime == 0)
        {
            _audio.PlayOneShot(_se);
            GamePlayManager.instance.CurrentStage.Ripple();
            shouttime = 180;
        }
        else if (shouttime != 0) 
        {
            Debug.Log(shouttime);
            shouttime--;
        }
    }
    public void Damage(Vector3 Nock)
    {
        if (mutekitime == 0)
        {
            mutekitime = 180;
            HP--;
            _audio.PlayOneShot(_se);
            damagevelocity = Nock/10;
            savevelocity = Nock / 10;
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
            mashcount++;
        }
        else if(collision.gameObject.tag == "TMash")
        {
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
