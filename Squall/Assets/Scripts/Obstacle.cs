using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    Material sunmaterial;
    Material rainmaterial;
    Material damagematerial;
    public Sprite damage;
    public Sprite sun;
    public Sprite rain;
    int damagetime;
    AudioSource _audio;
    AudioClip hitse;
    //
    RippleUI rippleUI;
    //

    // Start is called before the first frame update
    void Start()
    {
        sunmaterial = (Material)Resources.Load("Materials/blue");
        rainmaterial = (Material)Resources.Load("Materials/red");
        damagematerial = (Material)Resources.Load("Materials/damage");
        _audio = GetComponent<AudioSource>();
        hitse = (AudioClip)Resources.Load("Sounds/HitSE");
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
                if (transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite != rain)
                {
                    transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = rain;
                   
                }
             //   gameObject.GetComponent<Renderer>().material = rainmaterial;
                
                
            }
            else
            {
                if (transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite != sun)
                {
                    transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = sun;
                }
               // gameObject.GetComponent<Renderer>().material = sunmaterial;
             
            }
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "TMash" && damagetime == 0) 
        {
            damagetime = 600;
            Debug.Log(damagetime);
            if (transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite != damage )
            {
                _audio.PlayOneShot(hitse);
                Debug.Log(hitse.name);
                transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = damage;
            }
            // GetComponent<Renderer>().material = damagematerial;
            // sprite = damage;
        }
        if (collision.gameObject.tag == "Player" &&
            GamePlayManager.instance.Weather == GamePlayManager.WeatherStates.Squall &&
            damagetime == 0)  
        {
            collision.gameObject.GetComponent<Playercontrol>().Damage(
                (collision.transform.position - transform.position).normalized);
        }
    }
    public void Initialize()
    {
        sunmaterial = (Material)Resources.Load("Materials/blue");
        damagematerial = (Material)Resources.Load("Materials/damage");
        rainmaterial = (Material)Resources.Load("Materials/red");
        damagetime = 0;
        hitse = (AudioClip)Resources.Load("Sounds/hitSE");
        _audio = GetComponent<AudioSource>();
        //
        rippleUI = GetComponentInChildren<RippleUI>();
        //
    }

    //
    public void Ripple()
    {
        rippleUI.Ripple();
    }
    //
}
