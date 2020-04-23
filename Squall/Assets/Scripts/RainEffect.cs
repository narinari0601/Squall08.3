using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEffect : MonoBehaviour
{
    public AudioSource rainBGM;//雨音
    const float maxvol = 0.5f;//スコール時の最大ボリューム
    const float minvol = 0.1f;//スコールじゃない時の最小ボリューム
    const float addvol = 0.02f;//天気が変わった時に音量が変化する速度
    const float addangle = 5f;//角度の変わる速度

    Quaternion target;
    Quaternion now_rot;
    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //rainBGM = GetComponent<AudioSource>();
        rainBGM.volume = minvol;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        #region 音量関連
        if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)//スコール中に音量を上げる
        {
            if (rainBGM.volume < maxvol)
            {
                rainBGM.volume += addvol;
            }
            else if (rainBGM.volume > maxvol)//音量が最大値を超えないように設定
            {
                rainBGM.volume = maxvol;
            }
        }
        else if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Sign)//予兆のときに音量を上げる
        {
            if (rainBGM.volume < minvol)
            {
                rainBGM.volume += addvol;
            }
            else if (rainBGM.volume > minvol)//音量が最小値を上回らないように設定
            {
                rainBGM.volume = minvol;
            }
        }
        else if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Sun)//スコールじゃないときに音量を下げる
        {
            if (rainBGM.volume > 0)
            {
                rainBGM.volume -= addvol;
            }
            else if (rainBGM.volume < 0)//音量が0を下回らないように設定
            {
                rainBGM.volume = minvol;
            }
        }
        #endregion

        #region 風向き
        if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Up)//風向きが上の時
        {
            target = Quaternion.Euler(90, 0, 0);
            now_rot = transform.rotation;
            if (Quaternion.Angle(now_rot,target) <= 1)
            {
                transform.rotation = target;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, addangle));
            }
        }
        if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Down)//風向きが下の時
        {
            target = Quaternion.Euler(90, 0, 0);
            now_rot = transform.rotation;
            if (Quaternion.Angle(now_rot, target) <= 1)
            {
                transform.rotation = target;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, addangle));
            }
        }
        if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Right)//風向きが右の時
        {
            target = Quaternion.Euler(90, 0, 60);
            now_rot = transform.rotation;
            if (Quaternion.Angle(now_rot, target) <= 1)
            {
                transform.rotation = target;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, addangle));
            }
        }
        if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Left)//風向きが左の時
        {
            target = Quaternion.Euler(90, 0, -60);
            now_rot = transform.rotation;
            if (Quaternion.Angle(now_rot, target) <= 1)
            {
                transform.rotation = target;
            }
            else
            {
                transform.Rotate(new Vector3(0, 0, -addangle));
            }
        }
        #endregion

        if(GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
        else if(GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Sign)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.3f);
        }
        else if(GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Sun)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }
}
