using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemberSpriteAnimation : MonoBehaviour
{
    public Sprite[] UpSpriteAni;
    public Sprite[] DownSpriteAni;
    public Sprite[] LeftSpriteAni;
    public Sprite[] RightSpriteAni;
    public SpriteRenderer spriteRenderer;
    //アニメーションフレーム設定
    [SerializeField, Header("アニメーションフレーム設定")]
    public float AnimationFrame;
    Coroutine runCoroutine;
    public MemberControl memberControl;

    public enum MemberDirection
    {
        Up,
        Down,
        Left,
        Right,
        UpStop,
        DownStop,
        LeftStop,
        RightStop,
    }
    private MemberDirection memberDirection;
    public MemberDirection GetMemberDirection { get => memberDirection; set => memberDirection = value; }

    void Start()
    {
        spriteRenderer.sprite = UpSpriteAni[0];
        AnimeStart();
    }
    void Update()
    {
        AnimeStart();
        MemberAni();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            memberDirection = MemberDirection.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            memberDirection = MemberDirection.Down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            memberDirection = MemberDirection.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            memberDirection = MemberDirection.Right;
        }
    }
 
    public void AnimeStart()
    {
        if (runCoroutine == null)
        {
            runCoroutine = StartCoroutine(MemberAni());
        }
    }

    IEnumerator MemberAni()
    {
        if (memberControl.GetMemberDirection == MemberControl.MemberDirection.UpStop)
        {
            spriteRenderer.sprite = UpSpriteAni[1];
        }
        else if (memberControl.GetMemberDirection == MemberControl.MemberDirection.DownStop)
        {
            spriteRenderer.sprite = DownSpriteAni[1];
        }
        else if (memberControl.GetMemberDirection == MemberControl.MemberDirection.LeftStop)
        {
            spriteRenderer.sprite = LeftSpriteAni[1];
        }
        else if (memberControl.GetMemberDirection == MemberControl.MemberDirection.RightStop)
        {
            spriteRenderer.sprite = RightSpriteAni[1];
        }
        //////////////////////////////ここから動いてるとき
        else if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Up)
        {
            for (int i = 0; i < UpSpriteAni.Length; i++)
            {
                if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Up)
                {
                    spriteRenderer.sprite = UpSpriteAni[i];
                    yield return new WaitForSeconds(AnimationFrame);
                }
                else
                    runCoroutine = null;
            }
        }
        else if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Down)
        {
            for (int i = 0; i < DownSpriteAni.Length; i++)
            {
                if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Down)
                {
                    spriteRenderer.sprite = DownSpriteAni[i];
                    yield return new WaitForSeconds(AnimationFrame);
                }
                else
                    runCoroutine = null;
            }
        }
        else if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Left)
        {
            for (int i = 0; i < LeftSpriteAni.Length; i++)
            {
                if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Left)
                {
                    spriteRenderer.sprite = LeftSpriteAni[i];
                    yield return new WaitForSeconds(AnimationFrame);
                }
                else
                    runCoroutine = null;
            }
        }
        else if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Right)
        {
            for (int i = 0; i < RightSpriteAni.Length; i++)
            {
                if (memberControl.GetMemberDirection == MemberControl.MemberDirection.Right)
                {
                    spriteRenderer.sprite = RightSpriteAni[i];
                    yield return new WaitForSeconds(AnimationFrame);
                }
                else
                    runCoroutine = null;
            }
        }
        runCoroutine = null;

    }
}
