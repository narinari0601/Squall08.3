using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Material sunmaterial;
    Material rainmaterial;
    Material damagematerial;
    int damagetime;
    // Start is called before the first frame update
    void Start()
    {
        sunmaterial = (Material)Resources.Load("Materials/blue");
        rainmaterial = (Material)Resources.Load("Materials/red");
        damagematerial = (Material)Resources.Load("Materials/damage");

        damagetime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (damagetime > 0)
        {
            damagetime--;
        }
        else
        {
            if (GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall)
            {
                gameObject.GetComponent<Renderer>().material = rainmaterial;
            }
            else
            {
                gameObject.GetComponent<Renderer>().material = sunmaterial;
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "TMash")
        {
            damagetime = 600;
            GetComponent<Renderer>().material = damagematerial;
        }
        if (collision.gameObject.tag == "Player" &&
            GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall &&
            damagetime == 0)  
        {
            collision.gameObject.GetComponent<Playercontrol>().Damage();
        }
    }
    public void Initialize()
    {
        sunmaterial = (Material)Resources.Load("Materials/blue");
        damagematerial = (Material)Resources.Load("Materials/damage");
        rainmaterial = (Material)Resources.Load("Materials/red");
        damagetime = 0;
    }
}
