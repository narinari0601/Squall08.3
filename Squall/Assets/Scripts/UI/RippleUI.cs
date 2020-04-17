using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RippleUI : MonoBehaviour
{
    [SerializeField, Header("Imageオブジェクト")]
    private GameObject sprite = null;

    private RectTransform imageRect;

    [SerializeField, Header("発信者")]
    private GameObject senderObj = null;

    private bool isActive;

    private const float RIPPLE_TIME = 2;

    private const float RIRGT_MAX = 1280;
    private const float LEFT_MAX = 0;
    private const float UP_MAX = 720;
    private const float DOWN_MAX = 0;

    private float currentRippleTimer;

    private Vector2 rippleScale;
    private float rippleAlfa;
    private float red, green, blue;

    private Image spriteImage;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        isActive = false;
        currentRippleTimer = 0;
        imageRect = sprite.GetComponent<RectTransform>();
        rippleScale = Vector2.zero;
        spriteImage = sprite.GetComponent<Image>();
        rippleAlfa = 1.5f;
        red = spriteImage.color.r;
        green = spriteImage.color.g;
        blue = spriteImage.color.b;
    }

    // Update is called once per frame
    void Update()
    {

        if (GamePlayManager.instance.GameState != GamePlayManager.GamePlayStates.Play)
        {
            sprite.SetActive(false);
            return;
        }


        //出ているとき
        if (isActive)
        {
            currentRippleTimer += Time.deltaTime;
            rippleScale += new Vector2(2, 2);
            imageRect.sizeDelta = rippleScale;
            rippleAlfa -= 0.01f;
            spriteImage.color = new Color(red, green, blue, rippleAlfa);

            if (currentRippleTimer >= RIPPLE_TIME)
            {
                isActive = false;
                //sprite.SetActive(false);
                currentRippleTimer = 0;
                rippleScale = Vector2.zero;
                imageRect.sizeDelta = rippleScale;
                rippleAlfa = 1.5f;
                spriteImage.color = new Color(red, green, blue, rippleAlfa);
            }
        }

        else
        {
            MoveUI();
        }
    }

    public void Ripple()
    {
        var weather = GamePlayManager.instance.Weather;

        if (weather == GamePlayManager.WeatherStates.Squall)
            return;

        MoveUI();
        isActive = true;

        sprite.SetActive(true);
        
    }

    private void MoveUI()
    {
        
        //ワールド座標をスクリーン座標に
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, senderObj.transform.position);
        

        if (pos.x < LEFT_MAX)
        {
            pos.x = LEFT_MAX;
        }

        if (pos.x > RIRGT_MAX)
        {
            pos.x = RIRGT_MAX;
        }

        if (pos.y > UP_MAX)
        {
            pos.y = UP_MAX;
        }

        if (pos.y < DOWN_MAX)
        {
            pos.y = DOWN_MAX;
        }

        imageRect.transform.position = pos;
        
    }
}
