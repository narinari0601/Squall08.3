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
    int mashcount;
    GameObject throwmash;
    void Start()
    {
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
        throwmash = (GameObject)Resources.Load("throwmash");
    }
    public void Initialize()
    {
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();

        windpower = GamePlayManager.instance.CurrentStage.WindPower;

        throwmash = (GameObject)Resources.Load("throwmash");
    }
    // Update is called once per frame
    void Update()
    {
        WindMove();
        Move();
        Shout();
       
    }
    void Move()
    {
        wind = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.UpArrow))
        {

            transform.position += new Vector3(0, 0, 0.3f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, -0.3f);

        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-0.3f, 0, 0);

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.3f, 0, 0);

        }
        if (Input.GetKey(KeyCode.C))
        {
            if (mashcount > 0)
            {
                mashcount--;
                Instantiate(throwmash, new Vector3(-1.0f, 0.0f, 0.0f),
                    Quaternion.LookRotation(new Vector3(0, 90, 0), new Vector3(0, 0, 0)));
            }
            else
            {

            }
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
        transform.position += wind;
    }
    void Shout()
    {
       
        if (Input.GetKeyDown(KeyCode.B))
        {
            _audio.PlayOneShot(_se);
        }
    }
    void OnCollisionEnter(Collision collision)
    {


        Debug.Log("Hit"); // ログを表示する

        if (collision.gameObject.tag == "Mash")
        {
            mashcount++;
        }
    }
}
