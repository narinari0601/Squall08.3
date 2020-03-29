using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playercontrol : MonoBehaviour
{
    // Startis called before the first frame updat
    public AudioClip _se;
    private AudioSource _audio;
    private Vector3 wind;
    void Start()
    {
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();
    }
    void Initialize()
    {
        wind = new Vector3(0, 0, 0);
        _audio = gameObject.GetComponent<AudioSource>();

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
    }
    void WindMove()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            _audio.PlayOneShot(_se);
        }
    }
    void Shout()
    {
        if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Up
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(0, 0, 0.1f);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Down
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(0, 0, -0.1f);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Left
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(-0.1f, 0, 0);
        }
        else if (GamePlayManager.instance.SquallDirection == GamePlayManager.SquallDirections.Right
           && GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            wind = new Vector3(0.1f, 0, 0);
        }
        transform.position += wind;
    }
}
