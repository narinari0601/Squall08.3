using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverRight : MonoBehaviour
{
    [SerializeField]
    private GameObject riverObj = null;

    private RiverCol riverCol;

    void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        riverCol = riverObj.GetComponent<RiverCol>();
    }


    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject;

        if (obj.tag == "Player")
        {
            //Jump(obj);

            var weather = GamePlayManager.instance.Weather;
            if (weather != GamePlayManager.WeatherStates.Squall
                /*|| !riverCol.IsJump*/)
                return;
            
            var squallDir = GamePlayManager.instance.SquallDirection;
            riverCol.RightJump(obj, squallDir);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.gameObject;

        if (obj.tag == "Player")
        {
            riverCol.IsLeftHit = false;
            riverCol.Col.isTrigger = false;
            obj.GetComponent<Playercontrol>().IsJump = false;
        }
    }

    private void Jump(GameObject obj)
    {
        

        //Debug.Log("風の方向 : " + GamePlayManager.instance.SquallDirection);
        //Debug.Log("川の向き : " + riverCol.Direction);

        var weather = GamePlayManager.instance.Weather;
        if (weather != GamePlayManager.WeatherStates.Squall
            /*|| !riverCol.IsJump*/)
            return;

        var squallDir = GamePlayManager.instance.SquallDirection;
        var riverDir = riverCol.Direction;

        if (squallDir == GamePlayManager.SquallDirections.Left &&
           riverDir == RiverCol.Directions.Vertical)
        {
            obj.transform.position += new Vector3(-riverCol.ColSizeX - 0.5f, 0, 0);
        }

        if (squallDir == GamePlayManager.SquallDirections.Up &&
           riverDir == RiverCol.Directions.Horizontal)
        {
            obj.transform.position += new Vector3(0, 0, riverCol.ColSizeX + 0.5f);
        }

        //if (squallDir == GamePlayManager.SquallDirections.Right &&
        //   riverDir == RiverCol.Directions.Vertical)
        //{
        //    obj.transform.position += new Vector3(riverCol.ColSizeX + 0.5f, 0, 0);
        //}

        //if (squallDir == GamePlayManager.SquallDirections.Down &&
        //   riverDir == RiverCol.Directions.Horizontal)
        //{
        //    obj.transform.position += new Vector3(0, 0, -riverCol.ColSizeX - 0.5f);
        //}
    }
}
