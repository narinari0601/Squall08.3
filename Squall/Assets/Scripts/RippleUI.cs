using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RippleUI : MonoBehaviour
{
    [SerializeField, Header("Imageオブジェクト")]
    private GameObject sprite = null;

    private RectTransform imagePos;

    [SerializeField, Header("発信者")]
    private GameObject senderObj;

    private bool isActive;

    private const float RIPPLE_TIME = 2;

    private const float RIRGT_MAX = 1280;
    private const float LEFT_MAX = 0;
    private const float UP_MAX = 720;
    private const float DOWN_MAX = 0;

    private float currentRippleTimer;

    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        isActive = false;
        currentRippleTimer = 0;
        imagePos = sprite.GetComponent<RectTransform>();
        player = GamePlayManager.instance.Player;
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveUI();

        //出ているとき
        if (isActive)
        {
            currentRippleTimer += Time.deltaTime;

            if (currentRippleTimer >= RIPPLE_TIME)
            {
                isActive = false;
                sprite.SetActive(false);
                currentRippleTimer = 0;
            }
        }
    }

    public void Ripple()
    {
        sprite.SetActive(true);
        isActive = true;
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

        imagePos.transform.position = pos;
        
    }
}
