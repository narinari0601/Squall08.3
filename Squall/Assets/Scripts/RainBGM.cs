using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainBGM : MonoBehaviour
{
    AudioSource rainBGM;//雨音

    // Start is called before the first frame update
    void Start()
    {
        rainBGM = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
        {
            rainBGM.volume = 1;
        }
        else if(GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Sun || GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Sign)
        {
            rainBGM.volume = 0.5f;
        }
    }
}
