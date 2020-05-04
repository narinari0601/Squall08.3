using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverCol : MonoBehaviour
{
    public enum Directions
    {
        Vertical,  //縦向き(0度)
        Horizontal,  //横向き(90度)
        Other,
    }


    [SerializeField]
    private BoxCollider col = null;

    private Transform riverTransform;

    private float objAngle;

    private Directions direction;

    private GamePlayManager.SquallDirections jumpDir;

    //private bool isJump;

    private float colSize;

    private bool isRightHit;  //右から当たっていたらtrue
    private bool isLeftHit;  //左から当たっていたらtrue

    private GameObject jumpTarget;
    //private Transform jumpTarget;
    private Playercontrol pc;

    //private float jumpDistance;  //ジャンプの飛距離

    private float currentJumpDistance;

    //private BoxCollider boxCollider;

    public Directions Direction { get => direction; set => direction = value; }
    //public bool IsJump { get => isJump; set => isJump = value; }
    public float ColSizeX { get => colSize; set => colSize = value; }
    public bool IsRightHit { get => isRightHit; set => isRightHit = value; }
    public bool IsLeftHit { get => isLeftHit; set => isLeftHit = value; }
    public BoxCollider Col { get => col; set => col = value; }

    void Start()
    {
        riverTransform = GetComponent<Transform>();
        objAngle = riverTransform.localEulerAngles.y;

        direction = Directions.Other; //警告出るからとりあえずこれで初期化

        if (objAngle == 0)
        {
            direction = Directions.Vertical;
            colSize = col.bounds.size.x;
        }

        else if (objAngle == 90)
        {
            direction = Directions.Horizontal;
            colSize = col.bounds.size.z;
        }
        

        else
        {
            direction = Directions.Other;
            colSize = 100;
        }
        

        if (colSize < 2)
        {
            //isJump = true;
        }

        else
        {
            //isJump = false;
        }

        isRightHit = false;
        isLeftHit = false;
        jumpDir = GamePlayManager.SquallDirections.Up;
        
        currentJumpDistance = 0;

    }
    // Update is called once per frame
    void Update()
    {
        Jump();
    }

    private void Jump()
    {
        //if (isRightHit && jumpDir == GamePlayManager.SquallDirections.Left)
        //{
        //    pc.IsJump = true;
        //    jumpTarget.transform.position += new Vector3(-0.05f, 0, 0);
        //}

        //if (isRightHit && jumpDir == GamePlayManager.SquallDirections.Up)
        //{
        //    pc.IsJump = true;
        //    jumpTarget.transform.position += new Vector3(0, 0, 0.05f);
        //}

        //if (isLeftHit && jumpDir == GamePlayManager.SquallDirections.Right)
        //{
        //    pc.IsJump = true;
        //    jumpTarget.transform.position += new Vector3(0.05f, 0, 0);
        //}

        //if (isLeftHit && jumpDir == GamePlayManager.SquallDirections.Down)
        //{
        //    pc.IsJump = true;
        //    jumpTarget.transform.position += new Vector3(0, 0, -0.05f);
        //}
        

        if (currentJumpDistance >= colSize)
        {
            isRightHit = false;
            isLeftHit = false;
            col.isTrigger = false;
            pc.IsJump = false;
            currentJumpDistance = 0;
        }


        if (isRightHit)
        {
            if (jumpDir == GamePlayManager.SquallDirections.Left)
            {
                pc.IsJump = true;
                jumpTarget.transform.position += new Vector3(-0.05f, 0, 0);
                currentJumpDistance += 0.05f;
            }

            else if (jumpDir == GamePlayManager.SquallDirections.Up)
            {
                pc.IsJump = true;
                jumpTarget.transform.position += new Vector3(0, 0, 0.05f);
                currentJumpDistance += 0.05f;
            }
        }

        else if (isLeftHit)
        {
            if (jumpDir == GamePlayManager.SquallDirections.Right)
            {
                pc.IsJump = true;
                jumpTarget.transform.position += new Vector3(0.05f, 0, 0);
                currentJumpDistance += 0.05f;
            }

            else if (jumpDir == GamePlayManager.SquallDirections.Down)
            {
                pc.IsJump = true;
                jumpTarget.transform.position += new Vector3(0, 0, -0.05f);
                currentJumpDistance += 0.05f;
            }
            
        }
        

        


    }

    public void RightJump(GameObject obj, GamePlayManager.SquallDirections squallDir)
    {

        if ((squallDir == GamePlayManager.SquallDirections.Left && direction == RiverCol.Directions.Vertical) || //風が左向きで川が縦
            (squallDir == GamePlayManager.SquallDirections.Up && direction == RiverCol.Directions.Horizontal))   //風が上向きで川が横
        {
            isRightHit = true;
            col.isTrigger = true;
            jumpDir = squallDir;
            jumpTarget = obj;
            pc = jumpTarget.GetComponent<Playercontrol>();
        }
        
    }

    public void LeftJump(GameObject obj, GamePlayManager.SquallDirections squallDir)
    {
        if ((squallDir == GamePlayManager.SquallDirections.Right && direction == RiverCol.Directions.Vertical) ||  //風が右向きで川が縦
            (squallDir == GamePlayManager.SquallDirections.Down && direction == RiverCol.Directions.Horizontal))   //風が下向きで川が横
        {
            isLeftHit = true;
            col.isTrigger = true;
            jumpDir = squallDir;
            jumpTarget = obj;
            pc = jumpTarget.GetComponent<Playercontrol>();
        }
    }
    
}
