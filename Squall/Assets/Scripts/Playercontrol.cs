﻿using System.Collections;
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
    enum Direc
    {
        UP,DOWN,RIGHT,LEFT
    }
    Direc _direc;
    void Start()
    {
        _direc = Direc.UP;
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
        throwmash = (GameObject)Resources.Load("throwmash");
    }
    public void Initialize()
    {
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
        _direc = Direc.UP;
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
            _direc = Direc.UP;
            transform.position += new Vector3(0, 0, 0.3f);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0, 0, -0.3f);
            _direc = Direc.DOWN;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-0.3f, 0, 0);
            _direc = Direc.LEFT;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += new Vector3(0.3f, 0, 0);
            _direc = Direc.RIGHT;
        }
        if (Input.GetKey(KeyCode.C))
        {
            if (mashcount > 0)
            {
                mashcount--;
              
                Instantiate((GameObject)throwmash, transform.position+GetDirec()*3,
                    Quaternion.LookRotation(new Vector3(0, 90, 0), new Vector3(0, 0, 0)));
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
           return new Vector3(0, 0, -0.4f);
        }
        if (_direc == Direc.UP)
        {
            return new Vector3(0, 0, 0.4f);
        }
        if (_direc == Direc.RIGHT)
        {
            return new Vector3(0.4f, 0, 0);
        }
        if (_direc == Direc.LEFT)
        {
            return new Vector3(-0.4f, 0, 0);
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
        else if(collision.gameObject.tag == "TMash")
        {
            mashcount++;
        }
    }
}
