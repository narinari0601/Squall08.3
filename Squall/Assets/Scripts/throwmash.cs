using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class throwmash : MonoBehaviour
{
    Vector3 wind;
    float windpower;
    Vector3 move;
    float deletetime;
    float time;
    // Start is called before the first frame update
    void Start()
    {
        windpower = GamePlayManager.instance.CurrentStage.WindPower;
        wind = Vector3.zero;
        SetMove(GamePlayManager.instance.Player.GetComponent<Playercontrol>().GetDirec());
        time = 1;
        deletetime = 300;
    }
    void Initialize()
    {
        windpower = GamePlayManager.instance.CurrentStage.WindPower;
        wind = Vector3.zero;
        time = 1;
        deletetime = 300;
    }
    // Update is called once per frame
    void Update()
    {
        Wind();
        Move();
        DeleteTimer();
    }
    public void SetMove(Vector3 velocity)
    {
        move = velocity;
    }
    void Move()
    {
        transform.position += move;

    }
    void Wind()
    {
        time+= 0.1f;
        
        if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Up
          && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(0, 0, windpower * time);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Down
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(0, 0, -windpower * time);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Left
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(-windpower * time, 0, 0);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Right
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(windpower * time, 0, 0);
        }
        transform.position += wind;
    }
    void DeleteTimer()
    {
        deletetime--;
        if (deletetime <= 0)
        {
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
    

        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
