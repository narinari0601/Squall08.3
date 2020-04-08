using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverCol : MonoBehaviour
{
    public enum Directions
    {
        Left,
        Up,
        Right,
        Down,
        Other,
    }


    [SerializeField]
    private BoxCollider col;

    private Transform riverTransform;

    private float objAngle;

    private Directions direction;

    private bool isJump;

    public Directions Direction { get => direction; set => direction = value; }
    public bool IsJump { get => isJump; set => isJump = value; }

    void Start()
    {
        riverTransform = GetComponent<Transform>();
        objAngle = riverTransform.localEulerAngles.y;

        direction = Directions.Other; //警告出るからとりあえずこれで初期化

        if (objAngle == 0)
            direction = Directions.Left;

        else if (objAngle == 90)
            direction = Directions.Up;

        else if (objAngle == 180)
            direction = Directions.Right;

        else if (objAngle == 270)
            direction = Directions.Down;

        else
            direction = Directions.Other;


        if (col.bounds.size.x < 2)
        {
            isJump = true;
        }

        else
        {
            isJump = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    
}
