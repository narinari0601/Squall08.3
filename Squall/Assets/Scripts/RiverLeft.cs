using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiverLeft : MonoBehaviour
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
            Jump(obj);
        }
    }

    private void Jump(GameObject obj)
    {
        var weather = GamePlayManager.instance.Weather;
        if (weather != GamePlayManager.WeatherStates.Squall ||
            !riverCol.IsJump)
            return;

        var squallDir = GamePlayManager.instance.SquallDirection;
        var riverDir = riverCol.Direction;

        if (squallDir == GamePlayManager.SquallDirections.Right &&
           riverDir == RiverCol.Directions.Left)
        {
            obj.transform.position += new Vector3(2, 0, 0);
        }

        if (squallDir == GamePlayManager.SquallDirections.Down &&
           riverDir == RiverCol.Directions.Up)
        {
            obj.transform.position += new Vector3(0, 0, -2);
        }

        if (squallDir == GamePlayManager.SquallDirections.Left &&
           riverDir == RiverCol.Directions.Right)
        {
            obj.transform.position += new Vector3(-2, 0, 0);
        }

        if (squallDir == GamePlayManager.SquallDirections.Up &&
           riverDir == RiverCol.Directions.Down)
        {
            obj.transform.position += new Vector3(0, 0, 2);
        }
    }
}
