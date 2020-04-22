using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RippleUI : MonoBehaviour
{
    [SerializeField, Header("Imageオブジェクト")]
    private GameObject sprite = null;

    [SerializeField,Header("Imageオブジェクト達")]
    private GameObject[] sprites = new GameObject[0];
    
    private RectTransform imageRect01;
    private RectTransform imageRect02;
    private RectTransform imageRect03;

    [SerializeField, Header("発信者")]
    private GameObject senderObj = null;

    private bool isActive01;
    private bool isActive02;
    private bool isActive03;

    private const float RIRGT_MAX = 1280;
    private const float LEFT_MAX = 0;
    private const float UP_MAX = 720;
    private const float DOWN_MAX = 0;
    private const float SIZEDELTA_MAX = 256;  //波紋が消える大きさ
    private const float BORDERLINE01 = 66;    //波紋が3つから2つになる割合
    private const float BORDERLINE02 = 33;    //波紋が2つから1つになる割合
    
    private Vector2 rippleScale01;
    private Vector2 rippleScale02;
    private Vector2 rippleScale03;
    
    private float rippleAlfa01;
    private float rippleAlfa02;
    private float rippleAlfa03;

    private float red, green, blue;
    
    private Image spriteImage01;
    private Image spriteImage02;
    private Image spriteImage03;

    private bool isMember;

    private MemberControl memberControl;
    private float memberMaxHp;
    private float currentMemberHp;
    private int hpRetio;  //HPの割合


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        if (senderObj.tag == "Member")
            isMember = true;

        else
            isMember = false;
        
        isActive01 = false;
        
        imageRect01 = sprites[0].transform as RectTransform;
        
        rippleScale01 = Vector2.zero;
        rippleScale02 = Vector2.zero;
        rippleScale03 = Vector2.zero;
        
        spriteImage01 = sprites[0].GetComponent<Image>();
        
        rippleAlfa01 = 1.5f;
        rippleAlfa02 = 1.5f;
        rippleAlfa03 = 1.5f;

        red = spriteImage01.color.r;
        green = spriteImage01.color.g;
        blue = spriteImage01.color.b;

        //memberControl = transform.parent.gameObject.GetComponent<MemberControl>();
        //memberMaxHp = memberControl.GetMaxMemberHp();
        //currentMemberHp = memberControl.GetMemberHp();
        //hpRetio = (int)(currentMemberHp / memberMaxHp * 100);

        if (isMember)
        {
            isActive02 = false;
            isActive03 = false;

            imageRect02 = sprites[1].transform as RectTransform;
            imageRect03 = sprites[2].transform as RectTransform;

            spriteImage02 = sprites[1].GetComponent<Image>();
            spriteImage03 = sprites[2].GetComponent<Image>();

            memberControl = transform.parent.gameObject.GetComponent<MemberControl>();
            memberMaxHp = memberControl.GetMaxMemberHp();
            currentMemberHp = memberControl.GetMemberHp();
            hpRetio = (int)(currentMemberHp / memberMaxHp * 100);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isMember)
        {
            currentMemberHp = memberControl.GetMemberHp();
            hpRetio = (int)(currentMemberHp / memberMaxHp * 100);
        }
        
        Shout();
    }

    public void Ripple()
    {
        var weather = GamePlayManager.instance.Weather;

        if (weather == GamePlayManager.WeatherStates.Squall)
        {
            ResetUI();
            return;
        }

        
        isActive01 = true;
        MoveUI();

        foreach (var sprite in sprites)
        {
            sprite.SetActive(true);
        }

    }

    private void Shout()
    {
        if (GamePlayManager.instance.GameState != GamePlayManager.GamePlayStates.Play)
        {

            foreach (var sprite in sprites)
            {
                sprite.SetActive(false);
            }
            return;
        }

        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, senderObj.transform.position);
        if (pos.x > LEFT_MAX && pos.x < RIRGT_MAX && pos.y > DOWN_MAX && pos.y < UP_MAX)
        {
            imageRect01.transform.position = pos;
            if (isMember)
            {
                imageRect02.transform.position = pos;
                imageRect03.transform.position = pos;
            }
        }


        if (isActive01)
        {
            rippleScale01 += new Vector2(2.5f, 2.5f);
            imageRect01.sizeDelta = rippleScale01;
            rippleAlfa01 -= 0.01f;
            spriteImage01.color = new Color(red, green, blue, rippleAlfa01);

            if (isMember &&
                !isActive02 &&
                rippleScale01.x > 96 &&
                hpRetio > BORDERLINE01)
            {
                isActive02 = true;
            }

            if (rippleScale01.x > SIZEDELTA_MAX)
            {
                isActive01 = false;
                rippleScale01 = Vector2.zero;
                imageRect01.sizeDelta = rippleScale01;
                rippleAlfa01 = 1.5f;
                spriteImage01.color = new Color(red, green, blue, rippleAlfa01);
            }
        }

        if (isActive02)
        {
            rippleScale02 += new Vector2(2.5f, 2.5f);
            imageRect02.sizeDelta = rippleScale02;
            rippleAlfa02 -= 0.01f;
            spriteImage02.color = new Color(red, green, blue, rippleAlfa02);

            if (isMember &&
                !isActive03 &&
                rippleScale02.x > 96 &&
                hpRetio > BORDERLINE02)
            {
                isActive03 = true;
            }

            if (rippleScale02.x > SIZEDELTA_MAX)
            {
                isActive02 = false;
                rippleScale02 = Vector2.zero;
                imageRect02.sizeDelta = rippleScale02;
                rippleAlfa02 = 1.5f;
                spriteImage02.color = new Color(red, green, blue, rippleAlfa02);
            }
        }

        if (isActive03)
        {
            rippleScale03 += new Vector2(2.5f, 2.5f);
            imageRect03.sizeDelta = rippleScale03;
            rippleAlfa03 -= 0.01f;
            spriteImage03.color = new Color(red, green, blue, rippleAlfa03);

            if (rippleScale03.x > SIZEDELTA_MAX)
            {
                isActive03 = false;
                rippleScale03 = Vector2.zero;
                imageRect03.sizeDelta = rippleScale03;
                rippleAlfa03 = 1.5f;
                spriteImage03.color = new Color(red, green, blue, rippleAlfa03);
            }
        }
    }

    private void MoveUI()
    {
        
        //ワールド座標をスクリーン座標に
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, senderObj.transform.position);


        if (pos.x > LEFT_MAX && pos.x < RIRGT_MAX && pos.y > DOWN_MAX && pos.y < UP_MAX)
        {
            imageRect01.transform.position = pos;
            if (isMember)
            {
                imageRect02.transform.position = pos;
                imageRect03.transform.position = pos;
            }
        }

        else
        {
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
        }
        
        imageRect01.transform.position = pos;
        if (isMember)
        {
            imageRect02.transform.position = pos;
            imageRect03.transform.position = pos;
        }

    }

    private void ResetUI()
    {
        isActive01 = false;
        isActive02 = false;
        isActive03 = false;

        rippleScale01 = Vector2.zero;
        rippleScale02 = Vector2.zero;
        rippleScale03 = Vector2.zero;

        imageRect01.sizeDelta = rippleScale01;
        imageRect02.sizeDelta = rippleScale02;
        imageRect03.sizeDelta = rippleScale03;

        rippleAlfa01 = 1.5f;
        rippleAlfa02 = 1.5f;
        rippleAlfa03 = 1.5f;
    }
}
