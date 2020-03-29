using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwmash : MonoBehaviour
{
    Vector3 wind;
    float windpower;
    // Start is called before the first frame update
    void Start()
    {
        windpower = GamePlayManager.instance.CurrentStage.WindPower;
    }
    void Initialize()
    {
        windpower = GamePlayManager.instance.CurrentStage.WindPower;
    }
    // Update is called once per frame
    void Update()
    {
        Wind();
    }
    void Wind()
    {
        wind = new Vector3(0, 0, 0);
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
  
}
