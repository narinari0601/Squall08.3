using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverCol : MonoBehaviour
{
    public enum Direction
    {
        Left,
        Up,
        Right,
        Down,
        Other,
    }


    [SerializeField]
    private BoxCollider col;
    [SerializeField]
    private BoxCollider leftTrigger;
    [SerializeField]
    private BoxCollider rightTrigger;

    private Transform riverTransform;

    private float objAngle;

    private Direction direction;



    void Start()
    {
        riverTransform = GetComponent<Transform>();
        objAngle = riverTransform.localEulerAngles.y;

        direction = Direction.Other; //警告出るからとりあえずこれで初期化

        if (objAngle == 0)
            direction = Direction.Left;

        else if (objAngle == 90)
            direction = Direction.Up;

        else if (objAngle == 180)
            direction = Direction.Right;

        else if (objAngle == 270)
            direction = Direction.Down;

        else
            direction = Direction.Other;


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void Jump(GameObject player)
    {
        var weather = GamePlayManager.instance.Weather;

        if (weather != GamePlayManager.WeatherStates.Squall)
            return;


        var squallDirect = GamePlayManager.instance.SquallDirection;
        

    }
}
