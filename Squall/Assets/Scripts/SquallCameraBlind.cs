using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquallCameraBlind : MonoBehaviour
{
    public GamePlayManager gamePlayManager;
    GamePlayManager.WeatherStates currentstates;//現在の天気
    GamePlayManager.WeatherStates paststate;//１フレーム前の天気
    //Vector3 position;//視界阻害サイズ変更用
    public int direction;//ブロックごとの方向指定数字 0=した,1=うえ,2=みぎ,3=ひだり,
    int count;

    // Start is called before the first frame update
    void Start()
    {
        count = 21;
        currentstates = gamePlayManager.Weather;
    }

    // Update is called once per frame
    void Update()
    {
        paststate = currentstates;
        currentstates = gamePlayManager.Weather;

        if(currentstates == GamePlayManager.WeatherStates.Squall)
        {
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
                    transform.localScale -= new Vector3(0.02f, 0.02f, 0.02f);
                    count++;
                }
            }
        }
        else
        {
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
                    transform.localScale += new Vector3(0.02f,0.02f,0.02f);
                    count++;
                }
            }
        }
    }
}
