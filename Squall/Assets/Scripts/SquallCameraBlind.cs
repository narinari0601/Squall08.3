﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquallCameraBlind : MonoBehaviour
{
    //GamePlayManager gamePlayManager;
    GamePlayManager.WeatherStates currentstates;//現在の天気
    GamePlayManager.WeatherStates paststate;//１フレーム前の天気
    GamePlayManager.GamePlayStates currentstate2;//現在はマップを見てるか
    GamePlayManager.GamePlayStates paststate2;//さっきはマップを見てたか
    //Vector3 position;//視界阻害サイズ変更用
    public int direction;//ブロックごとの方向指定数字 0=した,1=うえ,2=みぎ,3=ひだり,4=,スプライトマスク
    int count;
    Transform transforms;

    // Start is called before the first frame update
    void Start()
    {
        count = 21;
        currentstates = GamePlayManager.instance.Weather;
        transforms = GameObject.Find("View").transform;
        //gamePlayManager = GameObject.Find("GamePlayManager");
    }

    // Update is called once per frame
    void Update()
    {
        paststate = currentstates;
        currentstates = GamePlayManager.instance.Weather;

        if(currentstates == GamePlayManager.WeatherStates.Squall)
        {
            if(currentstate2 == GamePlayManager.GamePlayStates.Play && paststate2 == GamePlayManager.GamePlayStates.Map)
            {
                count = 21;
                transforms.localScale -= new Vector3(0.42f,0.42f,0.42f);
            }

            if (paststate == GamePlayManager.WeatherStates.Sign)
            {
                count = 0;
            }
            if (direction == 0)
            {
                if (count < 21)
                {
                    transform.Translate(Vector3.forward * 0.1f);
                    count++;
                }
            }
            if (direction == 1)
            {
                if (count < 21)
                {
                    transform.Translate(Vector3.back * 0.1f);
                    count++;
                }
            }
            if (direction == 2)
            {
                if  (count < 21)
                {
                    transform.Translate(Vector3.left*0.25f);
                    count++;
                }
            }
            if (direction == 3)
            {
                if (count < 21)
                {
                    transform.Translate(Vector3.right * 0.25f);
                    count++;
                }
            }
            if (direction == 4)
            {
                if (count < 21)
                {
                    transforms.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                    count++;
                }
            }
        }
        else
        {
            if (currentstate2 == GamePlayManager.GamePlayStates.Play && paststate2 == GamePlayManager.GamePlayStates.Map)
            {
                count = 21;
                transforms.localScale += new Vector3(0.42f, 0.42f, 0.42f);
            }
            if (paststate == GamePlayManager.WeatherStates.Squall)
            {
                count = 0;
            }
            if (direction == 0)
            {
                if (count < 21)
                {
                    transform.Translate(Vector3.back*0.1f);
                    count++;
                }
            }
            if (direction == 1)
            {
                if (count < 21)
                {
                    transform.Translate(Vector3.forward * 0.1f);
                    count++;
                }
            }
            if (direction == 2)
            {
                if (count < 21)
                {
                    transform.Translate(Vector3.right * 0.25f);
                    count++;
                }
            }
            if (direction == 3)
            {
                if (count < 21)
                {
                    transform.Translate(Vector3.left * 0.25f);
                    count++;
                }
            }
            if (direction == 4)
            {
                if (count < 21)
                {
                    transforms.localScale += new Vector3(0.02f,0.02f,0.02f);
                    count++;
                }
            }
        }
    }
}
