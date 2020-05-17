using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquallSwitch : MonoBehaviour
{
    private GamePlayManager.SquallDirections dir;

    private Transform riverTransform;

    private float objAngle;

    private bool isInsert;

    //描画関連
    private SpriteRenderer spriteRenderer;

    [SerializeField, Header("押されてないときスプライト")]
    private Sprite usuallySprite = null;  

    [SerializeField, Header("押してあるときスプライト")]
    private Sprite effectSprite = null;  


    void Start()
    {
        riverTransform = GetComponent<Transform>();
        objAngle = riverTransform.localEulerAngles.y;

        if (objAngle == 0)
        {
            dir = GamePlayManager.SquallDirections.Left;
        }

        else if (objAngle == 90)
        {
            dir = GamePlayManager.SquallDirections.Up;
        }

        else if (objAngle == 180)
        {
            dir = GamePlayManager.SquallDirections.Right;
        }

        else if (objAngle == 270)
        {
            dir = GamePlayManager.SquallDirections.Down;
        }

        isInsert = false;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        isInsert = GamePlayManager.instance.CurrentStage.IsInsert;

        if (isInsert)
        {
            spriteRenderer.sprite = effectSprite;
        }

        else
        {
            spriteRenderer.sprite = usuallySprite;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        var obj = other.gameObject;

        if (obj.tag == "Player")
        {
            GamePlayManager.instance.CurrentStage.InterrectWind(dir);
        }
    }
}
