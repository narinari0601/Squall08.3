using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public static GamePlayManager instance = null;

    public enum WeatherStates
    {
        Sun,
        Sign,
        Squall,
    }

    public enum SquallDirections
    {
        Up,
        Down,
        Left,
        Right,
    }

    //天気関連
    private WeatherStates weather;

    [SerializeField,Header("天気が1周する時間")]
    private float weatherRotateTime = 0;

    [SerializeField, Header("通常時の比率")]
    private float sunRatio = 0;

    [SerializeField, Header("予兆の比率")]
    private float signRatio = 0;

    [SerializeField, Header("スコールの比率")]
    private float squallRatio = 0;

    private float toatalWeatherRatio;  //天候比の合計

    private float currentWeatherTimer;


    private SquallDirections squallDirection;


    public WeatherStates Weather { get => weather; set => weather = value; }
    public SquallDirections SquallDirection { get => squallDirection; set => squallDirection = value; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);  //シーンを切り替えても消えない
        }
        else
        {
            Destroy(this.gameObject);
        }

        Initialize();
    }

    private void Initialize()
    {
        weather = WeatherStates.Sun;

        toatalWeatherRatio = sunRatio + signRatio + squallRatio;

        currentWeatherTimer = 0;
        
        squallDirection = SquallDirections.Up;

        //Debug.Log("1周 : " + weatherRotateTime + "秒");
        //Debug.Log("晴れ : " + sunRatio / toatalWeatherRatio * weatherRotateTime + "秒");
        //Debug.Log("予兆 : " + signRatio / toatalWeatherRatio * weatherRotateTime + "秒");
        //Debug.Log("スコール : " + squallRatio / toatalWeatherRatio * weatherRotateTime + "秒");
    }

    private void FixedUpdate()
    {
        ChangeWeather();
    }

    private void ChangeWeather()
    {
        currentWeatherTimer += Time.deltaTime;


        if (currentWeatherTimer < sunRatio / toatalWeatherRatio * weatherRotateTime)
        {
            weather = WeatherStates.Sun;
            //Debug.Log("晴れ");
        }

        else if (currentWeatherTimer >= (toatalWeatherRatio - squallRatio) / toatalWeatherRatio * weatherRotateTime)
        {
            weather = WeatherStates.Squall;
            //Debug.Log("スコール");
        }

        else
        {
            weather = WeatherStates.Sign;
            //Debug.Log("予兆");
        }

        if (currentWeatherTimer >= weatherRotateTime)
        {
            currentWeatherTimer = 0;
        }
    }
}
