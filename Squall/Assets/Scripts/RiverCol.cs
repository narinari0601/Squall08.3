using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverCol : MonoBehaviour
{
    public enum Directions
    {
        Vertical,
        Horizontal,
        Other,
    }


    [SerializeField]
    private BoxCollider col = null;

    private Transform riverTransform;

    private float objAngle;

    private Directions direction;

    private bool isJump;

    private float colSize;

    public Directions Direction { get => direction; set => direction = value; }
    public bool IsJump { get => isJump; set => isJump = value; }
    public float ColSizeX { get => colSize; set => colSize = value; }

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

        //else if (objAngle == 180)
        //    direction = Directions.Right;

        //else if (objAngle == 270)
        //    direction = Directions.Down;

        else
        {
            direction = Directions.Other;
            colSize = 2;
        }
        

        if (colSize < 2)
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
